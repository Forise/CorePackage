//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class Dictionary_String_String : SerializableDictionary<string, string> { }

    [System.Serializable]
    public class Dictionary_String_Int : SerializableDictionary<string, int> { }

    [System.Serializable]
    public class Dictionary_Int_Int : SerializableDictionary<int, int> { }

    [System.Serializable]
    public class Dictionary_Int_String: SerializableDictionary<int, string> { }

    [System.Serializable]
    public class Dictionary_String_GameObject : SerializableDictionary<string, GameObject> { }

    [System.Serializable]
    public class Dictionary_Object_Chance : SerializableDictionary<GameObject, float> { }
    [System.Serializable]
    public class Dictionary_String_Sprite : SerializableDictionary<string, Sprite> { }
}