using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapInputScript : MonoBehaviour, IInteractable
{
    public string InputObject;
    public GameObject ParentObject;
    bool attachedToObject;
    private XRGrabInteractable grabInteractable;
    bool isHeld;
    bool OnAssemblyLine;
    public bool InheritParentColour;
    GameObject VisualObject;
    

    // Start is called before the first frame update
    void Start()
    {
        grabInteractable = transform.parent.GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        VisualObject = transform.parent.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {      
       
    }

    private void OnDestroy()
    {
        // Prevent memory leaks
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
    }
    public void SetOnAssembly(bool state)
    {
        OnAssemblyLine = state;
    }
}
