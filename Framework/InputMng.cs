using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCodeAssign {
    private bool _isPressed;
    public bool isPressed {
        get { return _isPressed; }
        set {
            if (_isPressed != value) {
                _isPressed = value;
                Debug.Log(keyCode + " Is Pressed: " + _isPressed);
            }
        }
    }
    public KeyCode keyCode;

    public KeyCodeAssign(KeyCode key) {
        keyCode = key;
    }

    public void Update() {
        if (!isPressed && Input.GetKeyDown(keyCode))
            isPressed = true;

        if (isPressed && Input.GetKeyUp(keyCode))
            isPressed = false;
    }
}

public class KeyCodeExectutor {
    public string key;
    public bool onlyExecuteOnce;
    public bool isActivated;
    public Action callFunc;
    public List<KeyCodeAssign> assignedKeys;

    public KeyCodeExectutor(string key, Action callFunc, bool onlyExecuteOnce, params KeyCode[] keyCodes) {
        this.key = key;
        this.onlyExecuteOnce = onlyExecuteOnce;
        this.callFunc = callFunc;

        assignedKeys = new List<KeyCodeAssign>();
        foreach (var k in keyCodes) {
            assignedKeys.Add(new KeyCodeAssign(k));
        }
    }

    public void Update() {
        assignedKeys.ForEach(a => a.Update());

        Debug.Log(key);

        if (assignedKeys.TrueForAll(k => k.isPressed)) {
            if (onlyExecuteOnce && isActivated) {
                return;
            }

            callFunc();
            isActivated = true;
        }
        else {
            isActivated = false;
        }
    }
}

public class InputMng : MonoBehaviour {
    public List<KeyCodeExectutor> executors = new List<KeyCodeExectutor> ();
    public Dictionary<string, KeyCodeExectutor> keysDict = new Dictionary<string, KeyCodeExectutor>();

    private static InputMng _Instance;

    public static InputMng Instance {
        get { return _Instance; }
    }

    public static void Init() {
        _Instance = FindObjectOfType<InputMng>();
    }

    public void Assign(Action p_func, bool p_executeOnce, params KeyCode[] p_keyCodes) {
        var key = KeyCodesToKey (p_keyCodes);

        if (keysDict.ContainsKey(key)) {
            Debug.Log("Key is already assigned, Update Function");
            keysDict[key].callFunc = p_func;
        }
        else {
            var e = new KeyCodeExectutor (key, p_func, p_executeOnce, p_keyCodes);
            keysDict.Add(key, e);
            executors.Add(e);
        }
    }

    public void Assign(Action func, params KeyCode[] p_keyCodes) {
        var key = KeyCodesToKey (p_keyCodes);

        if (keysDict.ContainsKey(key)) {
            Debug.Log("Key is already assigned, Update Function");
            keysDict[key].callFunc = func;
        }
        else {
            var executor = new KeyCodeExectutor (key, func, false, p_keyCodes);
            keysDict.Add(key, executor);
        }
    }

    private string KeyCodesToKey(KeyCode[] p_keyCodes) {
        var retval = string.Empty;
        Array.ForEach(p_keyCodes, k => retval += k.ToString());
        return retval;
    }

    private void Update() {
        executors.ForEach(e => e.Update());
    }

}