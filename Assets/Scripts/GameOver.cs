using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour {
    
    public static bool GameIsOver { get; private set; }

    public GameObject gameOverParent;
    
    private void OnEnable() {
        GameIsOver = false;
    }

    public void DoGameOver() {
        GameIsOver = true;
        gameOverParent.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
}
