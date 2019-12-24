using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectViewer
{

    public class UnityFile
    {


        private readonly string extension;




        public UnityFile(string path, UnityFolder parentFolder)
        {
            extension = FindExtension(path);





        }

        private string FindExtension(string path)
        {
            string[] splitPath = path.Split('.');
            string ext = splitPath[splitPath.Length - 1];
            return ext;
        }
        public string GetExtension()
        {
            return extension;

        }


    }

}