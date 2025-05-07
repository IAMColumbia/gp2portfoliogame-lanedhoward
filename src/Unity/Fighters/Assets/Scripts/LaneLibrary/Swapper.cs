using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneLibrary
{
    public static class Swapper
    {

        public static void SwapValuesAtIndices<T>(T[] objects, int i, int j)
        {
            T temp = objects[i];
            objects[i] = objects[j];
            objects[j] = temp;
        }

        public static void SwapValuesAtIndices<T>(List<T> objects, int i, int j)
        {
            T temp = objects[i];
            objects[i] = objects[j];
            objects[j] = temp;
        }
    }
}
