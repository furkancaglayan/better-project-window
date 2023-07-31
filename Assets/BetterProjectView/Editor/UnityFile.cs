using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProjectViewer
{
    public class UnityFile
    {
        private FileInfo _info;

        public UnityFile(string path, UnityFolder parentFolder)
        {
            _info = new FileInfo(path);
        }

        public string GetExtension()
        {
            return _info?.Extension ?? string.Empty;
        }
    }
}