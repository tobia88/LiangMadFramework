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
    public MonoBehaviour owner;
    public Action callFunc;
    public List<KeyCodeAssign> assignedKeys;

    public KeyCodeExectutor(MonoBehaviour _owner, string _key, Action _callFunc, bool _onlyExecuteOnce, params KeyCode[] keyCodes) {
        owner = _owner;
        key = _key;
        onlyExecuteOnce = _onlyExecuteOnce;
        callFunc = _callFunc;

        assignedKeys = new List<KeyCodeAssign>();

        foreach (var k in keyCodes) {
            assignedKeys.Add(new KeyCodeAssign(k));
        }
    }

    public void Update() {
        assignedKeys.ForEach(a => a.Update());

        if (assignedKeys.TrueForAll(k => k.isPressed)) {
            if (onlyExecuteOnce && isActivated) {
                return;
            }

            if (callFunc != null)
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

    public void Assign(MonoBehaviour _owner, Action _func, bool _executeOnce, params KeyCode[] _keyCodes) {
        var key = KeyCodesToKey (_keyCodes);

        if (keysDict.ContainsKey(key)) {
            Debug.Log("[InputMng]: Key is already assigned, Update Function");
            keysDict[key].callFunc = _func;
        }
        else {
            Debug.Log("[InputMng]: Key " + _keyCodes.ToArrayString() + " Is Succeed Assigned");
            var e = new KeyCodeExectutor (_owner, key, _func, _executeOnce, _keyCodes);
            keysDict.Add(key, e);
            executors.Add(e);
        }
    }

    private string KeyCodesToKey(KeyCode[] p_keyCodes) {
        var retval = string.Empty;
        Array.ForEach(p_keyCodes, k => retval += k.ToString());
        return retval;
    }

    private void Update() {
        for (int i = executors.Count - 1; i >= 0; i--) {
            if (executors[i].owner == null) {
                executors.RemoveAt(i);
            }
            else
                executors[i].Update();
        }
    }

}