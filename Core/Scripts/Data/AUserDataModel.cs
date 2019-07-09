//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    public abstract class AUserDataModel : Model
    {
        [SerializeField]
        private string stringDateTime;

        public string StringDateTime
        {
            get => stringDateTime;
            set
            {
                stringDateTime = value;
                DateTimeChanged();
            }
        }

        public event OnModelChangedDelegate OnDateTimeChanged;
        public event OnModelChangedDelegate OnUserDataChanged;

        protected void DateTimeChanged()
        {
            OnDateTimeChanged?.Invoke();
        }

        protected void UserDataChanged()
        {
            OnUserDataChanged?.Invoke();
        }
    }
}