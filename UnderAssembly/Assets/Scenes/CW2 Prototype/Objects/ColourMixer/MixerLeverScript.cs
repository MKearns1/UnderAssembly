using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.ParticleSystem;

public class MixerLeverScript : MonoBehaviour
{
    public Transform pivot; // The pivot point of the lever
    public XRGrabInteractable grabInteractable;
    private float pullTimer = 0f;
    public float unitDispenseTime = 1f;
    private bool unitDispensed = false;

    public bool LeverDown;

    private GameObject grabbingHand;

    void Start()
    {
        if (grabInteractable == null)
            grabInteractable = transform.GetChild(0).GetComponent<XRGrabInteractable>();

        // grabInteractable.selectEntered.AddListener(OnGrab);
        // grabInteractable.selectExited.AddListener(OnRelease);
    }

    void Update()
    {

        LeverDown = false;


        if (grabbingHand != null)
        {
            Vector3 handDirection = grabbingHand.transform.position - pivot.position;

        }

        if (GetComponent<HingeJoint>() != null)
        {
            float z = transform.rotation.eulerAngles.z;
            if (z > 180f) z -= 360f;


            if (z < 50)
            {
                LeverDown = false;

                //if (pullTimer >= unitDispenseTime && !unitDispensed)
                {
                    DispenseDyeUnit();
                    unitDispensed = true; Debug.Log("Up");

                    // StopSprayAfterDelay(0.5f); // Stops after a short burst
                }

            }

            if (z > 110)
            {
                LeverDown = true;
                pullTimer += Time.deltaTime;

                if(pullTimer < 1)
                {
                    Debug.Log("Down");

                }

            }
        }
    }


    void DispenseDyeUnit()
    {

    }

    void OnGrab(SelectEnterEventArgs args)
    {
        grabbingHand = args.interactorObject.transform.gameObject;
        Debug.Log(grabbingHand);

    }

    void OnRelease(SelectExitEventArgs args)
    {
        grabbingHand = null;
    }
}
