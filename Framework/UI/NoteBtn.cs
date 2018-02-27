using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteBtn : MonoBehaviour {
    public NoteData noteData;
    public Text buttonTxt;
    public Button button { get; private set; }
    public System.Action<NoteBtn> onPressBtn;
    public Sound clickSnd;

    private void Awake() {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnPressBtn);
    }

    public void Init(NoteData noteData) {
        this.noteData = noteData;
        buttonTxt.text = noteData.note;

        clickSnd.clip = noteData.clip;
    }

    public void PlaySound() {
        AudioMng.Instance.PlayOneShot(clickSnd);
    }

    private void OnPressBtn() {
        if (onPressBtn != null)
            onPressBtn(this);

        PlaySound();
    }
}
