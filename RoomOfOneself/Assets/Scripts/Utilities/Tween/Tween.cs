using UnityEngine;

/// <summary>
/// this is a static class for scripts to get tween value with certain easetype
/// </summary>
public static class Tween
{
    /// <summary>
    /// public enum to choose a easy curve type for a tween
    /// </summary>
    public enum EaseType
    {
        Linear,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
    }

    /// <summary>
    /// public method to get the current value of a tween according to the start value, end value, percentate value and its ease type
    /// </summary>
    /// <param name="easeType"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float GetValue(EaseType easeType, float start, float end, float t)
    {
        switch (easeType)
        {
            case EaseType.EaseInQuad:
                return EaseInQuad(start, end, t);
            case EaseType.EaseOutQuad:
                return EaseOutQuad(start, end, t);
            case EaseType.EaseInOutQuad:
                return EaseInOutQuad(start, end, t);
            case EaseType.EaseInBounce:
                return EaseInBounce(start, end, t);
            case EaseType.EaseOutBounce:
                return EaseOutBounce(start, end, t);
            case EaseType.EaseInOutBounce:
                return EaseInOutBounce(start, end, t);
            default:
                return Linear(start, end, t);
        }
    }

    #region Get Tween Value using Different Easetype

    public static float Linear(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, t);
    }

    public static float EaseInQuad(float start, float end, float t)
    {
        float scale = end - start;
        return scale * t * t + start;
    }

    public static float EaseOutQuad(float start, float end, float t)
    {
        float scale = end - start;
        return -scale * t * (2 - t) + start;
    }

    public static float EaseInOutQuad(float start, float end, float t)
    {
        float scale = end - start;
        t *= 2f;
        if (t < 1)
        {
            return scale * .5f * t * t + start;
        }
        else
        {
            t--;
            return -scale * .5f * (t * (t - 2) - 1) + start;
        }
    }

    public static float EaseInBounce(float start, float end, float t)
    {
        float scale = end - start;
        return scale - EaseOutBounce(0f, scale, 1f - t) + start;
    }

    public static float EaseOutBounce(float start, float end, float t)
    {
        float scale = end - start;
        if (t < (1 / 2.75f))
        {
            return scale * (7.5625f * t * t) + start;
        }
        else if (t < (2 / 2.75f))
        {
            t -= (1.5f / 2.75f);
            return scale * (7.5625f * t * t + .75f) + start;
        }
        else if (t < (2.5 / 2.75))
        {
            t -= (2.25f / 2.75f);
            return scale * (7.5625f * t * t + .9375f) + start;
        }
        else
        {
            t -= (2.625f / 2.75f);
            return scale * (7.5625f * t * t + .984375f) + start;
        }
    }

    public static float EaseInOutBounce(float start, float end, float t)
    {
        float scale = end - start;
        if(t<.5f)
        {
            return EaseInBounce(0f, scale, t * 2) * .5f + start;
        }
        else
        {
            return EaseOutBounce(0f, scale, t * 2 - 1) * .5f + scale * .5f + start;
        }
    }

    #endregion

}
