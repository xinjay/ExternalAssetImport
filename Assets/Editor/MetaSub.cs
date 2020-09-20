using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[DisallowMultipleComponent]
public class MetaSub : MetaBase
{
    public string CC;

    [MenuItem("MetaTest/Replace2Base")]
    static void Replace2Base()
    {
        var go = Selection.activeGameObject;
        var sub = go.GetComponent<MetaSub>();
        var @base = go.AddComponent<MetaBase>();
        EditorUtility.CopySerializedManagedFieldsOnly(sub, @base);
        DestroyImmediate(sub);
    }

    [MenuItem("MetaTest/Replace2Sub")]
    static void Replace2Sub()
    {
        var go = Selection.activeGameObject;
        var sub = go.GetComponent<MetaSub>();
        var @base = go.AddComponent<MetaBase>();
        EditorUtility.CopySerializedManagedFieldsOnly(@base, sub);
        DestroyImmediate(@base);
       
    }
}
