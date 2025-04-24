using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayChargeScript : MonoBehaviour
{
    GameObject SprayGun;
    SprayGunScript gunScript;
    GameObject Fluid;
    Transform AttatchPoint;
    public Color colour;

    // Start is called before the first frame update
    void Start()
    {
        SprayGun = GameObject.FindGameObjectWithTag("SprayGun");
        gunScript = SprayGun.transform.Find("TriggerArea").GetComponent<SprayGunScript>();
        Fluid = transform.GetChild(0).gameObject;
        Fluid.GetComponent<Renderer>().material.color = colour;

        AttatchPoint = transform.Find("AttachPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(AttatchPoint.position, gunScript.ChargeInsertionPoint.transform.position) < 0.05)
        {
            gunScript.Refill(colour);
            Destroy(gameObject);
        };
        
    }
}
