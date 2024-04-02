using UnityEngine;
using UnityEditor;

public class ShowPersistentDataPathEditor : EditorWindow
{
    [MenuItem("Tools/Show Persistent Data Path")]
    private static void ShowPersistentDataPath()
    {
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
    }
}
