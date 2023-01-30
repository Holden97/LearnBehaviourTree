using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Math.Sort
{
    class QuickSort
    {
        static int Partition(int[] array, int low, int high)
        {
            //1. Select a pivot point.
            int pivot = array[high];

            int lowIndex = (low - 1);

            //2. Reorder the collection.
            for (int j = low; j < high; j++)
            {
                if (array[j] <= pivot)
                {
                    lowIndex++;

                    int temp = array[lowIndex];
                    array[lowIndex] = array[j];
                    array[j] = temp;
                }
            }

            int temp1 = array[lowIndex + 1];
            array[lowIndex + 1] = array[high];
            array[high] = temp1;

            return lowIndex + 1;
        }

        //example
        //int[] array = { 72, 12, 6, 33, 81, 97, 37, 59, 52, 1, 20 };
        //int length = array.Length;
        //Sort(array, 0, length - 1);

        public static void Sort(int[] array, int low, int high)
        {
            if (low < high)
            {
                int partitionIndex = Partition(array, low, high);

                //3. Recursively continue sorting the array
                Sort(array, low, partitionIndex - 1);
                Sort(array, partitionIndex + 1, high);
            }
        }
    }
}
