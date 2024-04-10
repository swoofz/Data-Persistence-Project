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
    }

    void Start() {
        foreach (GameObject highscore in highscores) {
            GameObject place = highscore.transform.Find("Place").gameObject;
            Debug.Log(place.GetComponent<TextMeshProUGUI>().text);
        }
    }

    public void StartGame() {
        if (usernameInput.text != "") SceneManager.LoadScene(1);
        StartCoroutine("DisplayErrorMessage", usernameErrorLabel);
    }

#if UNITY_EDITOR
    public void Exit() { EditorApplication.ExitPlaymode(); }
#else
    public void Exit() { Application.Quit(); }
#endif


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
