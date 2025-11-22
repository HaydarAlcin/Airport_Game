#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class InspectorLockToggle
{
    [MenuItem("Tools/Inspector/Toggle Lock _`")]
    private static void ToggleInspectorLock()
    {
        var inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
        if (inspectorType == null) return;

        EditorWindow inspector = null;

        if (EditorWindow.focusedWindow != null &&
            EditorWindow.focusedWindow.GetType() == inspectorType)
        {
            inspector = EditorWindow.focusedWindow;
        }
        else
        {
            var allInspectors = Resources.FindObjectsOfTypeAll(inspectorType);
            if (allInspectors != null && allInspectors.Length > 0)
                inspector = allInspectors[0] as EditorWindow;
        }

        if (inspector == null) return;

        var isLockedProp = inspectorType.GetProperty(
            "isLocked",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (isLockedProp == null) return;

        bool current = (bool)isLockedProp.GetValue(inspector, null);
        isLockedProp.SetValue(inspector, !current, null);
        inspector.Repaint();
    }
}
#endif