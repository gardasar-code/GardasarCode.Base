namespace GardasarCode.Base.Extensions;

public static class NumberExtensions
{
    public static int GetFractionalPart(this float number)
    {
        return (int)((number - Math.Truncate(number)) * 1000);
    }

    public static int GetFractionalPart(this double number)
    {
        return (int)((number - Math.Truncate(number)) * 1000);
    }

    public static int GetFractionalPart(this decimal number)
    {
        return (int)(Math.Abs(number - Math.Truncate(number)) * 1000);
    }
}
