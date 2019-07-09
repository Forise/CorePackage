//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    public abstract class AAbility : ScriptableObject
    {
        public string id;
        public abstract ushort[] Prices { get; }
        public abstract AbilityRestoreType RestoreType { get; }
        public abstract bool IsAbleToUse { get; }
        public abstract int Level { get; set; }
        public abstract void SetByLevel();
        public abstract void Use();
    }
}