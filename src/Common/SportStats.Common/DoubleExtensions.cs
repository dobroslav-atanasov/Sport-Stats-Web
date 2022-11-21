namespace SportStats.Common;

public static class DoubleExtensions
{
    private const double POUND = 0.45359237;

    public static double ConvertPoundToKilograms(this double value)
    {
        return value * POUND;
    }
}