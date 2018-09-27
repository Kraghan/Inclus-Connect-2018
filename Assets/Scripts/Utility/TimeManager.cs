using Scripting.GameManagers;
using UnityEngine;

namespace Scripting.Utility
{

    public class TimeManager
    {

        // If slow motion factor == 1 : time is running normaly
        // If slow motion factor < 1 : time is running slowly
        // If slow motion factor > 1 : time is running quicky
        public static void StartSlowMotion(float slowMotionFactor = 0.25f)
        {
            Time.timeScale = slowMotionFactor;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            Managers.instance.soundManager.SetRTPCvalue("SlowMo", slowMotionFactor, Managers.instance.soundManager.soundNode);
            //Debug.LogFormat("Slomo factor : {0}", slowMotionFactor);
        }

        // Stop slow motion effect
        public static void StopSlowMotion()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            Managers.instance.soundManager.SetRTPCvalue("SlowMo", 1f, Managers.instance.soundManager.soundNode);
            //Debug.Log("Slomo stopped");
        }

    }
}
