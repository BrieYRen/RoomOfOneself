using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TimeFrozePanel is a derived class of UIPanel, used for panels such as pause menu, when it's open, the time speed will be zero
/// </summary>
public class TimeFrozePanel : UIPanel
{
    static List<TimeFrozePanel> timeFrozePanels = new List<TimeFrozePanel>();

    float previousTimeScale = 1f;

    protected override void OnStart()
    {
        base.OnStart();

        if (IsVisible)
        {
            timeFrozePanels.Add(this);
            Time.timeScale = 0f;
        }
            
    }

    /// <summary>
    /// overrided public method to open the panel 
    /// </summary>
    public override void Show()
    {
        base.Show();

        if (timeFrozePanels.Count == 0)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        if (!timeFrozePanels.Contains(this))
            timeFrozePanels.Add(this);
    }

    /// <summary>
    /// overrided public method to close the panel
    /// </summary>
    public override void Close()
    {
        base.Close();

        if (timeFrozePanels.Contains(this))
            timeFrozePanels.Remove(this);

        if (timeFrozePanels.Count == 0)
            Time.timeScale = previousTimeScale;
    }

}
