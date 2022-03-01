using UnityEngine.Events;

/// <summary>
/// this is a struct for a float tween
/// </summary>
public struct FloatTween : ITweenValue
{
    FloatTweenCallback targetCallback;
    FloatTweenFinishCallback finishCallback;

    public Tween.EaseType easeType;
    public float startValue;
    public float targetValue;

    public float duration { get; set; }
    public bool ignoreTimeScale { get; set; }

    public bool ValidTarget()
    {
        return targetCallback != null;
    }

    public void TweenValue(float percentage)
    {
        if (!ValidTarget())
            return;

        float value = Tween.GetValue(easeType, startValue, targetValue, percentage);
        targetCallback.Invoke(value);
    }

    public void AddOnChangeCallback(UnityAction<float> newCallback)
    {
        if (targetCallback == null)
            targetCallback = new FloatTweenCallback();

        targetCallback.AddListener(newCallback);
    }

    public void AddOnFinishCallback(UnityAction newFinishCallback)
    {
        if (finishCallback == null)
            finishCallback = new FloatTweenFinishCallback();

        finishCallback.AddListener(newFinishCallback);
    }

    public void OnFinish()
    {
        if (finishCallback != null)
            finishCallback.Invoke();
    }




    public class FloatTweenCallback : UnityEvent<float>
    {
        public FloatTweenCallback() { }
    }

    public class FloatTweenFinishCallback : UnityEvent
    {
        public FloatTweenFinishCallback() { }
    }
}
