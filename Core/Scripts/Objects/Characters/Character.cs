//Developed by Pavel Kravtsov.
using UnityEngine;
using System.Collections.Generic;

namespace Core
{
    public class Character : MonoBehaviour, IBuffable
    {
        #region Fields
        [SerializeField]
        protected string id;
        public string ID { get => id; }
        [SerializeField]
        private List<ABuff> buffs = new List<ABuff>();
        #endregion Fields

        #region Properties
        public List<ABuff> Buffs { get => buffs; protected set => buffs = value; }
        #endregion Properties

        protected virtual void Awake()
        {
            if (Buffs.Count > 0)
            {
                foreach (var b in Buffs)
                {
                    b.End += OnBuffEnd;
                    b.target = gameObject;
                    b.Execute();
                }
            }
        }

        public void Buff(ABuff buff)
        {
            if (!Buffs.Contains(buff) || buff.stuckable)
            {
                buff.target = gameObject;
                buff.End += OnBuffEnd;
                buff.Execute();
                buffs.Add(buff);
            }
        }
        public void Buff(ABuff[] buffs)
        {
            foreach(var b in buffs)
            {
                if (!Buffs.Contains(b) || b.stuckable)
                {
                    b.target = gameObject;
                    b.End += OnBuffEnd;
                    b.Execute();
                    this.buffs.Add(b);
                }
            }
        }

        public void Dispell(ABuff buff)
        {
            if (buffs.Contains(buff))
            {
                buff.Undo();
                buffs.Remove(buff);
            }
        }

        public void DispellByType(System.Type type)
        {
            buffs.RemoveAll((x) => 
            {
                if (x.GetType() == type)
                {
                    x.Undo();
                    return true;
                }
                else return false;
            });
        }

        public void DispellAll()
        {
            foreach(var b in Buffs)
                b.Undo();
            Buffs = new List<ABuff>();
        }

        #region Handlers
        public void OnBuffEnd(ABuff obj)
        {
            Dispell(obj);
        }
        #endregion Handlers
    }
}