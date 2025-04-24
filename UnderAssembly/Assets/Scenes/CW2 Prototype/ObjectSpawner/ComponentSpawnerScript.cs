using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSpawnerScript : MonoBehaviour
{
    public Transform SpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPos = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnObject(GameObject NewObject)
    {       
            Instantiate(NewObject, SpawnPos.position, Quaternion.identity);        
    }
}
