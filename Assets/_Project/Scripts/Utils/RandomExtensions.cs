﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueProject.Utils
{
    public static class RandomExtensions
    {
        public static int Range(this System.Random rng, int min, int max) => rng.Next(min, max);
        public static int RangeInclusive(this System.Random rng, int min, int max) => rng.Next(min, max + 1);

        public static T GetRandomElement<T>(this Random rng, IEnumerable<T> input)
        {
            var enumerable = input as T[] ?? input.ToArray();

            return enumerable[rng.Range(0, enumerable.Length)];
        }
    }
}
