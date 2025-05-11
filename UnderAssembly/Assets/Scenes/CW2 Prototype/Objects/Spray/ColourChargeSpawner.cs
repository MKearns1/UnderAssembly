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
        bool shouldSpawn = true;

        for (int i = ActiveCharges.Count - 1; i >= 0; i--)
        {
            GameObject c = ActiveCharges[i];

            if (c != null)
            {
                if (Vector3.Distance(c.transform.position, SpawnPos.position) < .2f)
                {
                    shouldSpawn = false; // Found one close enough, don't spawn
                    break;
                }
            }
            else
            {
                ActiveCharges.RemoveAt(i); // Clean up null entries
            }
        }

        if (shouldSpawn)
        {
            GameObject NewObject = Instantiate(ColourCharge, SpawnPos.position, SpawnPos.rotation);
            ActiveCharges.Add(NewObject);
            GeneralScript.Instance.ComponentsUsed++;
        }

    }

}
