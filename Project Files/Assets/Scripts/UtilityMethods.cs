using System.Collections.Generic;
using UnityEngine;

public static class UtilityMethods
{
    public static V GetDictValue<K, V>(this Dictionary<K, V> dict, K key)
    {
        V buffer;
        dict.TryGetValue(key, out buffer);
        return buffer;
    }

    public static Dictionary<K, V> ChainAdd<K, V>(this Dictionary<K, V> dict, K key, V value)
    {
        dict.Add(key, value);
        return dict;
    }

    public static LinkedListNode<T> GetNextNode<T>(this LinkedListNode<T> node, bool forwards)
        => forwards ? (node.Next ?? node.List.First) : (node.Previous ?? node.List.Last);
    
    public static LinkedList<T> ChainAddLast<T>(this LinkedList<T> list, T value)
    {
        list.AddLast(value);
        return list;
    }

    public static bool IsApproximatelyDivisibleBy(this float subject, int divisor, float errorMargin)
    {
        if (Mathf.Abs((float)((int)subject) - subject) <= errorMargin)
            if ((int)subject % divisor == 0)
                return true;
        return false;
    }
}