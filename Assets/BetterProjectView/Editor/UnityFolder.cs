using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ProjectViewer
{

    public class UnityFolder
    {

        private string folderPath;
        private UnityFolder parentFolder;

        private List<UnityFile> child_files;
        private List<UnityFolder> child_folders;
        private long size;

        public string Path
        {
            get { return folderPath; }
        }
        public long Size
        {
            get { return size; }
        }

        public int FileCount
        {
            get { return child_files.Count; }
        }

        public int SubFolderCount
        {
            get { return child_folders.Count; }
        }
        private readonly int depth;


        public UnityFolder(string folderPath, UnityFolder parentFolder, int depth)
        {
            this.folderPath = folderPath.Replace('\\','/');
            this.depth = depth;
            child_files = FindChildFiles();
            child_folders = FindChildFolders();

            //Create the object by giving its path. Then get the assetpreview.


            //Assets/New Folder-> folderName:New Folder



        }

      

        private List<UnityFolder> FindChildFolders()
        {
            //GetDirectories will return all the subfolders in the given path.
            string[] dirs = Directory.GetDirectories(folderPath);
            List<UnityFolder> folders = new List<UnityFolder>();
            foreach (var directory in dirs)
            {
                //Turn all directories into our 'UnityFolder' Object.
                UnityFolder newfolder = new UnityFolder(directory, this, depth + 1);
                folders.Add(newfolder);
                size += newfolder.size;
            }
            return folders;
        }

        private List<UnityFile> FindChildFiles()
        {
            //GetFiles is similar but returns all the files under the path(obviously)
            string[] fileNames = Directory.GetFiles(folderPath);
            List<UnityFile> files = new List<UnityFile>();
            foreach (var file in fileNames)
            {
                UnityFile newfile = new UnityFile(file, this);
                //Pass meta files.
                if (newfile.GetExtension().Equals("meta"))
                    continue;
                files.Add(newfile);
                size += new FileInfo(file).Length;
            }

            return files;

        }


        public UnityFolder FindFolder(string path)
        {
           

            if (path.Equals(this.folderPath.Trim()))
                return this;
            UnityFolder temp=null;
            for (int i = 0; i < child_folders.Count;i++ )
            {
                temp= child_folders[i].FindFolder(path);
                if (temp!=null)
                    return temp;
            }

            return temp;
        }
       





    }

}
