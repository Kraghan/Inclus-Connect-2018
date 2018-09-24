using UnityEngine;

public class TimeManager
{

    // If slow motion factor == 1 : time is running normaly
    // If slow motion factor < 1 : time is running slowly
    // If slow motion factor > 1 : time is running quicky
    public static void StartSlowMotion(float slowMotionFactor)
    {
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    // Stop slow motion effect
    public static void StopSlowMotion()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
	
}
