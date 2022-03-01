using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this is the base class for all ui panels, including how to trigger the panel, what will it do when show and close, as well as the sound effect and the tween effect
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour 
{
    #region Variables
   
    [Header("Trigger Method")]

    [SerializeField]
    [Tooltip("The keycode to open or close the panel")]
    protected KeyCode keyCode = KeyCode.None;

    [SerializeField]
    [Tooltip("If player should click or hold the keycode to open the panel")]
    protected KeyTriggerType triggerType = KeyTriggerType.Click;
    public KeyTriggerType TriggerType
    {
        get
        {
            return triggerType;
        }
    }


    [Header("OnShow")]

    [SerializeField]
    [Tooltip("Will this panel be bring to the top of all panels when it's open")]
    protected bool isTopOnShow = true;

    [SerializeField]
    [Tooltip("When this panel is opened, can player use hotkey to trigger other panels or only close this panel?")]
    protected LockHotkeyType lockHotkeyOnShow = LockHotkeyType.AllowAll;


    [Header("OnClose")]

    [SerializeField]
    [Tooltip("Check true if when the panel is closed the cursor will be hidden. ususally used in first-person camera projects")]
    protected bool hideCursorOnClose = false;


    [Header("Audio")]

    [SerializeField]
    [Tooltip("The name of the audio that will play once when the panel is open")]
    protected string openSfxName;

    [SerializeField]
    [Tooltip("The name of the audio that will play once when the panel is close")]
    protected string closeSfxName;


    [Header("Tween")]

    [SerializeField]
    [Tooltip("Will the panel fade its transparency when close, or it would fade with both transparency and size")]
    protected FadeType fadeType = FadeType.AlphaScale;

    [SerializeField]
    [Tooltip("Which tween type will used in the fade procedure")]
    Tween.EaseType easeType = Tween.EaseType.EaseInOutQuad;

    [SerializeField]
    [Tooltip("How many seconds will used to finish the tween")]
    protected float tweenDuration = .75f;
    public float TweenDuration
    {
        get
        {
            return tweenDuration;
        }
    }

    [SerializeField]
    [Tooltip("When the game time is frozen or in a fast speed mode, will the tween procedure cost the same time as it is in a normal time speed mode ")]
    protected bool ignoreTimeScale = true;


    protected static CursorLockMode previousCursorLockMode;
    protected static bool previousCursorVisibility;

    protected RectTransform rectTransform;
    protected CanvasGroup canvasGroup;
    
    public bool IsVisible
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            return canvasGroup.alpha == 1f;
        }
    }

    protected static List<UIPanel> visiblePanels = new List<UIPanel>();
    protected static List<UIPanel> visibleLockAllKeyPanels = new List<UIPanel>();

    protected Scrollbar[] scrollbars;

    protected bool isShowing;

    protected PanelInputHandler panelInputHandler;
    AudioManager audioManager;

    TweenRunner<FloatTween> alphaTweenRunner;
    TweenRunner<Vec3Tween> scaleTweenRunner;

    #endregion

    private void Start()
    {
        panelInputHandler = GameManager.instance.panelInputHandler;
        panelInputHandler.RegisterInput(keyCode, this);

        audioManager = GameManager.instance.audioManager;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        scrollbars = GetComponentsInChildren<Scrollbar>();

        isShowing = IsVisible;
        if (!IsVisible && fadeType == FadeType.AlphaScale)
            rectTransform.localScale = Vector3.zero;
        else
        {
            visiblePanels.Add(this);
          
            if (lockHotkeyOnShow == LockHotkeyType.LockAll)
                visibleLockAllKeyPanels.Add(this);

            if (lockHotkeyOnShow == LockHotkeyType.OnlyAllowMe)
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
                rectTransform.localScale = Vector3.zero;
                isShowing = false;
                visiblePanels.Remove(this);
            }
        }

        
        if (alphaTweenRunner == null)
            alphaTweenRunner = new TweenRunner<FloatTween>();
        alphaTweenRunner.Init(this);

        if (scaleTweenRunner == null)
            scaleTweenRunner = new TweenRunner<Vec3Tween>();
        scaleTweenRunner.Init(this);
  

        OnStart();
        StartCoroutine(OnDelayStart());
    }

    protected virtual void OnStart()
    {
        // do something in override
    }

    IEnumerator OnDelayStart()
    {
        yield return null;

        if (visibleLockAllKeyPanels.Count > 0)
            panelInputHandler.EnterLockAllMode();

    }


    /// <summary>
    /// public method to show the panel
    /// </summary>
    public virtual void Show()
    {
        if (isShowing)
            return;

        isShowing = true;

        if (isTopOnShow)
            Focus();
      
        TweenCanvasAlpha(canvasGroup.alpha, 1f);

        if(fadeType == FadeType.AlphaScale)
            TweenCanvasScale(Vector3.ClampMagnitude(rectTransform.localScale, 1.732f), Vector3.one);

        if(openSfxName != null)
            audioManager.PlayIfHasAudio(openSfxName, 0f);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Canvas.ForceUpdateCanvases();
        for (int i = 0; i < scrollbars.Length; i++)
        {
            scrollbars[i].value = 1f;
        }

        if (hideCursorOnClose)
        {
            if (visiblePanels.Count == 0)
            {
                previousCursorLockMode = Cursor.lockState;
                previousCursorVisibility = Cursor.visible;
            }
            visiblePanels.Add(this);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (lockHotkeyOnShow == LockHotkeyType.LockAll)
        {
            if (visibleLockAllKeyPanels.Count == 0)
            {
                panelInputHandler.EnterLockAllMode();
            }
            visibleLockAllKeyPanels.Add(this);
        }
        else if (lockHotkeyOnShow == LockHotkeyType.OnlyAllowMe)
            panelInputHandler.EnterAllowOneMode(keyCode, this, triggerType);

    }


    /// <summary>
    /// public method to close the panel
    /// </summary>
    public virtual void Close()
    {
        if (!isShowing)
            return;

        isShowing = false;

        TweenCanvasAlpha(canvasGroup.alpha, 0f);

        if (fadeType == FadeType.AlphaScale)
            TweenCanvasScale(rectTransform.localScale, Vector3.zero);

        if (closeSfxName != null)
        audioManager.PlayIfHasAudio(closeSfxName, 0f);

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (hideCursorOnClose)
        {
            visiblePanels.Remove(this);
            if (visiblePanels.Count == 0)
            {
                Cursor.lockState = previousCursorLockMode;
                Cursor.visible = previousCursorVisibility;
            }
        }

        if (lockHotkeyOnShow == LockHotkeyType.LockAll)
        {
            if (visibleLockAllKeyPanels.Contains(this))
                visibleLockAllKeyPanels.Remove(this);

            if (visibleLockAllKeyPanels.Count == 0)
                panelInputHandler.ExitLockAllMode();
        }
        else if (lockHotkeyOnShow == LockHotkeyType.OnlyAllowMe)
        {
            for(int i = 0; i < visiblePanels.Count;i++)
            {
                visiblePanels[i].Close();
            }
            panelInputHandler.ExitAllowOneMode();
        }
            
        
    }

    
    /// <summary>
    /// fade in or out canvasGroup with its transparency
    /// </summary>
    /// <param name="startValue"></param>
    /// <param name="endValue"></param>
    void TweenCanvasAlpha(float startValue, float endValue)
    {       
        FloatTween alphaTween = new FloatTween
        {
            easeType = easeType,
            duration = tweenDuration,
            startValue = startValue,
            targetValue = endValue,
            ignoreTimeScale = ignoreTimeScale
        };

        alphaTween.AddOnChangeCallback((float value) => { canvasGroup.alpha = value; });

        alphaTweenRunner.StartTween(alphaTween);
    }

    /// <summary>
    /// fade in or out canvasGroup with its scale
    /// </summary>
    /// <param name="startValue"></param>
    /// <param name="endValue"></param>
    void TweenCanvasScale(Vector3 startValue, Vector3 endValue)
    {
        float tempFloat;
        if (fadeType == FadeType.AlphaScale)
        {
            tempFloat = tweenDuration;
        }
        else
        {
            tempFloat = 0f;
        }

        Vec3Tween scaleTween = new Vec3Tween
        {
            easeType = easeType,
            duration = tempFloat,
            startValue = startValue,
            targetValue = endValue,
            ignoreTimeScale = ignoreTimeScale
        };

        scaleTween.AddOnChangeCallback((Vector3 value) => { rectTransform.localScale = value; });

        scaleTweenRunner.StartTween(scaleTween);
    }



    /// <summary>
    /// public method to toggle the visibility of the panel
    /// </summary>
    public virtual void Toggle()
    {
        if (!IsVisible)
        {
            Show();
        }
        else
        {
            Close();
        }
    }

    /// <summary>
    /// public method to move the panel on top
    /// </summary>
    public virtual void Focus()
    {
        rectTransform.SetAsLastSibling();
    }

    /// <summary>
    /// unregister this panel with its input keycode
    /// </summary>
    protected virtual void OnDestroy()
    {
        panelInputHandler.UnregisterInput(keyCode, this);
    }

    public enum FadeType
    {
        /// <summary>
        /// fade with only the alpha of the panel, usually used in main menu or sub menus
        /// </summary>
        AlphaOnly,

        /// <summary>
        /// fade with both the alpha and the scale of the panel, a default fade effect for mose panels
        /// </summary>
        AlphaScale,
    }

    protected enum LockHotkeyType
    {
        /// <summary>
        /// when the panel is Show(), lock all hotkeys so that no ui panel will be triggered, usually use in main menu or setting memu
        /// </summary>
        LockAll,

        /// <summary>
        /// lock all hotkeys except the one to toggle self, usually use in for instance pause menu
        /// </summary>
        OnlyAllowMe,

        /// <summary>
        /// allow all hotkey when this panel is show(), the default type for most hud panels
        /// </summary>
        AllowAll,

        /// <summary>
        /// use in SubPanel when lock key or not will be determined by the parent UIPanel
        /// </summary>
        None,
    }
}


