using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChargeSpawner : MonoBehaviour
{
    public bool Removed = true;
    public GameObject ColourCharge;
    public Transform SpawnPos;
    public List<GameObject> ActiveCharges;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject c in ActiveCharges)
        {
            if (c != null)
            {
                if (Vector3.Distance(c.transform.position, SpawnPos.position) < .2f)
                { Removed = false; }
                else
                {
                    Removed = true;
                }
            }
            else
            {
                ActiveCharges.Remove(c);
            }
        }
        if(Removed)
        {
            GameObject NewCharge = Instantiate(ColourCharge, SpawnPos.position, Quaternion.identity);
            ActiveCharges.Add(NewCharge);
            GeneralScript.Instance.ComponentsUsed++;
            Removed = false;
        }
    }

}
