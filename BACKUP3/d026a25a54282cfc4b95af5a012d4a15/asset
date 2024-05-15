using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class QuickTool : EditorWindow
{
    [MenuItem("Window/UI Toolkit/QuickTool")]
    public static void ShowExample()
    {
        QuickTool wnd = GetWindow<QuickTool>();
        wnd.titleContent = new GUIContent("QuickTool");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

    }
}
