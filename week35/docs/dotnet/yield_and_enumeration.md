# Forstå `yield` og state-machine i C# med GaList<T>

Denne dokumentasjonen forklarer hvordan `yield` fungerer i C#, hvordan kompilatoren lager en **state-machine** for enumerasjon, og hva `IEnumerator` og `IEnumerable<T>` betyr i denne sammenhengen.

---

## 1. Hva gjør `yield`?

Når du skriver en metode med `yield return`, som f.eks.:

```csharp
public IEnumerator<T> GetEnumerator()
{
    int startVersion = _version;
    for (int i = 0; i < _count; i++)
    {
        if (startVersion != _version)
            throw new InvalidOperationException("Collection was modified during enumeration.");
        yield return _items[i];
    }
}
```

så beskriver du **sekvensielt** hvordan elementene skal produseres. Kompilatoren:

1. Genererer en **skjult klassetype** (iterator-state-machine) som **implementerer `IEnumerator<T>`**.
2. Flytter lokale variabler (som `i` og `startVersion`) inn som **felter** i denne klassen.
3. Oversetter hvert `yield return` til en **pause** i `MoveNext()`:
   - Setter `Current`
   - Husker «hvor vi var» (state)
   - Returnerer `true`
4. Neste kall til `MoveNext()` **fortsetter** etter forrige `yield return`.

Kort sagt: `yield` lar deg skrive lesbar, sekvensiell kode som kompilatoren oversetter til en enumerator.

---
<div style="page-break-after:always;"></div>

## 2. Hvordan ser state-machine ut (forenklet)?

Forenklet pseudokode av generert enumerator:

```csharp
private sealed class GaListEnumerator : IEnumerator<T>
{
    private int _state;
    private T _current;
    private GaList<T> _list;
    private int _i;
    private int _startVersion;

    public bool MoveNext()
    {
        switch (_state)
        {
            case 0:
                _startVersion = _list._version;
                _i = 0;
                _state = 1;
                goto case 1;
            case 1:
                if (_i < _list._count)
                {
                    if (_startVersion != _list._version)
                        throw new InvalidOperationException("Collection was modified.");
                    _current = _list._items[_i];
                    _i++;
                    return true; // yield return
                }
                _state = -1;
                return false;
        }
        return false;
    }

    public T Current => _current;
    object IEnumerator.Current => _current!;
    public void Dispose() { _state = -2; }
}
```

Din `GetEnumerator()` returnerer i praksis en ny instans av denne state-machine-klassen.

---
<div style="page-break-after:always;"></div>

## 3. Hvorfor `startVersion`?

Snapshot av `_version` ved start sikrer **fail-fast**-iterasjon: hvis noen endrer listen under `foreach`, kastes et unntak i stedet for å risikere feil iterasjon.

---

## 4. Hva gjør `foreach`?

`foreach (var x in gaList)` oversettes til:

```csharp
using (var enumerator = gaList.GetEnumerator())
{
    while (enumerator.MoveNext())
    {
        var x = enumerator.Current;
        // løkkekropp
    }
}
```

`MoveNext()` kaller state-machine-logikken, og `Dispose()` kjøres til slutt (via `using`).

---

## 5. Hva er `IEnumerator` og `IEnumerable<T>`?

- **`IEnumerable<T>`**: Grensesnitt som eksponerer en metode `GetEnumerator()` som returnerer en `IEnumerator<T>`.
  - Lar en samling brukes i `foreach`.
  - Utfører ikke selve iterasjonen, men gir et objekt som gjør det.

```csharp
public interface IEnumerable<out T>
{
    IEnumerator<T> GetEnumerator();
}
```

- **`IEnumerator<T>`**: Representerer **selve enumeratoren**.
  - Har `MoveNext()`, `Current`, og `Dispose()`.
  - Utfører iterasjonen over elementene.

```csharp
public interface IEnumerator<out T> : IDisposable, IEnumerator
{
    T Current { get; }
    bool MoveNext();
    void Reset(); // sjelden brukt
}
```

**Likheter og forskjeller:**
- `IEnumerable<T>` = «container»; gir deg en ny enumerator hver gang.
- `IEnumerator<T>` = selve iterasjonsmekanismen; holder state underveis.

Når du bruker `yield`, skriver du en metode som returnerer `IEnumerable<T>` eller `IEnumerator<T>`. Kompilatoren genererer en privat klasse som implementerer `IEnumerator<T>` og returnerer den for deg.

---

## 6. Oppsummering

- `yield return` -> kompilatoren lager en skjult enumerator med state-machine.
- Lokale variabler blir felter i enumeratoren.
- `IEnumerator<T>` = iteratoren; `IEnumerable<T>` = gir iterator.
- `startVersion` + `_version` gir fail-fast under iterasjon.
- `foreach` bruker `GetEnumerator()`, `MoveNext()`, `Current` og `Dispose()` automatisk.
