using System;
using System.Collections.Generic;
using FlowersShop_CourseWork.Models;

namespace FlowersShop_CourseWork.Services;

public class SearchHelper
{
    public static int LevenshteinDistance(string source, string target)
    {
        if (string.IsNullOrEmpty(source)) return string.IsNullOrEmpty(target) ? 0 : target.Length;
        if (string.IsNullOrEmpty(target)) return source.Length;

        source = source.ToLower();
        target = target.ToLower();

        int n = source.Length;
        int m = target.Length;
        int[,] d = new int[n + 1, m + 1];

        for (int i = 0; i <= n; d[i, 0] = i++)
        {
        }

        for (int j = 0; j <= m; d[0, j] = j++)
        {
        }

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }

        return d[n, m];
    }

    public static Flower BinarySearchById(List<Flower> sortedList, string targetId)
    {
        int left = 0;
        int right = sortedList.Count - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int comparison = string.Compare(sortedList[mid].Id, targetId, StringComparison.Ordinal);

            if (comparison == 0)
                return sortedList[mid];

            if (comparison < 0)
                left = mid + 1;
            else
                right = mid - 1;
        }

        return null;
    }

    public static void QuickSortFlowers(List<Flower> list, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(list, left, right);
            QuickSortFlowers(list, left, pivotIndex - 1); 
            QuickSortFlowers(list, pivotIndex + 1, right); 
        }
    }

    private static int Partition(List<Flower> list, int left, int right)
    {
        Flower pivot = list[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            bool jInStock = (list[j].StockQuantity > 0);
            bool pivotInStock = (pivot.StockQuantity > 0);

            bool shouldSwap = false;

            if (jInStock && !pivotInStock)
            {
                shouldSwap = true;
            }
            else if (jInStock == pivotInStock)
            {
                if (list[j].Price < pivot.Price)
                {
                    shouldSwap = true;
                }
            }

            if (shouldSwap)
            {
                i++;
                Swap(list, i, j);
            }
        }
        Swap(list, i + 1, right);
        return i + 1;
    }

    private static void Swap(List<Flower> list, int a, int b)
    {
        Flower temp = list[a];
        list[a] = list[b];
        list[b] = temp;
    }
}