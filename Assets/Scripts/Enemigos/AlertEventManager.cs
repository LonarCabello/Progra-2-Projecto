using UnityEngine;
using System;

public static class AlertEventManager
{
    public static event Action<Transform, Transform> OnAlert;

    public static void SendAlert(Transform target, Transform sender)
    {
        OnAlert?.Invoke(target, sender);
    }
}
