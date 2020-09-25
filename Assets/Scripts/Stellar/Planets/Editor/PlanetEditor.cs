using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet planet;
    private Editor shapeEditor;
    private Editor colorEditor;

    void OnEnable()
    {
        planet = (Planet)target;

        planet.shapeSettingsFoldout = true;
        planet.colorSettingsFoldout = true;
    }

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref planet.colorSettingsFoldout, ref colorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingUpdated, ref bool foldOut, ref Editor settingsEditor)
    {
        if (settings != null)
        {
            foldOut = EditorGUILayout.InspectorTitlebar(foldOut, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldOut)
                {
                    CreateCachedEditor(settings, null, ref settingsEditor);
                    settingsEditor.OnInspectorGUI();

                    if (check.changed)
                    {
                        onSettingUpdated.Invoke();
                    }
                }
            }
        }
    }
}
