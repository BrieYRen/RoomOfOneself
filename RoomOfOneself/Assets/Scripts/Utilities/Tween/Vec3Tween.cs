using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// this is a struct for a vector 3 tween
/// </summary>
public struct Vec3Tween : ITweenValue
{
    Vec3TweenCallback targetCallback;
    Vec3TweenFinishCallback finishCallback;

    public Tween.EaseType easeType;
    public Vector3 startValue;
    public Vector3 targetValue;

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

        float valueX = Tween.GetValue(easeType, startValue.x, targetValue.x, percentage);
        float valueY = Tween.GetValue(easeType, startValue.y, targetValue.y, percentage);
        float valueZ = Tween.GetValue(easeType, startValue.z, targetValue.z, percentage);
        targetCallback.Invoke(new Vector3(valueX,valueY,valueZ));
    }

    public void AddOnChangeCallback(UnityAction<Vector3> newCallback)
    {
        if (targetCallback == null)
            targetCallback = new Vec3TweenCallback();

        targetCallback.AddListener(newCallback);
    }

    public void AddOnFinishCallback(UnityAction newFinishCallback)
    {
        if (finishCallback == null)
            finishCallback = new Vec3TweenFinishCallback();

        finishCallback.AddListener(newFinishCallback);
    }

    public void OnFinish()
    {
        if (finishCallback != null)
            finishCallback.Invoke();
    }



    public class Vec3TweenCallback : UnityEvent<Vector3>
    {
        public Vec3TweenCallback() { }
    }

    public class Vec3TweenFinishCallback : UnityEvent
    {
        public Vec3TweenFinishCallback() { }
    }
}
