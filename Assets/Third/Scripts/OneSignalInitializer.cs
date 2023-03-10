using OneSignalSDK;
using UnityEngine;

public class OneSignalInitializer : MonoBehaviour
{
    private void Start() => OneSignal.Default.Initialize("9714010e-053b-4ff4-80f2-116755222bcb");
}