//Developed by Pavel Kravtsov.
#if UNTY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

namespace Core
{
    public class GameCenterManager : MonoSingleton<GameCenterManager>
    {
        public UserIdentifierIOS UserIdentifier { get; set; }


        void Awake()
        {
            UserIdentifier = new UserIdentifierIOS();
        }
    }
}
#endif
