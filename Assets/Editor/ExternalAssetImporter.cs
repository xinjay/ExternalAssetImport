using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


class ExternalAssetImporter
{
    public static void ImportTexture(string srcFolder, string targetFolder, Action<TextureImporter> importerSettingCallback)
    {
        if (!string.IsNullOrEmpty(srcFolder))
        {
            var files = Directory.GetFiles(srcFolder)
                .Where(file => file.EndsWith("png") || file.EndsWith("jpg") || file.EndsWith("exr"));
            using (var importSetting = new TextureImportSetting())
            {
                var importer = importSetting.GetTextureImporter();
                importerSettingCallback?.Invoke(importer);
                importSetting.ConfirmImportSetting();
                AssetImportUtils.StartAssetEditing();
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var targetPath = $"{targetFolder}/{fileName}";
                    File.Copy(file, targetPath, true);
                    importSetting.GenerateMetaFile(targetPath);
                }
                AssetImportUtils.StopAssetEditing();
                AssetDatabase.Refresh();
            }
        }
    }
}

