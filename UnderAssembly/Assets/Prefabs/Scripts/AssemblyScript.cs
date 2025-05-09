using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyScript : MonoBehaviour, IInteractable
{
    bool onLine;
    GameObject item;
    public bool ON;
    public float AssemblySpeed;

    // Start is called before the first frame update
    void Start()
    {
        ON = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (onLine && ON)
        {
            if (item != null)
            {
                if (item.GetComponent<Rigidbody>() != null)
                {
                    //item.GetComponent<Rigidbody>().AddForce(1,0,0);
                    //item.GetComponent<Rigidbody>().velocity = Vector3.right * 0.5f;
                }
            }
        }

        //  GetComponent<Rigidbody>().velocity = new Vector3(AssemblySpeed,0,0) * Time.deltaTime;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (ON)
        {
            // onLine = true;
            item = other.gameObject;
            // Debug.Log(other.name);

            if (other.GetComponent<IInteractable>() != null)
                other.GetComponent<IInteractable>().SetOnAssembly(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
            other.GetComponent<IInteractable>().SetOnAssembly(false);

    }
    public void Switch()
    {
        ON = !ON;
    }
  
    public void SetOnAssembly(bool state )
    {

    }

 

}
