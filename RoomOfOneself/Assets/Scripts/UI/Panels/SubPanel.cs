using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SubPanel is a derived class of UIPanel use for panels that will only be toggled through buttons in other panels
/// </summary>
public class SubPanel : UIPanel
{  
    protected override void OnStart()
    {
        base.OnStart();

        keyCode = KeyCode.None;
        isTopOnShow = true;
        lockHotkeyOnShow = LockHotkeyType.None;
        hideCursorOnClose = false;
        fadeType = FadeType.AlphaOnly;
    }


}
