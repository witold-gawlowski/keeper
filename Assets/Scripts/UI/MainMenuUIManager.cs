using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : Singleton<MainMenuUIManager>
{
    public System.Action onLevelSelectionButtonPressEvent;
    public void OnStartPress()
    {
        onLevelSelectionButtonPressEvent();
    }

}
