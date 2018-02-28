using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Scene : BaseEntity {
    public bool IsUnloading { get; private set; }

    protected virtual void Awake() {
        if (!ApplicationMng.IsInit) {
            ApplicationMng.InitAndBackToCurrentScene();
            return;
        }
        Initialize();
    }

    protected virtual void Initialize() { }

    public virtual void PreUnload() { }

    public IEnumerator Unload() {
        IsUnloading = true;

        Destroy(gameObject);

        yield return Resources.UnloadUnusedAssets();

        System.GC.Collect();

        IsUnloading = false;

    }

    public virtual void OnStart(params object[] args) { }
}
