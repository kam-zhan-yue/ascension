/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public bool waiting;

    //===============PROCEDURE===============//
    public void Stop(float duration)
    //Purpose:          Stops the timer in Unity for a specific duration
    {
        if (waiting)
            return;
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    //===============PROCEDURE===============//
    public IEnumerator Wait(float duration)
    //Purpose:          IEnumerator for pausing the time briefly
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        waiting = false;
    }
}
