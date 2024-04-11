using UnityEngine.UI;
using UnityEngine;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public bool hasLoadData { get; private set; }

    public string Username;
    public HighScoreData[] highscores;

    private void Awake() {
        if (Instance != null) { 
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() { LoadHighscores(); }

    [System.Serializable]
    public class SaveData { public HighScoreData[] highscores; }

    [System.Serializable]
    public class HighScoreData {
        public string place;
        public string username;
        public string score;
    }

    public void SaveHighscores() {
        SaveData data = new SaveData();
        data.highscores = highscores;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighscores() {
        hasLoadData = false;

        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)) {
            hasLoadData = true;
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highscores = data.highscores;
        }
    }

    public void StoreHighscoresData(GameObject[] _highscores) {
        highscores = new HighScoreData[_highscores.Length];
        for (int i = 0; i < _highscores.Length; i++) {
            TextMeshProUGUI place, username, score;
            (place, username, score) = GetHighscoreText(_highscores[i]);

            highscores[i] = new HighScoreData {
                place = place.text,
                username = username.text,
                score = score.text,
            };
            hasLoadData = true;
        }
    }

    public void LoadHighScoresData(GameObject[] _highscores) {
        for (int i = 0; i < _highscores.Length; i++) {
            TextMeshProUGUI place, username, score;
            (place, username, score) = GetHighscoreText(_highscores[i]);

            place.SetText(highscores[i].place);
            username.SetText(highscores[i].username);
            score.SetText(highscores[i].score);
        }
    }

    public void UpdateHighscores(string _username, int _score) {
        HighScoreData[] newData = new HighScoreData[highscores.Length];
        int index = 0;
        foreach (HighScoreData highscore in highscores) {
            HighScoreData data;
            data = new HighScoreData() { 
                place=highscore.place, 
                username=highscore.username, 
                score=highscore.score
            };

            if (int.Parse(highscore.score) < _score) {
                data.score = _score.ToString();
                data.username = _username;

                _score = int.Parse(highscore.score);
                _username = highscore.username;
            }

            newData[index] = data;
            index++;
        }

        highscores = newData;
        DisplayHighestScoreText();
    }

    private void DisplayHighestScoreText() {
        Text highscoreText = GameObject.Find("Highest Score Text").transform.GetComponent<Text>();
        highscoreText.text = $"Best Score: {highscores[0].username} : {highscores[0].score}";
    }

    private (TextMeshProUGUI, TextMeshProUGUI, TextMeshProUGUI) GetHighscoreText(GameObject highscore) {
        GameObject place = highscore.transform.Find("Place").gameObject;
        GameObject username = highscore.transform.Find("Name").gameObject;
        GameObject score = highscore.transform.Find("Score").gameObject;

        return (
            place.GetComponent<TextMeshProUGUI>(),
            username.GetComponent<TextMeshProUGUI>(),
            score.GetComponent<TextMeshProUGUI>()
        );
    }
}