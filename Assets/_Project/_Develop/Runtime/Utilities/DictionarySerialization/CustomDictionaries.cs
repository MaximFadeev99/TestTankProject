using System;
using TestTankProject.Runtime._Project._Develop.Runtime.Sounds;
using UnityEngine;

namespace TestTankProject.Runtime.Utilities.DictionarySerialization
{
    [Serializable]
    public class AudioClipDictionary : SerializedDictionary<SoundTypes, AudioClip>{}
}
