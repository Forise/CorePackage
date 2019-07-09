//Developed by Pavel Kravtsov.
namespace Core
{
    [System.Serializable]
    public abstract class Model
    {
        public delegate void OnModelChangedDelegate();
    }
}