//Developed by Pavel Kravtsov.
using UnityEngine;
namespace Core
{
    [System.Serializable]
    public class MovementModel : GameObjectModel
    {
        #region Fields
        [SerializeField]
        private float speed;
        [SerializeField]
        private Vector3 direction = new Vector3();
        private bool isAbleToMove = true;
        #endregion Fields

        #region Properties
        public bool IsAbleToMove
        {
            get => isAbleToMove;
            set
            {
                isAbleToMove = value;
                OnMoveAbilityChanged?.Invoke();
            }
        }

        public float Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                OnSpeedChanged?.Invoke();
            }
        }

        public Vector3 Direction
        {
            get => direction;
            set
            {
                direction = value;
                OnDirectionChanged?.Invoke();
            }
        }
        #endregion Properties

        public event OnModelChangedDelegate OnSpeedChanged;
        public event OnModelChangedDelegate OnDirectionChanged;
        public event OnModelChangedDelegate OnMoveAbilityChanged;

        public MovementModel() { }

        public MovementModel(Vector2 position, float speed)
        {
            this.position = position;
            this.speed = speed;
        }
    }
}