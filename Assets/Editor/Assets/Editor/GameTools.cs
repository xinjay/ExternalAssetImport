using UnityEngine;
using System.Collections;
using UnityEditor;
public class GameTools : EditorWindow
{

    [MenuItem("Tools/MyTest")]
    static void AddWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<GameTools>(true, "mytest", true);
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.HelpBox("A.我是一个测试\nB:我的功能很简单", MessageType.Warning);
        m_popUpType = GUILayout.Toolbar(m_popUpType, m_toolbarStr,EditorStyles.radioButton);
        switch (m_popUpType)
        {
            case 0:
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("加数", GUILayout.Width(50));
                    addNum1 = EditorGUILayout.IntField(addNum1, GUILayout.Width(50));
                    GUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("被加数", GUILayout.Width(50));
                    addNum2 = EditorGUILayout.IntField(addNum2, GUILayout.Width(50));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    if (GUILayout.Button("等于", GUILayout.Width(100), GUILayout.Height(25)))
                    {
                        addResult = addNum1 + addNum2;
                        EditorUtility.DisplayDialog("提示", "恭喜您执行了加法操作", "OK");
                    }
                    GUILayout.Label(addResult.ToString(), GUILayout.Width(50));

                    m_toggle = EditorGUILayout.Toggle("我是ToggleBox", m_toggle);
                    m_enum = (MyEnumType)EditorGUILayout.EnumPopup("我是枚举框", m_enum);
                    m_myObj = EditorGUILayout.ObjectField("我是ObjField", m_myObj, typeof(Object), false);


                    EditorGUILayout.EndVertical();
                    break;
                }
            case 1:
                {
                    GUILayout.Label("祝大家学习进步", GUILayout.Width(500));
                    break;
                }
        }
    }

    private string[] m_toolbarStr = new string[] { "计算机", "关于" };
    private int m_popUpType;

    private int addNum1;
    private int addNum2;
    private int addResult;
    private Object m_myObj;
    private bool m_toggle;
    private MyEnumType m_enum = MyEnumType.en_2;
    private enum MyEnumType
    {
        en_1 = 1,
        en_2,
        en_3,
    }
}
