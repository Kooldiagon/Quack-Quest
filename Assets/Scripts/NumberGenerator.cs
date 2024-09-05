using System;
using System.Collections.Generic;
using System.Linq;

public class NumberGenerator
{
    private Random random;

    public NumberGenerator(int seed)
    {
        random = new Random(seed);
    }

    // Returns a random int 
    public int RandomInt(int min, int max)
    {
        return random.Next(min, max + 1);
    }
}
