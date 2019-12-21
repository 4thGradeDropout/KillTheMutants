using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringManipulation
{
    public static string ListToString<T>(List<T> list)
    {
        string result = "";
        list.ForEach(obj => result += obj.ToString() + " || ");
        return result;
    }
}
