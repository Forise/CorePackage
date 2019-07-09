//Developed by Pavel Kravtsov.
using Core.EventSystem;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MyDropDown : TMPro.TMP_Dropdown
{
    private void OnMouseDown()
    {
        EventManager.Notify(this, new GameEventArgs(Events.InputEvents.UI_TAPPED));
    }
}
