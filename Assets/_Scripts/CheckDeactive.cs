using System.Diagnostics;
using UnityEngine;

public class CheckDeactive : MonoBehaviour
{
    void OnDisable()
    {
        if (!Application.isPlaying) return;

        var stack = new StackTrace(true);
        UnityEngine.Debug.Log(
            $"{name} disabled by:\n{stack}",
            this
        );
    }
}
