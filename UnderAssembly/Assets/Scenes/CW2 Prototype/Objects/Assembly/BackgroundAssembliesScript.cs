using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BackgroundAssembliesScript : MonoBehaviour
{
    Transform StartPoint;
    Transform EndPoint;
    public List<GameObject> Objects;
    public float speed;
    GameObject CurrentObj;
    bool spawnedObj;

    // Start is called before the first frame update
    void Start()
    {
        StartPoint = transform.GetChild(0);
        EndPoint = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentObj == null && !spawnedObj)
        {
            spawnedObj = true;
            float randseconds = Random.Range(0, 5);
            Invoke("SpawnNewObject", randseconds);
        }

        if (CurrentObj != null)
        {
            if (Vector3.Distance(CurrentObj.transform.position, EndPoint.transform.position) < 1)
            {
                Destroy(CurrentObj);
                spawnedObj=false;
            }
            else
            {
                Vector3 Direction = EndPoint.transform.position - CurrentObj.transform.position;
                CurrentObj.transform.Translate(Direction.normalized * Time.deltaTime * speed);
                CurrentObj.transform.position = new Vector3(CurrentObj.transform.position.x, CurrentObj.transform.position.y, EndPoint.transform.position.z);
                CurrentObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    void SpawnNewObject()
    {
        int randint = Random.Range(0, Objects.Count);
        GameObject NewObject = Instantiate(Objects[randint], StartPoint.position, Quaternion.identity);
        CurrentObj = NewObject;
    }
}
