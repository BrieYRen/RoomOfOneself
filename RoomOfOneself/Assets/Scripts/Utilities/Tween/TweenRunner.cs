using System.Collections;
using UnityEngine;

/// <summary>
/// this is a class to run a tween in a container
/// </summary>
/// <typeparam name="T"></typeparam>
public class TweenRunner<T> where T : struct, ITweenValue
{
    protected MonoBehaviour coroutineContainer;
    protected IEnumerator tweenIEnumerator;

    /// <summary>
    /// configurate a container to run the coroutine
    /// </summary>
    /// <param name="newContainer"></param>
    public void Init(MonoBehaviour newContainer)
    {
        coroutineContainer = newContainer;
    }

    /// <summary>
    /// if there is a container and it's active in hierarchy, start the coroutine
    /// </summary>
    /// <param name="tweenInfo"></param>
    public void StartTween(T tweenInfo)
    {
        if (coroutineContainer == null)
            return;

        StopTween();

        if (!coroutineContainer.gameObject.activeInHierarchy)
        {
            tweenInfo.TweenValue(1f);
            return;
        }

        tweenIEnumerator = Start(tweenInfo);
        coroutineContainer.StartCoroutine(tweenIEnumerator);
    }

    /// <summary>
    /// stop a tween through stop its coroutine
    /// </summary>
    public void StopTween()
    {
        if (tweenIEnumerator != null)
        {
            coroutineContainer.StopCoroutine(tweenIEnumerator);
            tweenIEnumerator = null;
        }
    }

    /// <summary>
    /// get the updated tween value every frame during its duration time, and do something when its finished
    /// </summary>
    /// <param name="tweenInfo"></param>
    /// <returns></returns>
    static IEnumerator Start(T tweenInfo)
    {
        if (!tweenInfo.ValidTarget())
            yield break;

        float passedTime = 0f;
        while (passedTime < tweenInfo.duration)
        {
            passedTime += tweenInfo.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            float percentage = Mathf.Clamp(passedTime / tweenInfo.duration, 0f, 1f);
            tweenInfo.TweenValue(percentage);
            yield return null;
        }

        tweenInfo.TweenValue(1f);
        tweenInfo.OnFinish();
    }

}
