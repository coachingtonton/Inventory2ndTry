using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
///THIS SCRIPT will be attatched to camera and exist as a static class 
///Other classes will call it in their weapon handlers 
///The SO of the weapon WILL deterimine if it has HitStop, and how long 
///The HITSTOP duration is. 

{
    public static HitStop instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Freeze(float weaponSOHitStop)
    {
        StartCoroutine(HitStopCoroutine(weaponSOHitStop));
    }

    public IEnumerator HitStopCoroutine(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        // WAIT FOR SECONDS REAL TIME CONTINUES COUINTING EVEN WHEN THE GAMES TIME IS STOPPED
        Time.timeScale = 1;
    }

}
