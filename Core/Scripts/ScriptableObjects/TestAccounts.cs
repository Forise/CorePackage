//Developed by Pavel Kravtsov.
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "TestAcoounts", menuName = "Core/ScriptableObjects/TestAcoounts", order = 3)]
    public class TestAccounts : ScriptableObject
    {
        [SerializeField]
        private List<string> testAccounts = new List<string>();

        public List<string> Accounts
        {
            get { return testAccounts; }
        }
    }
}
