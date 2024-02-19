using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class ListUtility
{
    public static void Sort<T>(ref List<T> toSort, List<int> sortingIndexes)
    {
        int[] indexesCopy = new int[sortingIndexes.Count];
        sortingIndexes.CopyTo(0, indexesCopy, 0, sortingIndexes.Count);
        int i = 1;
        while (i < sortingIndexes.Count)
        {
            int buffer = indexesCopy[i];
            T valueBuffer = toSort[i];
            int j = i;
            while (j > 0 && indexesCopy[j - 1] > indexesCopy[j])
            {
                indexesCopy[j] = indexesCopy[j - 1];
                toSort[j] = toSort[j - 1];
                --j;
            }
            indexesCopy[j] = buffer;
            toSort[j] = valueBuffer;
            ++i;
        }
    }
}
