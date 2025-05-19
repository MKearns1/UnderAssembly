using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject CameraModel;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        CameraModel = transform.Find("CameraModel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.position - CameraModel.transform.position);

        // Optional: apply rotation offset
       // targetRotation *= Quaternion.Euler(90f, 0f, 0f); // Equivalent to +Vector3.left * 90
        targetRotation.eulerAngles += Vector3.left * 90;


        // Smoothly interpolate towards the target rotation
        CameraModel.transform.rotation = Quaternion.Slerp(
            CameraModel.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed);
    }
}
