/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator circleWipe;
    public float transitionTime = 1f;

    //===============PROCEDURE===============//
    public IEnumerator LoadLevel(int level)
    //Purpose:          Plays the level transition animation
    {
        circleWipe.SetTrigger("EndLevel");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(level);
    }
}
