using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameManager Instance;
    public string Username;

    private void Awake() {
        if (Instance == null) { 
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    [System.Serializable]
    public class SaveData {
        public GameObject[] highscores;
    }
}