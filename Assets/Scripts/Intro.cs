using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public Image IntroImage;
    public string loadlevel;

    IEnumerator Start()
    {
        IntroImage.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSeconds(2.5f);

        FadeOut();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(1);
    }

  
    void FadeIn()
    {
        IntroImage.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOut()
    {
        IntroImage.CrossFadeAlpha(0.0f, 2.5f, false);
    }
}
