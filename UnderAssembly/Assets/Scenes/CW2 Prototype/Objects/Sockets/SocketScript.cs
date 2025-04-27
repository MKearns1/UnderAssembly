using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketScript : MonoBehaviour
{
    XRSocketInteractor idf;

    // Start is called before the first frame update
    void Start()
    {
        idf = GetComponent<XRSocketInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        
        ////idf.interactablesSelected;
        //foreach(XRSocketInteractor x in idf.interactablesSelected)
        //{
        //    Debug.Log(x.gameObject);

        //}
        Debug.Log(idf.selectTarget);
    }
}
