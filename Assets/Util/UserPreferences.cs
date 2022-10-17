using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    [CreateAssetMenu(fileName = "UserPrefereces", menuName = "Scrtiprable Object/PreferencesAsset")]
    public class UserPreferences : ScriptableObject
    {
        public string _userId = "";

        public string _userName = "";

        public Handedness _preferredHandedness = Handedness.Right;

        public string UserId { get => _userId; set => _userId = value; }

        public string UserName { get => _userName; set => _userName = value; } 

        public Handedness PreferredHandedness { get => _preferredHandedness; set => _preferredHandedness = value; }

    }
}
