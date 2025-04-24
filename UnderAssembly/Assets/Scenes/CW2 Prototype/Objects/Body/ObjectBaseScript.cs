using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
}
