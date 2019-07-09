//Developed by Pavel Kravtsov.
namespace Core
{
    public abstract class AUserDataPresenter : Presenter
    {
        public event Model.OnModelChangedDelegate OnDataChanged
        {
            add { Model.OnUserDataChanged += value; }
            remove { Model.OnUserDataChanged -= value; }
        }
        protected abstract AUserDataModel Model { get; set; }

        public string StringDateTime { get => Model.StringDateTime; set => Model.StringDateTime = value; }

        public string JsonUD { get => UnityEngine.JsonUtility.ToJson(Model); }

        public abstract void SetNewModelData(AUserDataModel model);
    }
}