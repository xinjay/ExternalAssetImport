using System;
using UnityEditor;
public static class AssetImportUtils
{
    private static bool isStartAssetEditing = false;
    public static void StartAssetEditing()
    {
        if (!isStartAssetEditing)
        {
            AssetDatabase.StartAssetEditing();
            isStartAssetEditing = true;
        }
    }
    public static void StopAssetEditing()
    {
        if (isStartAssetEditing)
        {
            AssetDatabase.StopAssetEditing();
            isStartAssetEditing = false;
        }
    }
    public static void ForceRefresh(ImportAssetOptions options)
    {
        if (isStartAssetEditing)
            AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh(options);
        if (isStartAssetEditing)
            AssetDatabase.StartAssetEditing();
    }
    public static string GetRandomNameByTimeStamp()
    {
        var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var ramdomName = (long)timeSpan.TotalMilliseconds;
        return ramdomName.ToString();
    }
}

