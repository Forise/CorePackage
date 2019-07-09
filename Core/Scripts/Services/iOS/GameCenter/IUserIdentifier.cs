//Developed by Pavel Kravtsov.
#if UNITY_IOS
using System;
using UnityEngine;

namespace Core
{
    public delegate void UserIdentifierUpdated(IUserIdentifier changedUser);

    public interface IUserIdentifier : IEquatable<IUserIdentifier>
    {
        event UserIdentifierUpdated OnUserUpdated;

        string Identifier { get; }
        string Name { get; set; }
        Texture2D Avatar { get; }
        bool CanSave { get; }
        bool IsSignedIn { get; }
    }
}
#endif