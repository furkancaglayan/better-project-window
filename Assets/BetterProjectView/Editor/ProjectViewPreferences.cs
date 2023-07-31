using UnityEngine;
using UnityEditor;
#pragma warning disable 0618
namespace ProjectViewer
{
    [InitializeOnLoad]
    public class ProjectViewPreferences
    {
        private static bool _loaded = false;
        public static ProjectViewSettings ViewSettings;
        public static bool ShowSizeInfo = false;

        public static string objectGuid;


        static ProjectViewPreferences()
        {
            ProjectLoad();
        }

        [PreferenceItem("Project View+")]
        private static void CustomPreferencesGUI()
        {
            ProjectLoad();

            GUILayout.Label("Version 1.01");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Settings: ", GUILayout.Width(120));
            ViewSettings =
                (ProjectViewSettings)EditorGUILayout.ObjectField(ViewSettings, typeof(ProjectViewSettings), false);
            if (ViewSettings)
            {
                var path = AssetDatabase.GetAssetPath(ViewSettings);
                objectGuid = AssetDatabase.AssetPathToGUID(path);
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Show Size Info: ", EditorStyles.miniLabel, GUILayout.Width(120));
            ShowSizeInfo = GUILayout.Toggle(EditorPrefs.GetBool("showSizeInfo"), "");
            GUILayout.EndHorizontal();

            if (!GUI.changed) return;
            EditorPrefs.SetString("objectGuid", objectGuid);
            EditorPrefs.SetBool("showSizeInfo", ShowSizeInfo);
        }




        private static ProjectViewSettings LoadByGUID()
        {
            string[] guids = AssetDatabase.FindAssets("t:ProjectViewSettings", null);
            foreach (string guid in guids)
            {
                if (guid.Equals(objectGuid))
                    return (ProjectViewSettings)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(ProjectViewSettings));
            }
            return null;
        }

        private static void ProjectLoad()
        {
            if (!_loaded)
            {
                objectGuid = EditorPrefs.GetString("objectGuid");
                ShowSizeInfo = EditorPrefs.GetBool("showSizeInfo");
                ViewSettings = LoadByGUID();
                _loaded = true;
            }
        }
    }
}
