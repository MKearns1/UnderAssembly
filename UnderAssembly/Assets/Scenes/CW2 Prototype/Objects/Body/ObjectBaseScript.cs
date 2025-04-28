using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class ObjectBaseScript : MonoBehaviour, IInteractable
{
    public GameObject[] SnapPoints;
    public bool AllSnapsCorrect;
    public GameObject ObjectColourToChange;
    public Color[] PossibleColors;
    public Color DesiredColour;
    public Color CurrentColour;
    public bool CorrectColour;
    bool OnAssemblyLine;
    public int MaterialIndex;
    public List<GameObject> AttachedObjects;

    // Start is called before the first frame update
    void Start()
    {
        float randomNum = Random.Range(0, PossibleColors.Length);
        // randomNum = ((int)randomNum);
        DesiredColour = PossibleColors[(int)(randomNum)];

        GetComponent<Rigidbody>().velocity = Vector3.zero;
       
    }

    // Update is called once per frame
    void Update()
    {
        AllSnapsCorrect = true;
        for (int i = 0; i < SnapPoints.Length; i++)
        {
          //  if (!SnapPoints[i].GetComponent<SnapTriggerScript>().Filled )
            {
               // AllSnapsCorrect = false;
                break;
            }
        }


        if (ColorsAreEqual(CurrentColour, DesiredColour))
        {
            CorrectColour = true;
        }
        else
        {
            CorrectColour = false;

        }
        if (OnAssemblyLine)
        {
            transform.position += Vector3.right * Time.deltaTime * GameObject.Find("Assembly (2)").transform.GetChild(2).GetComponent<AssemblyScript>().AssemblySpeed/20;
        }

       // ObjectColourToChange.GetComponent<MeshRenderer>().materials[MaterialIndex].color = CurrentColour;
       // Debug.Log("snaps " + AllSnapsCorrect);
       // Debug.Log("correctcolour " + CorrectColour);
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
        AttachedObjects.Add(Component);

        StartCoroutine(WaitUntilSettled(Component, AttachPoint));

    }

    public void OnRemoveComponent(GameObject AttachPoint)
    {
        Destroy(AttachPoint.transform.Find("FakeComponent").gameObject);
        GetComponent<Rigidbody>().AddForce(Vector3.left * 0.0001f);
        
       // Physics.SyncTransforms();
    }


    private IEnumerator WaitUntilSettled(GameObject Component, GameObject AttachPoint)
    {
        XRSocketInteractor socket = AttachPoint.GetComponent<XRSocketInteractor>();

        Vector3 lastPosition;
        do
        {
            lastPosition = Component.transform.position;
            yield return null;
        }
        while (Vector3.Distance(lastPosition, Component.transform.position) > 0.001f);

        Debug.Log("Object fully settled at: " + Component.transform.position);
        CreateFakeComponent(Component, AttachPoint);
       
    }

    void CreateFakeComponent(GameObject Component, GameObject socket)
    {
        GameObject FakeComponent = Instantiate(Component, Component.transform.position, Component.transform.rotation);
        FakeComponent.transform.SetParent(socket.transform);
        FakeComponent.GetComponent<Rigidbody>().isKinematic = true;
        Physics.IgnoreCollision(FakeComponent.GetComponent<Collider>(), GetComponent<Collider>());
        Physics.IgnoreCollision(FakeComponent.GetComponent<Collider>(), Component.GetComponent<Collider>());
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

}
