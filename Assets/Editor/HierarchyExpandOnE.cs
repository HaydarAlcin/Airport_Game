#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyExpandOnE
{
    static readonly System.Type s_HierarchyType;
    static readonly MethodInfo s_GetExpandedIDs;
    static readonly MethodInfo s_SetExpanded;
    static readonly MethodInfo s_SetExpandedRecursive;

    static HierarchyExpandOnE()
    {
        var editorAsm = typeof(EditorWindow).Assembly;
        s_HierarchyType = editorAsm.GetType("UnityEditor.SceneHierarchyWindow");
        s_GetExpandedIDs = s_HierarchyType?.GetMethod("GetExpandedIDs", BindingFlags.Instance | BindingFlags.NonPublic);
        s_SetExpanded = s_HierarchyType?.GetMethod("SetExpanded", BindingFlags.Instance | BindingFlags.NonPublic, null,
                                   new[] { typeof(int), typeof(bool) }, null);
        s_SetExpandedRecursive = s_HierarchyType?.GetMethod("SetExpandedRecursive", BindingFlags.Instance | BindingFlags.NonPublic, null,
                                   new[] { typeof(int), typeof(bool) }, null);

        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
    }

    static void OnHierarchyItemGUI(int instanceID, Rect selectionRect)
    {
        var e = Event.current;
        if (e.type != EventType.KeyDown || e.keyCode != KeyCode.E) return;

        // yalnýzca fare o item üzerindeyken çalýþ
        if (!selectionRect.Contains(e.mousePosition)) return;

        var win = EditorWindow.focusedWindow;
        if (win == null || win.GetType() != s_HierarchyType) return;

        bool expanded = IsExpanded(win, instanceID);
        bool newState = !expanded;

        bool recursive = e.shift;
        if (recursive && s_SetExpandedRecursive != null)
            s_SetExpandedRecursive.Invoke(win, new object[] { instanceID, newState });
        else if (s_SetExpanded != null)
            s_SetExpanded.Invoke(win, new object[] { instanceID, newState });
        else if (s_SetExpandedRecursive != null)
            s_SetExpandedRecursive.Invoke(win, new object[] { instanceID, newState });

        win.Repaint();
        e.Use();
    }

    static bool IsExpanded(EditorWindow win, int id)
    {
        if (s_GetExpandedIDs == null) return false;
        var ids = (int[])s_GetExpandedIDs.Invoke(win, null);
        if (ids == null) return false;
        return ids.Contains(id);
    }
}
#endif