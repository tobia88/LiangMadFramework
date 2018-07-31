using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneExtensions
{
    public static T GetComponent<T> (this Scene _scene)
    {
        foreach( var obj in _scene.GetRootGameObjects())
        {
            T retval = obj.GetComponent<T>();
            if (retval != null)
                return retval;
        }

        return default(T);
    }
}
