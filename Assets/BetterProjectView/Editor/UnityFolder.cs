using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ProjectViewer
{
    public class UnityFolder
    {
        private string _path;
        private List<UnityFile> _files;
        private List<UnityFolder> _subFolders;
        private long _totalSize;
        private readonly int _depth;

        public string Path
        {
            get { return _path; }
        }

        public long Size
        {
            get { return _totalSize; }
        }

        public int FileCount
        {
            get { return _files.Count; }
        }

        public int SubFolderCount
        {
            get { return _subFolders.Count; }
        }


        public UnityFolder(string folderPath, UnityFolder parentFolder, int depth)
        {
            _path = folderPath.Replace('\\', '/');
            _depth = depth;
            _files = FindChildFiles();
            _subFolders = FindSubFolders();
        }

        private List<UnityFolder> FindSubFolders()
        {
            string[] dirs = new string[0];
            //GetDirectories will return all the subfolders in the given path.
            try
            {
               dirs = Directory.GetDirectories(_path);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }   

            List<UnityFolder> folders = new List<UnityFolder>();
            foreach (var directory in dirs)
            {
                UnityFolder newfolder = new UnityFolder(directory, this, _depth + 1);
                folders.Add(newfolder);
                _totalSize += newfolder._totalSize;
            }
            return folders;
        }

        private List<UnityFile> FindChildFiles()
        {
            string[] fileNames = new string[0];
            //GetDirectories will return all the subfolders in the given path.
            try
            {
                fileNames = Directory.GetFiles(_path);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            //GetFiles is similar but returns all the files under the path(obviously)
            List<UnityFile> files = new List<UnityFile>();
            foreach (var file in fileNames)
            {
                UnityFile newfile = new UnityFile(file, this);
                //Pass meta files.
                if (newfile.GetExtension().Equals("meta"))
                {
                    continue;
                }

                files.Add(newfile);
                _totalSize += new FileInfo(file).Length;
            }

            return files;

        }


        public UnityFolder FindFolder(string path)
        {
            if (path.Equals(this._path.Trim()))
            {
                return this;
            }

            for (int i = 0; i < _subFolders.Count; i++)
            {
                UnityFolder temp = _subFolders[i].FindFolder(path);
                if (temp != null)
                {
                    return temp;
                }
            }

            return null;
        }
    }

}
