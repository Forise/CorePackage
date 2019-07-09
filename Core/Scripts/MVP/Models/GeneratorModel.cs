//Developed by Pavel Kravtsov.
namespace Core
{
    [System.Serializable]
    public class GeneratorModel<T> : Model
    {
        protected System.Collections.Generic.List<T> objects = new System.Collections.Generic.List<T>();
        
        public event OnModelChangedDelegate OnObjectsChanged;

        public System.Collections.Generic.List<T> GeneratedObjects
        {
            get { return objects; }
            set
            {
                objects = value;
                OnObjectsChanged();
            }
        }
    }
}