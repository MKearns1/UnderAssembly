using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluatorScript : MonoBehaviour
{
    int CorrectlyAssembled;
    GameObject AssembledProduct;
    ObjectTemplateScript CurrentTemplate;
    ObjectBaseScript AssembledObjectScript;
    //List<GameObject> CorrectSocketPlacement;
    public List<string> CorrectSocketPlacement;

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

        if (GeneralScript.Instance.NewProductTemplate != null)
        {
            CurrentTemplate = GeneralScript.Instance.NewProductTemplate;
            CorrectSocketPlacement = CurrentTemplate.CorrectSocketObject;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if(other.GetComponent<ObjectBaseScript>() != null && AssembledObjectScript == null)
            {
                AssembledProduct = other.gameObject;
                AssembledObjectScript = other.GetComponent<ObjectBaseScript>();

                GeneralScript.Instance.ProductDelivered(EvaluateProduct());

                AssembledObjectScript.DestroySelf();
            }
        }
    }

    bool EvaluateProduct()
    {
        for (int i = 0; i < AssembledObjectScript.Sockets.Count; i++)
        {
            if (AssembledObjectScript.Sockets[i].transform.Find("FakeComponent") != null)
            {
                if(AssembledObjectScript.Sockets[i].transform.Find("FakeComponent").GetComponent<ComponentScript>().ObjectName != CurrentTemplate.CorrectSocketObject[i])
                {
                    Debug.Log("Sockets Wrong");
                    return false;
                }
            }
            else if (CurrentTemplate.CorrectSocketObject[i] != "Empty")
            {
                return false ;
            }
           
        }
        return true;
    }
}
