using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using OfficeOpenXml;
using System;
using System.Linq;
using UnityEditorInternal;

public class TransformExporter : EditorWindow
{
    //是否导出未激活的对象
    private bool includeUnenbaledObj = true;
    //节点搜索方式
    private SearchType searchType;
    //保存到处对象参数
    private Dictionary<string, bool> stateDic = new Dictionary<string, bool>();
    //Tag、Layer单选按钮名称
    private string[] m_toolbarStr = { "Tag", "Layer" };
    //Tag、Layer单选按钮标记
    private int radioBtnValue, currentValue = -1;
    //导出的1xls文件存储路径
    private string path = null;
    [MenuItem("Tools/TransformExporter(xls)")]
    public static void ShowExportWindows()
    {
        EditorWindow.GetWindow<TransformExporter>("Transform Exporter");
    }

    //对话框中的各种内容通过OnGUI函数来设置
    void OnGUI()
    {
        //绘制标题
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Transform Exporter");
        GUILayout.Space(10);

        includeUnenbaledObj = GUILayout.Toggle(includeUnenbaledObj, "是否包含未激活的对象?");
        GUILayout.Space(10);

        searchType = (SearchType)EditorGUILayout.EnumPopup("节点搜索方式", searchType);

        GUILayout.Label("请选择保存路径", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField(path, GUILayout.ExpandWidth(false));
        if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
            path = EditorUtility.SaveFolderPanel("Path to Save data", path, Application.dataPath);   //打开保存文件夹面板
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("请选择按Tag输出还是按Layer输出？", EditorStyles.boldLabel);
        radioBtnValue = GUILayout.Toolbar(radioBtnValue, m_toolbarStr, EditorStyles.radioButton);
        GUILayout.Space(10);
        switch (radioBtnValue)
        {
            case 0:
                if (currentValue != 0)//切换Tag,Layer标签时重置标记并清空当前状态
                {
                    stateDic.Clear();
                    currentValue = 0;
                }
                //Tag选择框  
                foreach (var tag in InternalEditorUtility.tags)
                {
                    if (!stateDic.ContainsKey(tag))
                        stateDic.Add(tag, false);
                    stateDic[tag] = EditorGUILayout.Toggle(tag, stateDic[tag]);
                }
                break;
            case 1:
                if (currentValue != 1)
                {
                    stateDic.Clear();
                    currentValue = 1;
                }
                //Layer选择框                                                                          
                foreach (var layer in InternalEditorUtility.layers)
                {
                    if (!stateDic.ContainsKey(layer))
                        stateDic.Add(layer, false);
                    stateDic[layer] = EditorGUILayout.Toggle(layer, stateDic[layer]);
                }
                break;
        }
        GUILayout.Space(10);
        if (GUILayout.Button("导出数据"))
        {
            //ExportData2(currentValue, stateDic);
            ExportData(currentValue, stateDic);
        }
    }

    public enum SearchType
    {
        BFS,
        DFS
    }

    private void ExportData(int index, Dictionary<string, bool> stateDic)
    {
        var name = "Scene_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_" + DateTime.Now.Ticks + ".xlsx";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory("ExcelDir");
            path = "ExcelDir";
        }
        var fullpath = Path.Combine(path, name);
        //获取当前激活场景的根节点对象
        var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        var validStates = stateDic.Keys.Where(key => stateDic[key]).ToArray();
        //如果不包含未激活的对象，则先把根节点中未激活的对象去除掉
        if (!includeUnenbaledObj)
            rootObjects = rootObjects.Where(obj => obj.activeSelf == true).ToArray();
        //根据所选tag或layer来定义回调，index=0为Tag，index=1为Layer
        Func<Transform, bool> action = null;
        if (index == 0)//按Tag输出
        {
            action = trans =>
            {
                //如果有效状态里包含了该transform的tag标签，则通过筛选
                return validStates.Contains(trans.tag);
            };
        }
        else if (index == 1)//按Layer输出
        {
            action = trans =>
            {
                //如果有效状态里包含了transform.gameObject的layer名称，则通过筛选
                return validStates.Contains(LayerMask.LayerToName(trans.gameObject.layer));
            };
        }


        //操作excel表格
        using (var stream = new FileStream(fullpath, FileMode.CreateNew))
        {
            //操作excel表格
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                //设置表名
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Transform信息");
                //设置表头
                worksheet.Cells[1, 1].Value = "name";
                worksheet.Cells[1, 2].Value = "position";
                worksheet.Cells[1, 3].Value = "rotation";
                worksheet.Cells[1, 4].Value = "eulerAngles";
                worksheet.Cells[1, 5].Value = "localScale";
                worksheet.Cells[1, 6].Value = "activeself";
                worksheet.Cells[1, 7].Value = "path";
                worksheet.Cells[1, 8].Value = "tag";
                worksheet.Cells[1, 9].Value = "layer";

                int i = 1;
                var list = new List<Transform>();
                //遍历子节点时的操作
                TransformExtension.TransformAction searchAction = item =>
                {
                    var need = !includeUnenbaledObj || includeUnenbaledObj && item.gameObject.activeSelf;
                    if (need)
                    {
                        if (action(item))
                        {
                            list.Add(item);
                        }
                    }
                    return need;
                };
                //循环写入每个根节点下通过筛选的数据
                foreach (var obj in rootObjects)
                {
                    list.Clear();
                    list.Add(obj.transform);//先放入根节点
                    switch (searchType)
                    {
                        case SearchType.BFS:
                            obj.transform.BFSGetChildren(searchAction);
                            break;
                        case SearchType.DFS:
                            obj.transform.DFSGetChildren(searchAction);
                            break;
                    }
                    //写入筛选出的数据
                    foreach (var item in list)
                    {
                        i++;
                        worksheet.Cells[i, 1].Value = item.name;
                        worksheet.Cells[i, 2].Value = item.position;
                        worksheet.Cells[i, 3].Value = item.rotation;
                        worksheet.Cells[i, 4].Value = item.eulerAngles;
                        worksheet.Cells[i, 5].Value = item.localScale;
                        worksheet.Cells[i, 6].Value = item.gameObject.activeSelf;
                        worksheet.Cells[i, 7].Value = item.GetRoute();
                        worksheet.Cells[i, 8].Value = item.tag;
                        worksheet.Cells[i, 9].Value = LayerMask.LayerToName(item.gameObject.layer);
                    }
                }
                //保存excel表格
                package.Save();
            }
        }
        //提示导出成功
        this.ShowNotification(new GUIContent("导出成功！路径为：" + path));
    }

    [MenuItem("Tools/SearchTransform2")]
    static void Search2()
    {
        var transform = Selection.activeTransform;
        var transforms = new List<Transform>();
        var transforms2 = new List<Transform>();

        //GetChild(transform, SearchOption.BFS, true, ref transforms);
        Debug.Log("_______DFSGetChildren_______");
        transform.DFSGetChildren(tran =>
        {
            var active = tran.gameObject.activeSelf;
            if (active)
            {
                transforms.Add(tran);
            }
            return active;
        });
        foreach (var item in transforms)
        {
            Debug.Log(item.name);
        }
        Debug.Log("_______BFSGetChildren_______");
        transform.DFSGetChildren(tran =>
        {
            var active = tran.gameObject.activeSelf;
            if (active)
            {
                transforms2.Add(tran);
            }
            return active;
        });
        foreach (var item in transforms2)
        {
            Debug.Log(item.name);
        }
    }

}

public enum SearchOption
{
    DFS,//深度优先遍历
    BFS//广度优先遍历
}