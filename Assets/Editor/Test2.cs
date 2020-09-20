//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using UnityEngine;
//using UnityEditor;
//using UnityEngine.Profiling;

//public class Test2 : MonoBehaviour
//{
//    private const string TexturePath = "Assets/Texture";
//    /// <summary>
//    ///批量导入后设置格式
//    /// </summary>
//    [MenuItem("Test/ImportCase2")]
//    static void Import2()
//    {
//        var folder = EditorUtility.OpenFolderPanel("打开图片路径", Application.dataPath, "");
//        if (!string.IsNullOrEmpty(folder))
//        {
//            var stopWatch = new System.Diagnostics.Stopwatch();
//            stopWatch.Reset();
//            stopWatch.Start();
//            var files = Directory.GetFiles(folder)
//                .Where(file => file.EndsWith("png") || file.EndsWith("jpg") || file.EndsWith("exr"));
//            var list = new List<string>();
//            AssetImportUtils.StartAssetEditing();
//            foreach (var file in files)
//            {
//                var fileName = Path.GetFileName(file);
//                var targetPath = $"{TexturePath}/{fileName}";
//                File.Copy(file, targetPath, true);
//                AssetDatabase.ImportAsset(targetPath);
//                list.Add(targetPath);
//            }
//            AssetImportUtils.StopAssetEditing();
//            AssetDatabase.Refresh();
//            AssetImportUtils.StartAssetEditing();
//            foreach (var file in list)
//            {
//                var importer = AssetImporter.GetAtPath(file) as TextureImporter;
//                importer.isReadable = true;
//                importer.SaveAndReimport();
//                AssetDatabase.ImportAsset(file);
//            }
//            AssetImportUtils.StopAssetEditing();
//            AssetDatabase.Refresh();
//            stopWatch.Stop();
//            var time = stopWatch.ElapsedMilliseconds;
//            Debug.Log($"耗时:{time}ms");
//        }
//    }

//    /// <summary>
//    ///通过meta文件来设置格式
//    /// </summary>
//    [MenuItem("Test/ImportCase3")]
//    static void Import3()
//    {
//        var folder = EditorUtility.OpenFolderPanel("打开图片路径", Application.dataPath, "");

//        var stopWatch = new System.Diagnostics.Stopwatch();
//        stopWatch.Reset();
//        stopWatch.Start();
//        ExternalAssetImporter.ImportTexture(folder, TexturePath,
//            importer => { importer.isReadable = true; });
//        stopWatch.Stop();
//        var time = stopWatch.ElapsedMilliseconds;
//        Debug.Log($"耗时:{time}ms");

//    }

//    private const int times = 10000;
//    private const int length = 100000;
//    [MenuItem("Test/CreateArrayOnStack")]
//    static void CreateArrayOnStake()
//    {
//        Profiler.BeginSample("CreateArrayOnStack");
//        var stopWatch = new System.Diagnostics.Stopwatch();
//        stopWatch.Reset();
//        stopWatch.Start();
//        unsafe
//        {
//            for (var index = 0; index < times; index++)
//            {
//                CreateStackArray();
//            }
//        }
//        stopWatch.Stop();
//        var time = stopWatch.ElapsedMilliseconds;
//        Profiler.EndSample();
//        Debug.Log($"OnStake耗时:{time}ms");
//    }

//    static unsafe void CreateStackArray()
//    {
//        var array = stackalloc int[length];
//        for (var i = 0; i < length; i++)
//        {
//            array[i] = i;
//        }

//    }

//    [MenuItem("Test/CreateArrayOnHeap")]
//    static void CreateArrayOnHeap()
//    {
//        Profiler.BeginSample("CreateArrayOnHeap");
//        var stopWatch = new System.Diagnostics.Stopwatch();
//        stopWatch.Reset();
//        stopWatch.Start();

//        for (var index = 0; index < times; index++)
//        {
//            CreateHeapArray();
//        }
//        stopWatch.Stop();
//        var time = stopWatch.ElapsedMilliseconds;
//        Profiler.EndSample();
//        Debug.Log($"OnHeap耗时:{time}ms");
//    }

//    static void CreateHeapArray()
//    {
//        var array = new int[length];
//        for (var i = 0; i < length; i++)
//        {
//            array[i] = i;
//        }
//    }

//    void Update()
//    {
//        Profiler.BeginSample("CreateArrayOnStack");
//        CreateStackArray();
//        Profiler.EndSample();

//        Profiler.BeginSample("CreateArrayOnHeap");
//        CreateHeapArray();
//        Profiler.EndSample();

//    }

//    [MenuItem("Test/AssetEditing")]
//    static void Editing()
//    {
//        try
//        {
//            AssetDatabase.StartAssetEditing();
//            //Import,Refresh

//        }
//        finally
//        {
//            AssetDatabase.StopAssetEditing();
//        }

//        //

//    }
//}
