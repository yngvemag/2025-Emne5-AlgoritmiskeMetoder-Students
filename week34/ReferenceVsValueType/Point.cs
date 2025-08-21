namespace ReferenceVsValueType;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public static void ChangePoint(Point p)
    {
        p.X = 100;
        p.Y = 200;
    }
    
    public static void ChangePointRef(ref Point p)
    {
        p.X = 300;
        p.Y = 400;
    }
}