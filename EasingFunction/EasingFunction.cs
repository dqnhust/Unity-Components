#pragma warning disable 0649
/// <summary>
/// Some Ease Function from https://easings.net/en
/// </summary>
public static class EasingFunction
{

    /// <summary>
    /// no easing, no acceleration
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float Linear(float t) => t;
    /// <summary>
    /// accelerating from zero velocity
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseInQuad(float t) => t * t;
    /// <summary>
    /// decelerating to zero velocity
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseOutQuad(float t) => t * (2 - t);
    /// <summary>
    /// acceleration until halfway, then deceleration
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseInOutQuad(float t) => t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
    /// <summary>
    /// accelerating from zero velocity 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseInCubic(float t) => t * t * t;
    /// <summary>
    /// decelerating to zero velocity 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseOutCubic(float t) => (--t) * t * t + 1;
    /// <summary>
    /// acceleration until halfway, then deceleration 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseInOutCubic(float t) => t < .5 ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
    /// <summary>
    /// accelerating from zero velocity 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseInQuart(float t) => t * t * t * t;
    /// <summary>
    /// decelerating to zero velocity 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseOutQuart(float t) => 1 - (--t) * t * t * t;
    /// <summary>
    /// acceleration until halfway, then deceleration
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseInOutQuart(float t) => t < .5 ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;
    /// <summary>
    /// accelerating from zero velocity
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseInQuint(float t) => t * t * t * t * t;
    /// <summary>
    /// decelerating to zero velocity
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseOutQuint(float t) => 1 + (--t) * t * t * t * t;
    /// <summary>
    /// acceleration until halfway, then deceleration 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float EaseInOutQuint(float t) => t < .5 ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;
}
