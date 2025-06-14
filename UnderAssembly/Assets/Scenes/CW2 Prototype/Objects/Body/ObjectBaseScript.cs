using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class ObjectBaseScript : MonoBehaviour, IInteractable
{
    public GameObject ObjectColourToChange;
    GameObject Triggers;
    public Color[] PossibleColors;
    public Color DesiredColour;
    public Color CurrentColour;
    public bool CorrectColour;
    bool OnAssemblyLine;
    public int MaterialIndex;
    public Dictionary<string ,GameObject> AttachedObjects = new Dictionary<string, GameObject>();   // Only use for this is to just re enable collision for component after removal.
    public List<GameObject> Sockets;

    // Start is called before the first frame update
    void Start()
    {
        float randomNum = Random.Range(0, PossibleColors.Length);
        // randomNum = ((int)randomNum);
        DesiredColour = PossibleColors[(int)(randomNum)];
        Triggers = transform.Find("Triggers").gameObject;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        for (int i = 0; i < Triggers.transform.childCount; i++)
        {
            Sockets.Add(Triggers.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CorrectColour = (ColorsAreEqual(CurrentColour, DesiredColour));
   
        if (OnAssemblyLine && GameObject.Find("Assembly (2)").transform.Find("Trigger").GetComponent<AssemblyScript>().ON)
        {
            transform.position += Vector3.right * Time.deltaTime * GameObject.Find("Assembly (2)").transform.Find("Trigger").GetComponent<AssemblyScript>().AssemblySpeed/20;
        }

         ObjectColourToChange.GetComponent<MeshRenderer>().materials[MaterialIndex].color = CurrentColour;
    }

    bool ColorsAreEqual(Color a, Color b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance &&
               Mathf.Abs(a.a - b.a) < tolerance;
    }



    public void SetOnAssembly(bool state)
    {
        OnAssemblyLine = state;
    }

    public void OnAddComponent(GameObject AttachPoint)
    {
        XRSocketInteractor socket = AttachPoint.GetComponent<XRSocketInteractor>();
        GameObject Component = socket.selectTarget.gameObject;
        AttachedObjects.Add(AttachPoint.gameObject.name,Component);
        StartCoroutine(WaitUntilSettled(Component, AttachPoint));
        SoundManagerScript.Instance.PlaySound("AttachSound", gameObject, false, .75f);
        Component.GetComponent<ComponentScript>().baseObject = this;
    }

    public void OnRemoveComponent(GameObject AttachPoint)
    {
        XRSocketInteractor socket = AttachPoint.GetComponent<XRSocketInteractor>();
        GameObject RemovedComponent;
        AttachedObjects.TryGetValue(AttachPoint.gameObject.name, out RemovedComponent);

        foreach (Transform child in AttachPoint.transform)
        {
            if(child.name == "FakeComponent")
                Destroy(child.gameObject);

        }


        foreach (Transform t in transform.Find("Triggers"))
        {
            if (t.Find("FakeComponent") != null)
            {
                Collider CurrentFakeColliders = t.Find("FakeComponent").GetComponent<Collider>();
                Physics.IgnoreCollision(CurrentFakeColliders, RemovedComponent.GetComponent<Collider>(),false);
                Physics.IgnoreCollision(RemovedComponent.GetComponent<Collider>(), CurrentFakeColliders,false);

            }
        }


        Physics.IgnoreCollision(GetComponent<Collider>(), RemovedComponent.GetComponent<Collider>(), false);
        Physics.IgnoreCollision(RemovedComponent.GetComponent<Collider>(), GetComponent<Collider>(), false);
        AttachedObjects.Remove(AttachPoint.name);

        GetComponent<Rigidbody>().AddForce(Vector3.left * 0.0001f);

        RemovedComponent.GetComponent<ComponentScript>().baseObject = null;

        // Physics.SyncTransforms();
    }


    private IEnumerator WaitUntilSettled(GameObject Component, GameObject AttachPoint)
    {
        XRSocketInteractor socket = AttachPoint.GetComponent<XRSocketInteractor>();

        Vector3 lastPosition;
        Quaternion lastRotation;
        do
        {
            lastPosition = Component.transform.position;
            lastRotation = Component.transform.rotation;
            yield return null;
        }
        while (Vector3.Distance(lastPosition, Component.transform.position) > 0.001f || Quaternion.Angle(lastRotation, Component.transform.rotation) > 0.1f);

        yield return new WaitForSeconds(0.05f);
        Debug.Log("Object fully settled at: " + Component.transform.position);

        if (socket.selectTarget != null)
        CreateFakeComponent(Component, AttachPoint);
       
    }

    void CreateFakeComponent(GameObject Component, GameObject socket)
    {
        if (socket.transform.Find("FakeComponent") != null)
            Destroy(socket.transform.Find("FakeComponent").gameObject);

        GameObject FakeComponent = Instantiate(Component, Component.transform.position, Component.transform.rotation);
        FakeComponent.transform.SetParent(socket.transform);

        foreach (Collider c in GetComponents<Collider>())
        {
            Physics.IgnoreCollision(c, Component.GetComponent<Collider>());
            Physics.IgnoreCollision(Component.GetComponent<Collider>(), c);
        }

        //foreach (KeyValuePair<string, GameObject> entry in AttachedObjects)
        //{
        //    //if (entry.Value.transform.Find("FakeComponent") != null)
        //    {
        //        Collider AttachedObjCollider;
        //        AttachedObjCollider = entry.Value.GetComponent<Collider>();

        //        Physics.IgnoreCollision(AttachedObjCollider, Component.GetComponent<Collider>());
        //        Physics.IgnoreCollision(Component.GetComponent<Collider>(), AttachedObjCollider);
        //    }
        //}

        foreach (Transform t in transform.Find("Triggers"))
        {
            if (t.Find("FakeComponent") != null)
            {
                Collider CurrentFakeColliders = t.Find("FakeComponent").GetComponent<Collider>();
                Physics.IgnoreCollision(CurrentFakeColliders, Component.GetComponent<Collider>());
                Physics.IgnoreCollision(Component.GetComponent<Collider>(), CurrentFakeColliders);

                foreach (KeyValuePair<string, GameObject> entry in AttachedObjects)
                {

                    Collider CurrentRealAttachedColliders;
                    CurrentRealAttachedColliders = entry.Value.GetComponent<Collider>();

                    Physics.IgnoreCollision(CurrentRealAttachedColliders, FakeComponent.GetComponent<Collider>());
                    Physics.IgnoreCollision(FakeComponent.GetComponent<Collider>(), CurrentRealAttachedColliders);

                }

            }
        }

        Physics.IgnoreCollision(FakeComponent.GetComponent<Collider>(), Component.GetComponent<Collider>());
        Physics.IgnoreCollision(Component.GetComponent<Collider>(), FakeComponent.GetComponent<Collider>());

        
        //Destroy(Component.gameObject);
        Destroy(FakeComponent.GetComponent<XRGrabInteractable>());
        Destroy(FakeComponent.GetComponent<XRGeneralGrabTransformer>());
        Destroy(FakeComponent.GetComponent<Rigidbody>());
        FakeComponent.gameObject.name = "FakeComponent";
    }
    void CopyCollider(GameObject source, GameObject target)
    {
        // Check if the source object has a collider
        Collider sourceCollider = source.GetComponent<Collider>();

        if (sourceCollider != null)
        {
            // Check if the target object already has a collider
            Collider targetCollider = target.GetComponent<Collider>();

            // If the target object doesn't have a collider, add the same type as the source
            if (targetCollider == null)
            {
                if (sourceCollider is BoxCollider)
                    targetCollider = target.AddComponent<BoxCollider>();
                else if (sourceCollider is SphereCollider)
                    targetCollider = target.AddComponent<SphereCollider>();
                else if (sourceCollider is CapsuleCollider)
                    targetCollider = target.AddComponent<CapsuleCollider>();
                else if (sourceCollider is MeshCollider)
                    targetCollider = target.AddComponent<MeshCollider>();
                else
                {
                    Debug.LogWarning("Collider type not supported for copying.");
                    return;
                }
            }

            // Calculate scaling ratio
            Vector3 sourceScale = source.transform.lossyScale;
            Vector3 targetScale = target.transform.lossyScale;
            Vector3 scaleRatio = new Vector3(
                sourceScale.x / targetScale.x,
                sourceScale.y / targetScale.y,
                sourceScale.z / targetScale.z
            );

            // Copy base collider properties
            targetCollider.isTrigger = sourceCollider.isTrigger;
            targetCollider.material = sourceCollider.material;

            // Copy specific collider properties based on type
            if (sourceCollider is BoxCollider sourceBox && targetCollider is BoxCollider targetBox)
            {
                targetBox.center = sourceBox.center;
                targetBox.size = Vector3.Scale(sourceBox.size, scaleRatio);
            }
            else if (sourceCollider is SphereCollider sourceSphere && targetCollider is SphereCollider targetSphere)
            {
                targetSphere.center = sourceSphere.center;
                float avgScale = (scaleRatio.x + scaleRatio.y + scaleRatio.z) / 3f;
                targetSphere.radius = sourceSphere.radius * avgScale;
            }
            else if (sourceCollider is CapsuleCollider sourceCapsule && targetCollider is CapsuleCollider targetCapsule)
            {
                targetCapsule.center = sourceCapsule.center;
                float avgScale = (scaleRatio.x + scaleRatio.y + scaleRatio.z) / 3f;
                targetCapsule.radius = sourceCapsule.radius * avgScale;
                targetCapsule.height = sourceCapsule.height * avgScale;
                targetCapsule.direction = sourceCapsule.direction;
            }
            else if (sourceCollider is MeshCollider sourceMesh && targetCollider is MeshCollider targetMesh)
            {
                targetMesh.sharedMesh = sourceMesh.sharedMesh;
                targetMesh.convex = sourceMesh.convex;
                // isTrigger and material already copied above
            }
        }
        else
        {
            Debug.LogError("Source object does not have a collider.");
        }
    }


    public void DestroySelf()
    {
        var objectsToDestroy = new List<GameObject>(AttachedObjects.Values);
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        Destroy(gameObject);
    }
}
