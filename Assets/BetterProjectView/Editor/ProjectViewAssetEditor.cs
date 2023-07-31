using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProjectViewer.ProjectViewSettings))]
public class ProjectViewAssetEditor : Editor {

    public override void OnInspectorGUI()
    {
        var projectViewSettings = (ProjectViewer.ProjectViewSettings) target;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Folder Icon:", GUILayout.Width(120));
        projectViewSettings._icon =
            (Texture2D) EditorGUILayout.ObjectField(projectViewSettings._icon, typeof(Texture2D), false);
        GUILayout.EndHorizontal();


        if (projectViewSettings._pairs == null)
            projectViewSettings._pairs = new List<ProjectViewer.Pair>();

        GUILayout.BeginVertical();
        for (var i = 0; i < projectViewSettings._pairs.Count; i++)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.BeginHorizontal();
            GUILayout.Space(8);
            GUILayout.Label("Folder Name:", GUILayout.Width(120));
            projectViewSettings._pairs[i]._folderName = GUILayout.TextField(projectViewSettings._pairs[i]._folderName);
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Space(8);
            GUILayout.Label("Folder Color:", GUILayout.Width(120));
            projectViewSettings._pairs[i]._color = EditorGUILayout.ColorField(projectViewSettings._pairs[i]._color);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Delete", EditorStyles.toolbarButton))
                projectViewSettings._pairs.RemoveAt(i);
            GUILayout.EndVertical();

        }
        GUILayout.Space(8);

        if (GUILayout.Button("Add Pair", EditorStyles.toolbarButton))
            projectViewSettings._pairs.Add(new ProjectViewer.Pair());
        GUILayout.EndVertical();


    }
}
