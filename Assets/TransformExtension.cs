using System;
using System.Collections.Generic;
using UnityEngine;
public static class TransformExtension
{
    public delegate bool TransformAction(Transform transfrom);
    public static string GetRoute(this Transform transform, string splitter = ".")
    {
        var result = transform.name;
        var parent = transform.parent;
        while (parent != null)
        {
            result = $"{parent.name}{splitter}{result}";
            parent = parent.parent;
        }
        return result;
    }

    public static void DFSGetChildren(this Transform transform, TransformAction action)
    {
        var childCount = transform.childCount;
        for (var index = 0; index < childCount; index++)
        {
            var child = transform.GetChild(index);
            if (action.Invoke(child))
                child.DFSGetChildren(action);
        }
    }
    public static void BFSGetChildren(this Transform transform, TransformAction action)
    {
        var childCount = transform.childCount;
        var transformList = new List<Transform>();
        for (var index = 0; index < childCount; index++)
        {
            var child = transform.GetChild(index);
            if (action.Invoke(child))
                transformList.Add(child);
        }
        for (var index = 0; index < transformList.Count; index++)
        {
            var child = transformList[index];
            child.BFSGetChildren(action);
        }
    }
}