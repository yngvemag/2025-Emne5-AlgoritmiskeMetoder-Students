# En grundig innføring i `Span<T>` og `ReadOnlySpan<T>` i C# (med fokus på ASP.NET)

> **Kort oppsummert:** `Span<T>` er en **lettvekts, stack-allokert** "view" over sammenhengende minne (array, stack, usikret/innpinnet minne). Den lar deg **slice** og jobbe med data **uten kopieringer**, noe som gir mindre GC-press og bedre ytelse – spesielt i ASP.NET der hver allokasjon koster under last.

---

## Hvorfor bry seg i ASP.NET?

- Mindre allokasjoner → mindre GC-pådrag → jevnere responstider under last.
- Effektiv parsing/serialisering (JSON/CSV/protokoller).
- Tekstbehandling (ASCII/UTF-8) uten å materialisere nye strenger.
- Bufferhåndtering og streaming (knyttet til `System.IO.Pipelines` og `ReadOnlySequence<T>`).

Typiske scenarier:

- Lese/parse request-body eller headers.
- Validere/parse tokens, API-nøkler, UUID-er, datoer, tall.
- Effektiv fil-IO og nettverks-IO, eller konvertering mellom `byte` og `char` via `Encoding`-APIer.

---

## Grunnbegreper

### Hva er `Span<T>`?

- En **`ref struct`** som peker til en **sammenhengende** sekvens av elementer av typen `T`.
- Kan referere til:
  - Hele eller deler av en **array** (uten kopiering).
  - **Stack-allokert** minne via `stackalloc`.
  - **Umanaged** minne (via `unsafe`/pinned).
- Er **stack-bundet**: kan **ikke** løftes til heap (kan ikke være felt i klasser, kan ikke brukes over `await`, kan ikke “escape” metoden).
- Muterbar: du kan **skrive** til dataene den peker på.

### `ReadOnlySpan<T>`

- Samme ide, men **kun lesing**.
- Bruk som parameter når du ikke skal endre innholdet (bedre API-kontrakt).

### Slektskap til `Memory<T>` og `ReadOnlyMemory<T>`

- `Memory<T>`/`ReadOnlyMemory<T>` **kan lagres på heap** og overleve `await`, men må materialiseres via `.Span` for synkron tilgang.
- Bruk `Memory<T>` når du trenger å **beholde** et view over data **etter** en `await` eller eksponere det **ut av metoden**.

---

## Stack vs Heap: hvorfor det betyr noe

- **Stack**: svært rask allokering/deallokering, automatisk opprydding når scope ender, ingen GC. Begrenset størrelse (per tråd).
- **Heap**: krever GC, allokasjoner under last kan gi **GC-pauser** og **jitter** i responstid.

`Span<T>` lever på **stacken** (selve strukturen), og kan peke til data på stacken (via `stackalloc`) eller heap (arrays). Fordel: du kan **arbeide med data uten å kopiere** og uten ekstra heap-allokasjoner.

**Eksempel – stack-allokert buffer**:

```csharp
public static int SumOfAsciiDigits(ReadOnlySpan<byte> data)
{
    // Midlertidig arbeidsbuffer på stacken
    Span<int> counts = stackalloc int[10]; // 10 heltall på stacken, ingen GC
    foreach (var b in data)
    {
        if ((uint)(b - (byte)'0') <= 9u) counts[b - (byte)'0']++;
    }

    int weightedSum = 0;
    for (int i = 0; i < counts.Length; i++) weightedSum += i * counts[i];
    return weightedSum;
}
```

---

## Slicing uten kopieringer

```csharp
byte[] buffer = GetBuffer();
Span<byte> span = buffer;          // referanse til hele arrayen
Span<byte> header = span[..4];     // første 4 bytes (slice)
Span<byte> payload = span[4..];    // resten (slice)
```

- `header` og `payload` peker inn i **samme underliggende minne**; ingen kopiering.
- Supert til å parse protokoll-headere, lengdefelt, etc.

---

<div style="page-break-after:always;"></div>

## Trygg og effektiv parsing i ASP.NET

### Eksempel: parse en UUID sendt i en header

```csharp
public static bool TryParseUuidHeader(string? headerValue, out Guid id)
{
    id = default;
    if (string.IsNullOrWhiteSpace(headerValue)) return false;

    ReadOnlySpan<char> span = headerValue.AsSpan().Trim();
    return Guid.TryParse(span, out id); // unngår midlertidige strenger
}
```

### Eksempel: parse tall uten allokasjon

```csharp
public static bool TryParsePositiveInt(ReadOnlySpan<char> s, out int value)
{
    value = 0;
    foreach (char c in s)
    {
        if ((uint)(c - '0') > 9u) return false;
        value = checked(value * 10 + (c - '0'));
    }
    return true;
}
```

---
<div style="page-break-after:always;"></div>


## Tekst & encoding uten unødige kopier

```csharp
public static string Base64UrlEncode(ReadOnlySpan<byte> data)
{
    // Estimer lengde og stackalloker buffer for ytelse
    int len = ((data.Length + 2) / 3) * 4;
    Span<char> chars = stackalloc char[len];

    Convert.TryToBase64Chars(data, chars, out int written);
    // gjøre URL-trygg: bytt ut +/ og fjern =
    for (int i = 0; i < written; i++)
    {
        ref char ch = ref chars[i];
        if (ch == '+') ch = '-';
        else if (ch == '/') ch = '_';
    }

    int padding = (written >= 2 && chars[written - 1] == '=') ? 1 : 0;
    if (padding == 1 && written >= 2 && chars[written - 2] == '=') padding = 2;
    ReadOnlySpan<char> trimmed = chars[..(written - padding)];
    return new string(trimmed); // én strengeallokasjon til slutt
}
```

**Tips:** Mange BCL-APIer har `Span`-vennlige overloads som heter `TryXxx(Span/ReadOnlySpan...)`, f.eks. `Encoding.UTF8.GetBytes(ReadOnlySpan<char>, Span<byte>, ...)` og `Convert.TryToBase64Chars(...)`.

---

## `stackalloc` – når du trenger midlertidig buffer

```csharp
public static string ToHex(ReadOnlySpan<byte> bytes)
{
    Span<char> hex = stackalloc char[bytes.Length * 2];
    const string map = "0123456789abcdef";
    int i = 0;
    foreach (var b in bytes)
    {
        hex[i++] = map[b >> 4];
        hex[i++] = map[b & 0xF];
    }
    return new string(hex);
}
```

- Ingen heap-allokasjon for mellomresultat (kun sluttstrengen).
- **Pass på størrelsen**: store `stackalloc` kan gi stack overflow; hold deg til «små» buffere (typisk noen KB eller mindre).

---

## Viktige regler og begrensninger

1. `Span<T>`/`ReadOnlySpan<T>` er `ref struct` → **kan ikke**:
   - være **felt i en klasse** (men kan være lokal i en `struct ref`).
   - **box'es** eller **captur'es** av lambda/iterator.
   - krysse `await`/`yield` grenser.
2. De peker ofte inn i andres minne → **livstid/gyldighet** må respekteres:
   - Ikke returner en `Span<T>` som peker til stack/minne som ikke lenger er gyldig.
   - For arrays er det ok, fordi arrayen lever på heap – men viewet (`Span`) må fortsatt ikke «escape» på ulovlige måter (som felt i klasse).
3. Trenger du varighet utover metoden/await? → **bruk `Memory<T>`** eller kopier.

---

## Sammenligningstabell

| Egenskap | `Span<T>` | `ReadOnlySpan<T>` | `Memory<T>` | `ReadOnlyMemory<T>` |
|---|---|---|---|---|
| Muterbar | Ja | Nei | Ja (via `.Span`) | Nei (via `.Span`) |
| Kan lagres på heap | Nei | Nei | Ja | Ja |
| Kan krysse `await` | Nei | Nei | Ja | Ja |
| Kan peke til stack | Ja | Ja | Nei | Nei |
| Typisk bruk | rask, synkron prosessering | rask lesing | asynkron/bevaring | asynkron/bevaring (read-only) |

---
<div style="page-break-after:always;"></div>

## Integrasjon med `System.IO.Pipelines` og `ReadOnlySequence<T>`

I høyytelses ASP.NET-kode møter du ofte `PipeReader`/`PipeWriter` og `ReadOnlySequence<byte>`. Slik jobber du spans-basert:

```csharp
while (true)
{
    ReadResult result = await reader.ReadAsync();
    ReadOnlySequence<byte> buffer = result.Buffer;

    // Få et sammenhengende span hvis mulig
    if (buffer.IsSingleSegment)
    {
        ReadOnlySpan<byte> span = buffer.FirstSpan;
        Process(span); // spans-basert parsing
    }
    else
    {
        // Samle eller behandle segmentvis uten kopier der mulig
        foreach (var segment in buffer)
        {
            Process(segment.Span);
        }
    }

    reader.AdvanceTo(buffer.End);
    if (result.IsCompleted) break;
}
```

`ReadOnlySequence<T>` kan bestå av flere segmenter (ikke sammenhengende), men hvert segment gir deg et `ReadOnlySpan<T>`.

---
<div style="page-break-after:always;"></div>

## Praktiske ASP.NET-eksempler

### 1) Validering av API-key i middleware uten ekstra allokasjoner

```csharp
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    public ApiKeyMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext ctx)
    {
        if (ctx.Request.Headers.TryGetValue("X-Api-Key", out var values))
        {
            ReadOnlySpan<char> key = values.ToString().AsSpan().Trim();
            if (IsValidKey(key))
            {
                await _next(ctx);
                return;
            }
        }
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }

    private static bool IsValidKey(ReadOnlySpan<char> key)
    {
        // demo: krever 32 hex chars
        if (key.Length != 32) return false;
        foreach (var c in key)
        {
            bool hex = (c >= '0' && c <= '9') ||
                       (c >= 'a' && c <= 'f') ||
                       (c >= 'A' && c <= 'F');
            if (!hex) return false;
        }
        return true;
    }
}
```
<div style="page-break-after:always;"></div>

### 2) Parse `Content-Type` raskt

```csharp
public static (ReadOnlySpan<char> MediaType, ReadOnlySpan<char> Charset) ParseContentType(ReadOnlySpan<char> s)
{
    // f.eks: "application/json; charset=utf-8"
    int semi = s.IndexOf(';');
    ReadOnlySpan<char> media = semi >= 0 ? s[..semi].Trim() : s.Trim();
    ReadOnlySpan<char> charset = ReadOnlySpan<char>.Empty;

    if (semi >= 0)
    {
        var tail = s[(semi + 1)..].Trim();
        const string key = "charset=";
        if (tail.StartsWith(key, StringComparison.OrdinalIgnoreCase))
            charset = tail[key.Length..].Trim();
    }
    return (media, charset);
}
```

### 3) `string.Create` for effektiv formatting

```csharp
public static string FormatUserId(int id)
{
    return string.Create(10, id, (span, value) =>
    {
        // Fyll med "User-0001" stil
        ReadOnlySpan<char> prefix = "User-".AsSpan();
        prefix.CopyTo(span);
        span = span[prefix.Length..];
        // skriv 4 siffer med pad
        for (int i = 3; i >= 0; i--)
        {
            span[i] = (char)('0' + (value % 10));
            value /= 10;
        }
    });
}
```

---
<div style="page-break-after:always;"></div>

## Ytelsesråd

- **Unngå unødvendige kopier**: bruk `AsSpan()`, slicing og `Try`-overloads.
- **Begrens `stackalloc` størrelse**: gjerne < 1–2 KB per kall (konservativ tommelfinger).
- **Foretrekk `ReadOnlySpan<T>` i signaturer** når du ikke muterer – klarere intensjon, åpner for flere kallertyper (string/array/literals).
- **Mål, ikke gjett**: bruk BenchmarkDotNet og ASP.NET-profilering for å bekrefte gevinstene.
- **Pass på livstider**: ikke la en `Span<T>` leve for lenge eller forlate metoden på ulovlige måter.

---

## Når bør du **ikke** bruke `Span<T>`?

- Når koden blir **komplisert** uten målbar gevinst.
- Når du **må lagre** viewet over tid eller krysse `await` → bruk `Memory<T>` eller kopier.
- Når datastrukturen ikke er **sammenhengende** (vurder `ReadOnlySequence<T>`).

---

## Vanlige feil og feller

- **Returnere en `Span<T>`** som peker til stackallokert minne (UB).
- **Lagre `Span<T>` i felt/klasse** (forbudt – kompilatorfeil).
- **Bruke `Span<T>` i async-metoder over `await`** (kompilatorfeil).
- **For stor `stackalloc`** → stack overflow.

---

## Små kodebiter som er nyttige

**Kopiere uten allokasjon (til eksisterende buffer)**

```csharp
public static bool TryCopy(ReadOnlySpan<byte> src, Span<byte> dst, out int written)
{
    if (dst.Length < src.Length) { written = 0; return false; }
    src.CopyTo(dst);
    written = src.Length;
    return true;
}
```

**Case-insensitiv sammenligning uten nye strenger**

```csharp
public static bool EqualsOrdinalIgnoreCase(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
    => a.Equals(b, StringComparison.OrdinalIgnoreCase);
```

<div style="page-break-after:always;"></div>

## Hurtig sjekkliste

- Trenger du rask, allokasjonsfri prosessering **her og nå**? → `Span<T>`/`ReadOnlySpan<T>`.
- Trenger du å **beholde** viewet, eller krysse `await`? → `Memory<T>`/`ReadOnlyMemory<T>`.
- Skal du bare **lese**? → `ReadOnlySpan<T>` i API.
- Er datasettet lite og midlertidig? → vurder `stackalloc`.
- Må du jobbe med ikke-sammenhengende buffers? → `ReadOnlySequence<T>` + spans per segment.

---

## Oppsummering

`Span<T>` og `ReadOnlySpan<T>` er nøkkelverktøy for **høyytelses, allokasjonsbevisst** C#-kode – spesielt i ASP.NET. Bruk dem til å **slice**, **parse** og **transformere** data uten unødvendige kopier, hold buffere små med `stackalloc`, og velg `Memory<T>` når livstid/await krever det. Mål ytelsen, og bruk disse verktøyene der de faktisk gjør en målbar forskjell.
