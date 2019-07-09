using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class ABuff : ScriptableObject, ICommand
    {
        public GameObject target;
        public BuffType type;
        public BuffTimeType timeType;
        public float duration;
        public bool stuckable = false;
        public abstract event System.Action<ABuff> End;

        public abstract void Execute();
        public abstract void Undo();
    }
}