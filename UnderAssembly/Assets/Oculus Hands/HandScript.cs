using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandScript : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;
    List<InputDevice> devices;

    public GameObject handModelPrefab;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }

    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger0", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger0", 0);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripvalue))
        {
            handAnimator.SetFloat("Grip", gripvalue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            UpdateHandAnimation();
        }




        //if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
        //{
        //    Debug.Log("sdasdasd");
        //}
        //if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
        //{

        //}


    }

    void TryInitialize()
    {
        devices = new List<InputDevice>();
        //InputDeviceCharacteristics ControllerCharacteristics = InputDeviceCharacteristics.Controller;
        //InputDevices.GetDevices(devices);
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        //foreach (InputDevice device in devices)
        //{
        //    Debug.Log(device.name);
        //}
        if (devices.Count > 0)
        {
            targetDevice = devices[0];

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }
}
