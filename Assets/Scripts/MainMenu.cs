using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void OnEnable()
    {
        SoundSystem.PlaySong(Songs.SongDictionary[Songs.MainTheme]);
        SoundSystem.PlaySongPart("ToIntro");
    }

    public void PlayGame ()
    {
        SoundSystem.PlaySong(Songs.SongDictionary[Songs.MainTheme]);
        SoundSystem.PlaySongPart("ToIntro");
        SceneManager.LoadScene(2);

    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}

