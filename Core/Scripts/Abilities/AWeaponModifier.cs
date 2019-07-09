using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class AWeaponModifier : ScriptableObject, ICommand
    {
        public WeaponModifierType type;

        public abstract void Execute();

        public abstract void Undo();
    }
}