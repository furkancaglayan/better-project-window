using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProjectViewer
{

    [Serializable,CreateAssetMenu]
    public class ProjectViewSettings : ScriptableObject
    {
        [SerializeField] public List<Pair> _pairs;
        [SerializeField] public Texture2D _icon;

    }
    [Serializable]
    public class Pair
    {
        [SerializeField] public string _folderName;
        [SerializeField] public Color _color;
    }


}

