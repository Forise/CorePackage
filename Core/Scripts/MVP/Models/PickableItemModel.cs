//Developed by Pavel Kravtsov.
using UnityEngine;
namespace Core
{
    [System.Serializable]
    public class PickableItemModel : GameObjectModel
    {
        [SerializeField]
        private PickableItemType type;

        public PickableItemType Type
        {
            get => type;
            set
            {
                type = value;
                OnTypeChanged?.Invoke();
            }
        }

        public event OnModelChangedDelegate OnTypeChanged;
    }
}