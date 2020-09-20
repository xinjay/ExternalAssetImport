using UnityEngine;
using UnityEditor;
public class Test
{
    [MenuItem("GameObject/SearchNode",priority = 0)]
    static void Search()
    {
        var trans = Selection.activeTransform;
        Debug.Log("_____BFS_____");
        trans.BFSGetChildren(child =>
        {
            var active = child.gameObject.activeSelf;
            if (active)
            {
                Debug.Log(child.name);
            }
            return active;
        });
        Debug.Log("_____DFS_____");
        trans.DFSGetChildren(child =>
        {
            var active = child.gameObject.activeSelf;
            if (active)
            {
                Debug.Log(child.name);
            }
            return active;
        });
    }
}
