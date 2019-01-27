using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void OnEnable()
    {
        MusicSystem.PlaySong(Songs.SongDictionary[Songs.MainTheme]);
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(2);

    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}

