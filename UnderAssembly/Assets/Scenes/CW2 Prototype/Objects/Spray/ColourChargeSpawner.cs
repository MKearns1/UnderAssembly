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

        bool foundNull = false;

        // Quick scan for any nulls (lightweight)
        foreach (var obj in ActiveCharges)
        {
            if (obj == null || obj.Equals(null))
            {
                foundNull = true;
                break;
            }
        }

        // Only remove nulls if any were found
        if (foundNull)
        {
            ActiveCharges.RemoveAll(x => x == null || x.Equals(null));
        }

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

        }

        if (shouldSpawn)
        {
            GameObject NewObject = Instantiate(ColourCharge, SpawnPos.position, SpawnPos.rotation);
            ActiveCharges.Add(NewObject);
            GeneralScript.Instance.ComponentsUsed++;
        }

    }

}
