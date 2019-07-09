using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface IBuffable
    {
        List<ABuff> Buffs { get; }
        void Buff(ABuff buff);
        void Buff(ABuff[] buffs);
        void Dispell(ABuff buff);
        void DispellByType(System.Type type);
        void DispellAll();
        void OnBuffEnd(ABuff s);
    }
}