using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LiangExtentions {
    public static T GetRandom<T>(this List<T> list) {
        return list[Random.Range(0, list.Count)];
    }

    public static T GetRandom<T>(this T[] array) {
        return array[Random.Range(0, array.Length)];
    }

    public static void Shuffle<T>(this List<T> list) {
        for(int i = 0; i < list.Count; i++) {
            int randIndex = Random.Range(0, list.Count);

            T origin = list[i];
            list[i] = list[randIndex];
            list[randIndex] = origin;
        }
    }

    public static void Shuffle<T>(this T[] array) {
        for (int i = 0; i < array.Length; i++) {
            int randIndex = Random.Range(0, array.Length);

            T origin = array[i];
            array[i] = array[randIndex];
            array[randIndex] = origin;
        }
    }
}
