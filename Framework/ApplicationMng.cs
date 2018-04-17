using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationMng : MonoBehaviour {
    public static bool IsInit { get; private set; }

    private void Awake() {
        InitializeUnity();
        InitializeGame();

        IsInit = true;
    }

    private void InitializeUnity() {

    }

    private void InitializeGame() {
        GameData.Init();
        AudioMng.Init();
        DataMng.Init();
        InputMng.Init();
        SceneMng.Init();
    }
}
