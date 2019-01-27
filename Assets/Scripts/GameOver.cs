using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour {
    
    public static bool GameIsOver { get; private set; }

    public GameObject gameOverParent;
    
    private void OnEnable() {
        GameIsOver = false;
    }

    public void DoGameOver() {
        MusicSystem.PlaySong(Songs.SongDictionary[Songs.GameOver]);
        GameIsOver = true;
        gameOverParent.SetActive(true);
    }

    public void MainMenu()
    {
        MusicSystem.PlaySong(Songs.SongDictionary[Songs.MainTheme]);
        MusicSystem.PlaySongPart("ToIntro");
        SceneManager.LoadScene(0);
    }


    public void PlayAgain()
    {
        MusicSystem.PlaySong(Songs.SongDictionary[Songs.MainTheme]);
        MusicSystem.PlaySongPart("ToIntro");
        SceneManager.LoadScene(1);
    }
}
