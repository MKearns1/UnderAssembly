using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluatorScript : MonoBehaviour
{
    int CorrectlyAssembled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CorrectlyAssembled != 0)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if(other.GetComponent<ObjectBaseScript>() != null)
            {

                if(other.GetComponent<ObjectBaseScript>().AllSnapsCorrect && other.GetComponent<ObjectBaseScript>().CorrectColour)
                {
                    CorrectlyAssembled++;
                }

                Destroy(other.gameObject);
            }
        }
    }
}
