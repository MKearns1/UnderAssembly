using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapTriggerScript : MonoBehaviour
{
    public GameObject SnapTriggerPrefab;
    public Transform PlacementPosition;
    public string RequiredInputObject;
    public bool Filled;
    public GameObject AttachedObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && false)
        {
            if (!Filled)
            {
                if (other.GetComponent<SnapInputScript>() != null)
                {
                    if (other.GetComponent<SnapInputScript>().InputObject == RequiredInputObject)
                    {

                        SnapObject(other.gameObject);
                    }
                }
            }
        }
    }



    public void SnapObject(GameObject obj)
    {
        
        AttachedObject = obj;
        obj.GetComponent<SnapInputScript>().ParentObject.GetComponent<XRGrabInteractable>().enabled = false;
        //Physics.IgnoreCollision(obj.GetComponent<Collider>(), GetComponent<Collider>());
        //Physics.IgnoreCollision(obj.GetComponent<SnapInputScript>().ParentObject.GetComponent<Collider>(), GetComponent<Collider>());
        //Physics.IgnoreCollision(obj.GetComponent<SnapInputScript>().ParentObject.GetComponent<Collider>(), obj.GetComponent<Collider>());

        Transform originalParent = obj.transform.parent;
        Vector3 worldPos = obj.GetComponent<SnapInputScript>().ParentObject.transform.position;
        Quaternion worldRot = obj.GetComponent<SnapInputScript>().ParentObject.transform.rotation;
        Vector3 worldScale = obj.GetComponent<SnapInputScript>().ParentObject.transform.lossyScale;

        obj.GetComponent<SnapInputScript>().ParentObject.transform.SetParent(PlacementPosition, false);
        obj.GetComponent<SnapInputScript>().ParentObject.transform.position = PlacementPosition.position;
        obj.GetComponent<SnapInputScript>().ParentObject.transform.rotation = PlacementPosition.rotation;
        obj.GetComponent<SnapInputScript>().ParentObject.transform.localScale = worldScale; // maintains world scale

        Destroy(obj.GetComponent<SnapInputScript>().ParentObject.GetComponent<XRGrabInteractable>());
        Destroy(obj.GetComponent<SnapInputScript>().ParentObject.GetComponent<Rigidbody>());

        Filled = true;
       // obj.GetComponent<SnapInputScript>().ParentObject.transform.SetParent(PlacementPosition, true);
       // obj.GetComponent<SnapInputScript>().ParentObject.transform.position = PlacementPosition.position;
       // obj.GetComponent<SnapInputScript>().ParentObject.transform.localScale = Vector3.one;
       // //obj.p.transform.SetParent(PlacementPosition, false);
       //// obj.transform.position = PlacementPosition.position;
    }
}
