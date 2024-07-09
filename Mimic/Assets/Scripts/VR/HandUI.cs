using UnityEngine;
using UnityEngine.XR;

public class HandUI : MonoBehaviour
{
    public GameObject menuUI;

    private void Update()
    {   
        InputDevice Ldevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand); 
        InputFeatureUsage<Quaternion> LhandRotationUsage = CommonUsages.deviceRotation;

        InputDevice Rdevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        InputFeatureUsage<Quaternion> RhandRotationUsage = CommonUsages.deviceRotation;

        if (Ldevice.TryGetFeatureValue(LhandRotationUsage, out Quaternion LhandRotation) && Rdevice.TryGetFeatureValue(RhandRotationUsage, out Quaternion RhandRotation))
        {
            Vector3 LeulerRotation = LhandRotation.eulerAngles;
            Vector3 ReulerRotation = RhandRotation.eulerAngles;
           // Debug.Log($"Euler angles: x:{ReulerRotation.x}, y:{ReulerRotation.y}, z:{ReulerRotation.z}");
            if (LeulerRotation.z > 50f && LeulerRotation.z < 90f || ReulerRotation.z > 300f && ReulerRotation.z < 340f) 
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
