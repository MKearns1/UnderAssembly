using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TricycleScript : MonoBehaviour
{
    public GameObject[] SnapPoints;
    public bool AllSnapsCorrect;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < SnapPoints.Length; i++)
        {
            AllSnapsCorrect = true;
            if (SnapPoints[i].GetComponent<SnapTriggerScript>().Filled)
            {
                continue;
            }
            else { AllSnapsCorrect = false; break; }
            
        }
    }
}
