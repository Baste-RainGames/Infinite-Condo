using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour {
    
    public static bool GameIsOver { get; private set; }

    public static bool InGame { get; private set; }

    public GameObject gameOverParent;

    public GameObject InGameParent;

    private void OnEnable() {
        GameIsOver = false;
    }

    public void DoGameOver() {
        SoundSystem.PlaySong(Songs.SongDictionary[Songs.GameOver]);
        GameIsOver = true;
        gameOverParent.SetActive(true);
        InGameParent.SetActive(false);
    }

    public void MainMenu()
    {
        SoundSystem.PlaySong(Songs.SongDictionary[Songs.MainTheme]);
        SoundSystem.PlaySongPart("ToIntro");
        SceneManager.LoadScene(1);
    }


    public void PlayAgain()
    {
        SoundSystem.PlaySong(Songs.SongDictionary[Songs.MainTheme]);
        SoundSystem.PlaySongPart("ToIntro");
        SceneManager.LoadScene(2);
    }
}
