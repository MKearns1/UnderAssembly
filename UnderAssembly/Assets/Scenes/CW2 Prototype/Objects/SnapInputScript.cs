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
        if (isHeld)
        {
            ParentObject.GetComponent<Rigidbody>().excludeLayers = LayerMask.GetMask("BaseObjects");
        }
        else if(ParentObject.GetComponent<Rigidbody>() != null)
        {
            ParentObject.GetComponent<Rigidbody>().excludeLayers = 0;

        }

        if (GeneralScript.Instance.productSnapPoints.Count > 0)
        {
            foreach (Transform t in GeneralScript.Instance.productSnapPoints)
            {
                if ((t != null))
                {
                    if (Vector3.Distance(transform.position, t.position) < .05f)
                    {
                        Debug.Log("CLOSETOOBJECT");
                        if (!attachedToObject)
                        {
                            if (!t.GetComponent<SnapTriggerScript>().Filled)
                            {

                                if (InputObject == t.GetComponent<SnapTriggerScript>().RequiredInputObject)
                                {

                                    attachedToObject = true;
                                    //ParentObject.transform.position = t.position;
                                    t.GetComponent<SnapTriggerScript>().SnapObject(this.gameObject);


                                }

                            }

                        }
                    }
                    if (InheritParentColour && attachedToObject)
                    {
                        VisualObject.GetComponent<ChangeColourScript>().SetColour(t.parent.parent.GetComponent<ObjectBaseScript>().CurrentColour);
                    }
                }
            }
        }
        if (OnAssemblyLine)
        {
           ParentObject.transform.position += Vector3.right * Time.deltaTime * .5f;
        }

        
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
