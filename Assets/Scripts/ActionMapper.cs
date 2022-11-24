using UnityEngine;

public class ActionMapper
{
    public static bool GetDevicesTrigger() {
        return ActionMapper.GetMouseTrigger() 
            || ActionMapper.GetCardboardTrigger() 
            || ActionMapper.GetOculusTrigger();
    }

    // Mouse Trigger
    private static bool GetMouseTrigger() {
        return Input.GetMouseButtonDown(0); // Left-click
    }

    // Cardboard Trigger
    private static bool GetCardboardTrigger() {
        return Google.XR.Cardboard.Api.IsTriggerPressed;
    }

    // Occulus Trigger
    private static bool GetOculusTrigger() {
        bool value = false;
        if (UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand).TryGetFeatureValue(new ("TriggerTouch"), out value)) {
            return value;
        }
        return value;
    }
}