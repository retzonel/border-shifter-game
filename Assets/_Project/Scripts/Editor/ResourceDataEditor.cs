using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResourceData))]
public class ResourceDataEditor : Editor
{
    private ResourceData _resourceData;

    private void OnEnable()
    {
        _resourceData = target as ResourceData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (_resourceData.sprite == null)
        {
            return;
        }

        var texture = AssetPreview.GetAssetPreview(_resourceData.sprite);
        GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(80));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}
