using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator circleWipe;
    public float transitionTime = 1f;

    public IEnumerator LoadLevel(int level)
    {
        circleWipe.SetTrigger("EndLevel");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(level);
    }
}
