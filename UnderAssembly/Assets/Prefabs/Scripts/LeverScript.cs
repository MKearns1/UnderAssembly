using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeverScript : MonoBehaviour
{
    public Transform pivot; // The pivot point of the lever
    public XRGrabInteractable grabInteractable;
    bool playedSound;
    
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
                GameObject.Find("Assembly (2)").transform.Find("Trigger").GetComponent<AssemblyScript>().ON = true;
                if(!playedSound)
                {
                    playedSound = true;
                    SoundManagerScript.Instance.PlaySound("LeverSound", gameObject, false, 1);
                }
            }
            else
            if (z > 15f)
            {
                GameObject.Find("Assembly (2)").transform.Find("Trigger").GetComponent<AssemblyScript>().AssemblySpeed = 0;
                GameObject.Find("Assembly (2)").transform.Find("Trigger").GetComponent<AssemblyScript>().ON = false;
                if (!playedSound)
                {
                    playedSound = true;
                    SoundManagerScript.Instance.PlaySound("LeverSound", gameObject, false, 1);
                }
            }
            else
            {
                playedSound = false;
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
