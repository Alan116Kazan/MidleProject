using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SettingsSelector))]
public class SettingsSelectorEditor : Editor
{
    private string[] settingsNames;
    private Settings[] allSettings;
    private int selectedIndex;

    public override void OnInspectorGUI()
    {
        var selector = (SettingsSelector)target;

        string[] guids = AssetDatabase.FindAssets("t:Settings");
        List<Settings> settingsList = new List<Settings>();
        List<string> names = new List<string>();

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            Settings asset = AssetDatabase.LoadAssetAtPath<Settings>(path);
            if (asset != null)
            {
                settingsList.Add(asset);
                names.Add(asset.name);
            }
        }

        allSettings = settingsList.ToArray();
        settingsNames = names.ToArray();

        selectedIndex = 0;
        if (selector.selectedSettings != null)
        {
            for (int i = 0; i < allSettings.Length; i++)
            {
                if (selector.selectedSettings == allSettings[i])
                {
                    selectedIndex = i;
                    break;
                }
            }
        }

        int newIndex = EditorGUILayout.Popup("Выбрать конфиг", selectedIndex, settingsNames);

        if (newIndex != selectedIndex)
        {
            Undo.RecordObject(selector, "Change Selected Settings");
            selector.selectedSettings = allSettings[newIndex];
            EditorUtility.SetDirty(selector);
        }

        if (selector.selectedSettings != null)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Параметры:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Hero Health:", selector.selectedSettings.HeroHealth.ToString());
        }
    }
}
