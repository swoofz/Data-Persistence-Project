using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class MenuUIHandler : MonoBehaviour {

    public GameObject username;
    public GameObject highscoreObj;

    private InputField usernameInput;
    private TextMeshProUGUI usernameErrorLabel;
    private GameObject[] highscores;

    void Awake() {
        usernameInput = username.transform.Find("Input").GetComponent<InputField>();
        usernameErrorLabel = username.transform.Find("Error Label").GetComponent<TextMeshProUGUI>();

        highscores = new GameObject[highscoreObj.transform.childCount];
        for (int i = 0; i < highscoreObj.transform.childCount; i++) {
            highscores[i] = highscoreObj.transform.GetChild(i).gameObject;
        }

        // Load in Game data
        GameManager.Instance.highscores = highscores;
        usernameInput.text = GameManager.Instance.Username;
    }

    public void StartGame() {
        if (usernameInput.text == "") {
            StartCoroutine("DisplayErrorMessage", usernameErrorLabel);
            return;
        }

        // Username Persistence through scenes
        GameManager.Instance.Username = usernameInput.text;
        SceneManager.LoadScene(1);
    }

    public void Exit() {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

        GameManager.Instance.SaveHighscores();
    }


    private IEnumerator DisplayErrorMessage(TextMeshProUGUI _errorLabel) {
        _errorLabel.alpha = 1f;
        yield return new WaitForSeconds(5f);
        while (_errorLabel.alpha > 0f) {
            _errorLabel.alpha -= Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }
        StopCoroutine("DisplayErrorMessage");
    }
}
