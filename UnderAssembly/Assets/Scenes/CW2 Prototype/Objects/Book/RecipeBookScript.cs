using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RecipeBookScript : MonoBehaviour
{
    Animator Animator;
    bool open;
    bool isHeld;
    private XRGrabInteractable grabInteractable;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();

        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHeld)
        {
            if (!open)
            {
                Animator.SetBool("Open", true);
                open = true;
                Debug.Log("OPpen");
            }
        }
        else
        {
            if (open)
            {
                Animator.SetBool("Open", false);
                open = false;
                Debug.Log("OPpen");
            }
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
        Debug.Log("Spray can is now held.");
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        Debug.Log("Spray can is now released.");
    }
}
