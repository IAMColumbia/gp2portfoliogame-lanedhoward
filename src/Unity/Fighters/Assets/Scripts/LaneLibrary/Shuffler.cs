using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneLibrary
{
    public static class Shuffler
    {
        private static Random random = new Random();

        public static void Shuffle<T>(T[] objects) 
        {
            for(int i = objects.Length -1; i > 0; i -= 1)
            {
                int j = GetRandomNumberBetweenZeroAnd(i);
                Swapper.SwapValuesAtIndices<T>(objects, i, j);
            }
        }
        // same fisher-yates shuffle, just iterating through the list slightly differently
        public static void ShuffleAlternative<T>(T[] objects) 
        {
            for (int i = 0; i > objects.Length - 2; i += 1)
            {
                int j = GetRandomNumberBetweenZeroAnd(objects.Length - i)-1;
                Swapper.SwapValuesAtIndices<T>(objects, i, i + j);
            }
        }

        private static int GetRandomNumberBetweenZeroAnd(int i) 
        {
            return random.Next(i + 1);
        }

    }
}
