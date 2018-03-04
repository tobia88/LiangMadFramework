using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationMng : MonoBehaviour {
    public static bool IsInit { get; private set; }

    private static string m_nextSceneName;

    public static void InitAndBackToCurrentScene() {
        m_nextSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("ApplicationScene");
    }

    private void Awake() {
        InitializeUnity();
        InitializeGame();

        IsInit = true;
        SwitchScene();
    }

    private void InitializeUnity() {

    }

    private void InitializeGame() {
        AudioMng.Init();
        DataMng.Init();
        InputMng.Init();
        SceneMng.Init();
    }

    private void SwitchScene() {
        if (m_nextSceneName != null) {
            SceneMng.Instance.LoadScene(m_nextSceneName);
            m_nextSceneName = null;
        }

    }
}
