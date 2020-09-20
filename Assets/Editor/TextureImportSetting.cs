using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class TextureImportSetting : IDisposable
{
    private const string ROOT = "Assets/";
    private Texture2D innerTexture;
    private TextureImporter innerTextureImporter;
    private string innerTexturePath;
    private string metaFile;
    private string metaContent;
    private string innerTextureGuid;

    public TextureImportSetting()
    {
        var ramdomName = AssetImportUtils.GetRandomNameByTimeStamp();
        innerTexturePath = $"{ROOT}{ramdomName}.png";
        innerTexture = new Texture2D(1, 1);
        var bytes = innerTexture.EncodeToPNG();
        File.WriteAllBytes(innerTexturePath, bytes);
        AssetDatabase.ImportAsset(innerTexturePath);

        innerTextureImporter = AssetImporter.GetAtPath(innerTexturePath) as TextureImporter;
        metaFile = $"{innerTexturePath}.meta";
        metaContent = File.ReadAllText(metaFile);
        innerTextureGuid = AssetDatabase.AssetPathToGUID(innerTexturePath);
    }

    public TextureImporter GetTextureImporter()
    {
        return innerTextureImporter;
    }

    public void ConfirmImportSetting()
    {
        innerTextureImporter.SaveAndReimport();
        metaContent = File.ReadAllText(metaFile);
    }

    public void GenerateMetaFile(string path)
    {
        var newGuid = AssetDatabase.AssetPathToGUID(path);
        var meta = metaContent.Replace(innerTextureGuid, newGuid);
        var newMetaFile = $"{path}.meta";
        File.WriteAllText(newMetaFile, meta);
        AssetDatabase.ImportAsset(path);
    }
    public void Dispose()
    {
        Debug.Log("Dispose");
        innerTexture = null;
        innerTextureImporter = null;
        AssetDatabase.DeleteAsset(innerTexturePath);
        metaFile = null;
        metaContent = null;
        innerTextureGuid = null;
    }
}
