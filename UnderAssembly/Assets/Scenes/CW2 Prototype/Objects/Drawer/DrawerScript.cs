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
        foreach (GameObject c in ActiveObjects)
        {
            if (c != null)
            {
                if (Vector3.Distance(c.transform.position, SpawnPos.position) < RespawnObjectRange)
                { Removed = false; }
                else
                {
                    Removed = true;
                }
            }
            else
            {
                ActiveObjects.Remove(c);
            }
        }
        if (Removed)
        {
            GameObject NewObject = Instantiate(ObjectToSpawn, SpawnPos.position, SpawnPos.rotation);
            ActiveObjects.Add(NewObject);
            GeneralScript.Instance.ComponentsUsed++;
            Removed = false;
        }
    }
}
