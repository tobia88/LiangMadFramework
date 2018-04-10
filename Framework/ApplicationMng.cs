using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationMng : MonoBehaviour {
    public static bool IsInit { get; private set; }

    private void Awake() {
        if (IsInit)
            Destroy(gameObject);

        InitializeUnity();
        InitializeGame();

        IsInit = true;
    }

    private void InitializeUnity() {

    }

    private void InitializeGame() {
        AudioMng.Init();
        DataMng.Init();
        InputMng.Init();
        SceneMng.Init();
    }
}
