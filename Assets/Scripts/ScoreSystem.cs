using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour{
    
    public static ScoreSystem instance;

    public TMP_Text text;

    private int _score;
    private int Score {
        get { return _score; }
        set {
            text.text = "Score: " + value;
            _score = value;
        }
    }

    private void Awake() {
        instance = this;
        IncreaseScore(0);
    }

    public int GetScoreFor(Block block) {
        return 10;
    }

    public static void IncreaseScore(int score) {
        instance.Score += score;
    }
}