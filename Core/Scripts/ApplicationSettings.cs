//Developed by Pavel Kravtsov.
using UnityEngine;
namespace Core
{
    public class ApplicationSettings : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}