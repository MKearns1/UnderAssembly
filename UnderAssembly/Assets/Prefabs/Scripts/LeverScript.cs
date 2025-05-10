using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeverScript : MonoBehaviour
{
    public Transform pivot; // The pivot point of the lever
    public XRGrabInteractable grabInteractable;

    
    
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
        if (grabbingHand != null)
        {
            Vector3 handDirection = grabbingHand.transform.position - pivot.position;

        }

        if (GetComponent<HingeJoint>() != null)
        {
            float z = transform.rotation.eulerAngles.z;
            if (z > 180f) z -= 360f;

            if (z < -15f)
            {
                GameObject.Find("Assembly (2)").transform.Find("Trigger").GetComponent<AssemblyScript>().AssemblySpeed = 10;

            }

            if (z > 15f)
            {
                GameObject.Find("Assembly (2)").transform.Find("Trigger").GetComponent<AssemblyScript>().AssemblySpeed = 0;

            }
        }
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
