using UnityEngine;
using UnityEngine.XR;

public class OculusWristMenuController : MonoBehaviour
{
    public GameObject menuUI;

    private void Update()
    {   
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand); 
        InputFeatureUsage<Quaternion> handRotationUsage = CommonUsages.deviceRotation;
        if (device.TryGetFeatureValue(handRotationUsage, out Quaternion handRotation))
        {
            Vector3 eulerRotation = handRotation.eulerAngles;
           // Debug.Log($"Euler angles: x:{eulerRotation.x}, y:{eulerRotation.y}, z:{eulerRotation.z}");
            if (eulerRotation.z > 50f && eulerRotation.z < 90f) 
            {
                menuUI.SetActive(true);
            }
            else
            {
                menuUI.SetActive(false);
            }
        }
    }
}
