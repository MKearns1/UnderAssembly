using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

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
            if (!SnapPoints[i].GetComponent<SnapTriggerScript>().Filled)
            {
                AllSnapsCorrect = false;
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

        ObjectColourToChange.GetComponent<MeshRenderer>().materials[MaterialIndex].color = CurrentColour;
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
        GameObject NewCollision = AttachPoint.transform.Find("Collision").gameObject;
        Debug.Log(socket.selectTarget);

        AttachPoint.transform.Find("Collision").GetComponent<Collider>().enabled = true;
        // Component.GetComponent<Collider>().excludeLayers = NewCollision.layer;
        //NewCollision.GetComponent<Collider>().excludeLayers = Component.layer;
        Physics.IgnoreCollision(NewCollision.GetComponent<Collider>(), Component.GetComponent<Collider>());

        CopyCollider(Component, NewCollision);
       // Component.GetComponent<Collider>().enabled = false;
       // Physics.IgnoreCollision()
    }

    public void OnRemoveComponent(GameObject AttachPoint)
    {
        AttachPoint.transform.Find("Collision").GetComponent<Collider>().enabled=false;
        GetComponent<Rigidbody>().AddForce(Vector3.left * 0.0001f);
       // Physics.SyncTransforms();
    }

    void CopyCollider(GameObject source, GameObject target)
    {
        // Check if the source object has a collider
        Collider sourceCollider = source.GetComponent<Collider>();

        if (sourceCollider != null)
        {
            // Check if the target object already has a collider
            Collider targetCollider = target.GetComponent<Collider>();

            // If the target object doesn't have a collider, add the same type of collider as the source
            if (targetCollider == null)
            {
                // You can add different types of colliders based on the source collider's type
                if (sourceCollider is BoxCollider)
                {
                    targetCollider = target.AddComponent<BoxCollider>();
                }
                else if (sourceCollider is SphereCollider)
                {
                    targetCollider = target.AddComponent<SphereCollider>();
                }
                else if (sourceCollider is CapsuleCollider)
                {
                    targetCollider = target.AddComponent<CapsuleCollider>();
                }
                else if (sourceCollider is MeshCollider)
                {
                    targetCollider = target.AddComponent<MeshCollider>();
                }
            }

            // Copy properties from the source collider to the target collider
            // BoxCollider Example
            if (sourceCollider is BoxCollider sourceBox && targetCollider is BoxCollider targetBox)
            {
                targetBox.center = sourceBox.center;
                targetBox.size = sourceBox.size;
            }
            // SphereCollider Example
            else if (sourceCollider is SphereCollider sourceSphere && targetCollider is SphereCollider targetSphere)
            {
                targetSphere.center = sourceSphere.center;
                targetSphere.radius = sourceSphere.radius;
            }
            // CapsuleCollider Example
            else if (sourceCollider is CapsuleCollider sourceCapsule && targetCollider is CapsuleCollider targetCapsule)
            {
                targetCapsule.center = sourceCapsule.center;
                targetCapsule.radius = sourceCapsule.radius;
                targetCapsule.height = sourceCapsule.height;
                targetCapsule.direction = sourceCapsule.direction;
            }
            // MeshCollider Example
            else if (sourceCollider is MeshCollider sourceMesh && targetCollider is MeshCollider targetMesh)
            {
                targetMesh.sharedMesh = sourceMesh.sharedMesh;
                targetMesh.convex = sourceMesh.convex;
                targetMesh.isTrigger = sourceMesh.isTrigger;
            }
        }
        else
        {
            Debug.LogError("Source object does not have a collider.");
        }
    }
}
