//Developed by Pavel Kravtsov.
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "TestDevices", menuName = "Core/ScriptableObjects/TestDevices")]
    public class TestDevices : ScriptableObject
    {
        [SerializeField]
        private List<string> testDevices = new List<string>();

        public List<string> Devices
        {
            get { return testDevices; }
        }
    }
}