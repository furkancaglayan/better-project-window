using System.IO;
using UnityEditor;
using UnityEngine;


namespace ProjectViewer
{
    [InitializeOnLoad]
    public class ProjectViewEditor : Editor
    {
        private static UnityFolder Assets;

        private static void Update()
        {
            EditorApplication.projectWindowItemOnGUI += ProjectViewEditor.EditorCallback();
            Assets = new UnityFolder("Assets", null, 0);
        }

        public static void LateUpdate()
        {
            EditorApplication.projectWindowItemOnGUI -= ProjectViewEditor.EditorCallback();
            Update();

        }

        static ProjectViewEditor()
        {
            Update();
            EditorApplication.projectChanged += LateUpdate;
        }


        static void OnDestroy()
        {
            EditorApplication.projectChanged -= LateUpdate;
            EditorApplication.projectWindowItemOnGUI -= ProjectViewEditor.EditorCallback();
        }

        static EditorApplication.ProjectWindowItemCallback EditorCallback()
        {
            EditorApplication.ProjectWindowItemCallback callback = GUIOptions;
            return callback;
        }

        static void GUIOptions(string guid, Rect rect)
        {
            //return if settings is not assigned.
            if (!ProjectViewPreferences.ViewSettings)
                return;


            string fileName = AssetDatabase.GUIDToAssetPath(guid);

            //favorites tab, draw only box
            if (string.IsNullOrEmpty(fileName))
            {
                Rect r = new Rect(rect.x - 200, rect.y, rect.width + 200, rect.height);
                DrawBox(r);
                return;
            }


            FileInfo info = new FileInfo(fileName);
            if (!string.IsNullOrEmpty(info.Extension))
            {
                //files, not folders. Draw box around them.
                return;
            }


            string[] splt = fileName.Split('/');
            string folderName = splt[splt.Length - 1];

            if (ProjectViewPreferences.ViewSettings._pairs != null)
                foreach (var VARIABLE in ProjectViewPreferences.ViewSettings._pairs)
                {
                    if (string.IsNullOrEmpty(VARIABLE._folderName))
                        continue;
                    if (VARIABLE._folderName.Equals(folderName))
                    {
                        Drawer(rect, ProjectViewPreferences.ViewSettings._icon, VARIABLE._color, fileName);
                        ShowFolderInfo(rect, ProjectViewPreferences.ViewSettings._icon, VARIABLE._color, fileName);
                        return;

                    }
                }
            ShowFolderInfo(rect, null, Color.white, fileName);
            //Drawer(rect, ProjectViewSettings._iconSize, ProjectViewSettings._folderIcon, extension, fileName);

        }
        private static void DrawBox(Rect r)
        {
            if (!EditorGUIUtility.isProSkin)
                GUI.color = new Color(0.8f, 0.8f, 0.8f, .2f);
            GUI.BeginGroup(r, GUI.skin.box);
            GUI.color = Color.white;
            GUI.EndGroup();
        }
        private static void Drawer(Rect r, Texture2D texture, Color col, string fileName)
        {

            if (r.height > 16)
            {
                GUI.color = col;
                var textHeigth = GUI.skin.font.fontSize;
                GUI.DrawTexture(new Rect(new Vector2(r.x + 1, r.y), new Vector2(r.width, r.height - textHeigth)), texture);
                GUI.color = Color.white;
            }
            else
            {
                UnityFolder folder = Assets.FindFolder(fileName);
                if (folder == null)
                    return;
                GUI.color = col;
                GUI.DrawTexture(new Rect(new Vector2(r.x + 1, r.y), new Vector2(r.height, r.height)), texture);
                GUI.color = Color.white;
            }

        }

        private static void ShowFolderInfo(Rect r, Texture2D texture, Color col, string fileName)
        {
            if (r.height <= 16)
            {
                UnityFolder folder = Assets.FindFolder(fileName);
                if (folder == null)
                    return;

                Rect rect = new Rect(r.x - 200, r.y, r.width + 200, r.height);
                DrawBox(rect);
                GUI.BeginGroup(r);
                GUI.color = col;

                if (ProjectViewPreferences.ShowSizeInfo)
                {
                    if (!EditorGUIUtility.isProSkin)
                        if (GUI.color == Color.white)
                            GUI.color = Color.black;
                    if (folder.FileCount > 0 || folder.SubFolderCount > 0)
                    {

                        var sizeFormat = GetSizeFormat(folder.Size);
                        var sizeText = GUI.skin.label.CalcSize(new GUIContent(sizeFormat));
                        var controlRect = EditorGUILayout.GetControlRect();
                        GUI.Label(new Rect(new Vector2(r.width - 100, 0), new Vector2(200, 16)),
                            sizeFormat, EditorStyles.whiteMiniLabel);

                    }
                    else
                        GUI.Label(new Rect(new Vector2(r.width - 100, 0), new Vector2(200, 16)),
                       "Empty!", EditorStyles.whiteMiniLabel);
                }
                GUI.color = Color.white;

                GUI.EndGroup();
            }

        }
        private void OnGUI()
        {
            Repaint();
        }

        private static void DrawDottedLines(Vector3 p1, Vector3 p2)
        {
            Handles.color = Color.white;
            Handles.DrawLine(p1, p2);
        }
        private static string GetSizeFormat(long size)
        {
            double kb_size = 1024;
            if (size < kb_size)
                return (size).ToString("F2") + " B";

            double mb_size = 1024 * kb_size;

            if (size < mb_size)
                return (size / kb_size).ToString("F2") + " KB";


            double gb_size = 1024 * mb_size;
            double mb = size / mb_size;
            if (mb < 1024)
                return (size / mb_size).ToString("F2") + " MB";
            return (size / gb_size).ToString("F2") + " GB";

        }
    }
}
