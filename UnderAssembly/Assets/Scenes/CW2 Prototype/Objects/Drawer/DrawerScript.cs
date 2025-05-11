using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

public class DrawerScript : MonoBehaviour
{
    public bool Removed = true;
    public GameObject ObjectToSpawn;
    public Transform SpawnPos;
    public List<GameObject> ActiveObjects;
    public float RespawnObjectRange = 0.3f;

    Vector3 startpos;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ClampedPos = transform.position;

        ClampedPos.z = Mathf.Clamp(transform.position.z,startpos.z-.0122f,startpos.z+0.2132f);
        ClampedPos.x = Mathf.Clamp(transform.position.x,startpos.x-.0122f,startpos.x+0.2132f);
        transform.position = ClampedPos;

        bool shouldSpawn = true;

        for (int i = ActiveObjects.Count - 1; i >= 0; i--)
        {
            GameObject c = ActiveObjects[i];

            if (c != null)
            {
                if (Vector3.Distance(c.transform.position, SpawnPos.position) < RespawnObjectRange)
                {
                    shouldSpawn = false; // Found one close enough, don't spawn
                    break;
                }
            }
            else
            {
                ActiveObjects.RemoveAt(i); // Clean up null entries
            }
        }

        if (shouldSpawn)
        {
            GameObject NewObject = Instantiate(ObjectToSpawn, SpawnPos.position, SpawnPos.rotation);
            ActiveObjects.Add(NewObject);
            GeneralScript.Instance.ComponentsUsed++;
        }


    }
}
