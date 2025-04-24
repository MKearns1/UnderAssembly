#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Management;

[InitializeOnLoad]
public class XRPlayModeReset
{
    static XRPlayModeReset()
    {
        // Subscribe to play mode state change (e.g., when you press stop)
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    static void OnPlayModeChanged(PlayModeStateChange state)
    {
        // When exiting play mode (pressing Stop)
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            Debug.Log("?? Deinitializing XR on Play mode exit...");

            if (XRGeneralSettings.Instance != null &&
                XRGeneralSettings.Instance.Manager != null &&
                XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
        }
    }
}
#endif
