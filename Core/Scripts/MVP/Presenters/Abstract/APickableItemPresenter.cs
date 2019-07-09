//Developed by Pavel Kravtsov.
using UnityEngine;
namespace Core
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class APickableItemPresenter : Presenter
    {
        [SerializeField]
        protected string tagToDetect;
        [SerializeField]
        protected PickableItemModel model = new PickableItemModel();
        [SerializeField]
        protected Collider2D coll;

        public PickableItemType GetItemType()
        {
            return model.Type;
        }

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        protected virtual void Init()
        {
            if(coll)
                coll.enabled = false;
            if (model == null)
                model = new PickableItemModel();
            model.OnTypeChanged += UpdateType;
        }

        public abstract GameObject PickUp();
        protected abstract void UpdateType(); 
    }
}