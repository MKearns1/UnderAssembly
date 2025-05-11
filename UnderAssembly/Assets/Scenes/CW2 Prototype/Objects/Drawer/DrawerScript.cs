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

        bool foundNull = false;

        // Quick scan for any nulls (lightweight)
        foreach (var obj in ActiveObjects)
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
            ActiveObjects.RemoveAll(x => x == null || x.Equals(null));
        }

        bool shouldSpawn = true;

        for (int i = ActiveObjects.Count - 1; i >= 0; i--)
        {
            GameObject c = ActiveObjects[i];


            if (Vector3.Distance(c.transform.position, SpawnPos.position) < RespawnObjectRange)
            {
                shouldSpawn = false;
                break;
            }
        }

        if (shouldSpawn)
        {
            GameObject NewObject = Instantiate(ObjectToSpawn, SpawnPos.position, SpawnPos.rotation);
            ActiveObjects.Add(NewObject);
            GeneralScript.Instance.ComponentsUsed++;
        }

    }


    public static List<GameObject> RemoveNulls(List<GameObject> e)
    {
        e.RemoveAll(x => x == null);
        return e;
    }
}
