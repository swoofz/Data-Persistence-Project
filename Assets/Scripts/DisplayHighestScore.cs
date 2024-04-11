using UnityEngine;
using UnityEngine.UI;


public class DisplayHighestScore : MonoBehaviour {
    private void Awake() {
        if (!GameManager.Instance) return;

        Text text = GetComponent<Text>();
        GameManager.HighScoreData highscore = GameManager.Instance.highscores[0];
        text.text = $"Best Score: {highscore.username} : {highscore.score}";
    }

}
