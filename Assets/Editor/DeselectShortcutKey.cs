using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class DeselectShortcutKey
{
    [MenuItem("GameObject/Deselect all %#D")]
    static void DeselectAll()
    {
        Selection.objects = new Object[0];
    }

    static DeselectShortcutKey()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    static bool AutoDeselectOnPlay = false;
    private static void LogPlayModeState(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode && AutoDeselectOnPlay) DeselectAll();
    }
}
