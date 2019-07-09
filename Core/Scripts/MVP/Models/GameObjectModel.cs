//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    public class GameObjectModel : Model
    {
        #region Fields
        protected Vector3 position;
        protected Vector3 localPosition;
        protected Quaternion rotation;
        #endregion Fields

        #region Properties
        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                OnPositionChanged?.Invoke();
            }
        }

        public Vector3 LocalPosition
        {
            get => localPosition;
            set
            {
                localPosition = value;
                OnLocalPositionChanged?.Invoke();
            }
        }

        public Quaternion Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                OnRotationChanged?.Invoke();
            }
        }
        #endregion Properties
            
        public event OnModelChangedDelegate OnPositionChanged;
        public event OnModelChangedDelegate OnLocalPositionChanged;
        public event OnModelChangedDelegate OnRotationChanged;
    }
}