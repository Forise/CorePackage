//Developed by Pavel Kravtsov.
using Core.EventSystem;
using UnityEngine.UI;

namespace Core
{
    public class MyButton : Button
    {
        protected override void Start()
        {        
            base.Start();
            onClick.AddListener(MakeClickSound);
        }

        protected override void OnDestroy()
        {
            onClick.RemoveListener(MakeClickSound);
            base.OnDestroy();
        }

        private void MakeClickSound()
        {
            EventManager.Notify(this, new GameEventArgs(EventManager_Input.UI_TAPPED));
        }
    }
}