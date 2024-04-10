using System.Collections;
using System.IO;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public string Username;
    public GameObject[] highscores;

    private void Awake() {
        if (Instance != null) { 
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        LoadHighscores();
    }


    [System.Serializable]
    public class SaveData {
        public HighScoreData[] highscores;
    }

    [System.Serializable]
    public class HighScoreData {
        public string place;
        public string username;
        public string score;
    }

    public void SaveHighscores() {
        SaveData data = new SaveData();
        data.highscores = new HighScoreData[highscores.Length];
        for (int i = 0; i < highscores.Length; i++) {
            TextMeshProUGUI place, username, score;
            (place, username, score) = GetHighscoreText(i);

            data.highscores[i] = new HighScoreData {
                place = place.text,
                username = username.text,
                score = score.text,
            };
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighscores() {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            for (int i = 0; i < data.highscores.Length; i++) {
                TextMeshProUGUI place, username, score;
                (place, username, score) = GetHighscoreText(i);

                place.SetText(data.highscores[i].place);
                username.SetText(data.highscores[i].username);
                score.SetText(data.highscores[i].score);
            }
        }
    }

    private (TextMeshProUGUI, TextMeshProUGUI, TextMeshProUGUI) GetHighscoreText(int index) {
        GameObject place = highscores[index].transform.Find("Place").gameObject;
        GameObject username = highscores[index].transform.Find("Name").gameObject;
        GameObject score = highscores[index].transform.Find("Score").gameObject;

        return (
            place.GetComponent<TextMeshProUGUI>(),
            username.GetComponent<TextMeshProUGUI>(),
            score.GetComponent<TextMeshProUGUI>()
        );
    }
}