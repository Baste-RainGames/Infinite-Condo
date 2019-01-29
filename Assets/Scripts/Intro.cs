using System.Collections;
using System.Linq;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public Image IntroImage;
    public string loadlevel;

    IEnumerator Start()
    {
        var loader = GetComponent<StudioBankLoader>();
        loader.Load();

        IntroImage.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSeconds(2.5f);

        FadeOut();
        yield return new WaitForSeconds(2.5f);

        yield return new WaitUntil(() => loader.Banks.All(RuntimeManager.HasBankLoaded));

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
