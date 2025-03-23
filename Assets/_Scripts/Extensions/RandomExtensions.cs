using System;

public static class RandomExtensions
{
    public static double GetRandomNumber(this Random random, double minimum, double maximum) => random.NextDouble() * (maximum - minimum) + minimum;
}