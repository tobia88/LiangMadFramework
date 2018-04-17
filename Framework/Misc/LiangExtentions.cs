using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LiangExtentions {
    // ===============================================
    // LIST EXTENSIONS
    // ===============================================
    public static T GetRandom<T>(this List<T> _list) {
        return _list[Random.Range(0, _list.Count)];
    }

    public static string ToListString<T>(this List<T> _list) {
        return ToArrayString(_list.ToArray());
    }

    public static void Shuffle<T>(this List<T> _list) {
        for (int i = 0; i < _list.Count; i++) {
            int randIndex = Random.Range(0, _list.Count);

            T origin = _list[i];
            _list[i] = _list[randIndex];
            _list[randIndex] = origin;
        }
    }

    // ===============================================
    // ARRAY EXTENSIONS
    // ===============================================


    public static string ToArrayString<T>(this T[] _list) {
        string retval = _list[0].ToString();

        for (int i = 1; i < _list.Length; i++) {
            retval += ",";
            retval += _list[i].ToString();
        }

        return retval;
    }

    public static T GetRandom<T>(this T[] _array) {
        return _array[Random.Range(0, _array.Length)];
    }

    public static void Shuffle<T>(this T[] _array) {
        for (int i = 0; i < _array.Length; i++) {
            int randIndex = Random.Range(0, _array.Length);

            T origin = _array[i];
            _array[i] = _array[randIndex];
            _array[randIndex] = origin;
        }
    }


    // ===============================================
    // ARRAY EXTENSIONS
    // ===============================================

    public static void Clear(this Transform _trans) {
        for(int i = 0; i < _trans.childCount; i++) {
            if (Application.isPlaying)
                Object.Destroy(_trans.GetChild(i).gameObject);
            else
                Object.DestroyImmediate(_trans.GetChild(i).gameObject);
        }
    }
}
