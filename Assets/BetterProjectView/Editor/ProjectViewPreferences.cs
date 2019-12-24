using UnityEngine;
using UnityEditor;
#pragma warning disable 0618
namespace ProjectViewer
{
    [InitializeOnLoad]
    public class ProjectViewPreferences
    {
        private static bool prefsLoaded = false;
        public static ProjectViewSettings viewSettings;
        public static bool showSizeInfo = false;

        public static string objectGuid;


        static ProjectViewPreferences()
        {
            ProjectLoad();
        }

        [PreferenceItem("Project View+")]
        private static void CustomPreferencesGUI()
        {
            ProjectLoad();


            GUILayout.Label("Version 1.00 ");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Settings: ",  GUILayout.Width(120));
            viewSettings =
                (ProjectViewSettings)EditorGUILayout.ObjectField(viewSettings, typeof(ProjectViewSettings), false);
            if (viewSettings)
            {
                var path = AssetDatabase.GetAssetPath(viewSettings);
                objectGuid = AssetDatabase.AssetPathToGUID(path);
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Show Size Info: ", EditorStyles.miniLabel, GUILayout.Width(120));
            showSizeInfo = GUILayout.Toggle(EditorPrefs.GetBool("showSizeInfo"), "");
            GUILayout.EndHorizontal();

            if (!GUI.changed) return;
            EditorPrefs.SetString("objectGuid", objectGuid);
            EditorPrefs.SetBool("showSizeInfo", showSizeInfo);
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
            if (!prefsLoaded)
            {
                objectGuid = EditorPrefs.GetString("objectGuid");
                showSizeInfo = EditorPrefs.GetBool("showSizeInfo");
                viewSettings = LoadByGUID();
                prefsLoaded = true;
            } 
            
        }


    }
}
