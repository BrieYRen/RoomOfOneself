/// <summary>
/// this is a interface to store tween values
/// </summary>
public interface ITweenValue
{
    /// <summary>
    /// get value from a tween
    /// </summary>
    /// <param name="percentage"></param>
    void TweenValue(float percentage);

    /// <summary>
    /// readonly variable if the tween will ignore or obey the game's time scale
    /// </summary>
    bool ignoreTimeScale { get; }

    /// <summary>
    /// readonly variable how many seconds would the tween takes
    /// </summary>
    float duration { get;}

    /// <summary>
    /// method to make sure the callback to get tween value is not empty before invoke the callback
    /// </summary>
    /// <returns></returns>
    bool ValidTarget();

    /// <summary>
    /// invoke the callback to do something when a tween is finished
    /// </summary>
    void OnFinish();
}
