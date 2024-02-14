using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListUtility
{
    public static void Sort<T>(ref List<T> toSort, List<int> sortingIndexes)
    {
        int i = 1;
        while (i < sortingIndexes.Count)
        {
            int buffer = sortingIndexes[i];
            T valueBuffer = toSort[i];
            int j = i;
            while (j > 0 && sortingIndexes[j - 1] > sortingIndexes[j])
            {
                sortingIndexes[j] = sortingIndexes[j - 1];
                toSort[j] = toSort[j - 1];
                --j;
            }
            sortingIndexes[j] = buffer;
            toSort[j] = valueBuffer;
            ++i;
        }
    }
}
