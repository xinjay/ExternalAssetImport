using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine.Profiling;

public class Test : MonoBehaviour
{
    private const string TexturePath = "Assets/Texture";
    [MenuItem("Test/GeneratTexture")]
    static void GeneratTexture()
    {
        var path = EditorUtility.SaveFolderPanel("存储路径", Application.dataPath, "");
        if (!string.IsNullOrEmpty(path))
        {
            //生成500张图片
            var count = 500;
            for (var index = 0; index < count; index++)
            {
                EditorUtility.DisplayProgressBar("生成图片", $"[{index + 1}/{count}]", (index + 1f) / count);
                var random = AssetImportUtils.GetRandomNameByTimeStamp();
                var filename = $"{path}/{random}.png";
                var texture = new Texture2D(128, 128);
                var bytes = texture.EncodeToPNG();
                File.WriteAllBytes(filename, bytes);
            }
            EditorUtility.ClearProgressBar();
        }
    }

    /// <summary>
    ///批量导入贴图
    /// </summary>
    [MenuItem("Test/ImportTexture")]
    static void ImportTexture()
    {
        var folder = EditorUtility.OpenFolderPanel("打开图片路径", Application.dataPath, "");
        if (!string.IsNullOrEmpty(folder))
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Reset();
            stopWatch.Start();
            var files = Directory.GetFiles(folder)
                .Where(file => file.EndsWith("png") || file.EndsWith("jpg") || file.EndsWith("exr"));
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var targetPath = $"{TexturePath}/{fileName}";
                File.Copy(file, targetPath, true);
                AssetDatabase.ImportAsset(targetPath);
            }
            stopWatch.Stop();
            var time = stopWatch.ElapsedMilliseconds;
            Debug.Log($"耗时:{time}ms");
        }
    }

    /// <summary>
    ///批量导入贴图（合批）
    /// </summary>
    [MenuItem("Test/ImportTextureInBatchMode")]
    static void ImportTextureInBatchMode()
    {
        var folder = EditorUtility.OpenFolderPanel("打开图片路径", Application.dataPath, "");
        if (!string.IsNullOrEmpty(folder))
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Reset();
            stopWatch.Start();
            try
            {
                AssetImportUtils.StartAssetEditing();
                var files = Directory.GetFiles(folder)
                    .Where(file => file.EndsWith("png") || file.EndsWith("jpg") || file.EndsWith("exr"));
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var targetPath = $"{TexturePath}/{fileName}";
                    File.Copy(file, targetPath, true);
                    AssetDatabase.ImportAsset(targetPath);
                }
            }
            finally
            {
                AssetImportUtils.StopAssetEditing();
            }
            stopWatch.Stop();
            var time = stopWatch.ElapsedMilliseconds;
            Debug.Log($"耗时:{time}ms");
        }
    }

    /// <summary>
    ///批量设置贴图
    /// </summary>
    [MenuItem("Test/TextureSetting")]
    static void TextureSetting()
    {
        var stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Reset();
        stopWatch.Start();
        var files = Directory.GetFiles(TexturePath)
            .Where(file => file.EndsWith("png") || file.EndsWith("jpg") || file.EndsWith("exr"));
        foreach (var file in files)
        {
            var importer = AssetImporter.GetAtPath(file) as TextureImporter;
            importer.isReadable = true;
            importer.SaveAndReimport();
            AssetDatabase.ImportAsset(file);
        }
        stopWatch.Stop();
        var time = stopWatch.ElapsedMilliseconds;
        Debug.Log($"耗时:{time}ms");
    }

    /// <summary>
    ///批量设置贴图（合批）
    /// </summary>
    [MenuItem("Test/TextureSettingInBatchMode")]
    static void TextureSettingInBatchMode()
    {
        var stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Reset();
        stopWatch.Start();
        try
        {
            var files = Directory.GetFiles(TexturePath)
                .Where(file => file.EndsWith("png") || file.EndsWith("jpg") || file.EndsWith("exr"));
            foreach (var file in files)
            {
                var importer = AssetImporter.GetAtPath(file) as TextureImporter;
                importer.isReadable = true;
                importer.SaveAndReimport();
                AssetDatabase.ImportAsset(file);
            }
        }
        finally
        {
            AssetImportUtils.StopAssetEditing();
        }
        stopWatch.Stop();
        var time = stopWatch.ElapsedMilliseconds;
        Debug.Log($"耗时:{time}ms");
    }


    [MenuItem("Test/MoreStartAssetEditing(界面无响应)")]
    static void MoreStartAssetEditing()
    {
        try
        {
            AssetDatabase.StartAssetEditing();
            AssetDatabase.StartAssetEditing();
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
        }
    }
    [MenuItem("Test/MoreStopAssetEditing(报错)")]
    static void MoreStoptAssetEditing()
    {
        try
        {
            AssetDatabase.StartAssetEditing();
            AssetDatabase.StopAssetEditing();
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
        }
    }

    [MenuItem("Test/InvalidImportInBatchMode")]
    static void InvalidImportInBatchMode()
    {
        var texture = new Texture2D(128, 128);
        var bytes = texture.EncodeToPNG();
        var path = "Assets/Test.png";
        if (File.Exists(path))//避免遗留数据影响
            AssetDatabase.DeleteAsset(path);
        try
        {
            AssetImportUtils.StartAssetEditing();
            File.WriteAllBytes(path, bytes);
            AssetDatabase.ImportAsset(path); //此时Import无效
            var importer = (TextureImporter)AssetImporter.GetAtPath(path); //importer=null;
            importer.isReadable = true; //抛出异常
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            AssetImportUtils.StopAssetEditing();
        }
    }
}
