using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneralScript : MonoBehaviour
{
    public static GeneralScript Instance;

    public GameObject CurrentProduct;
    public List<Transform> productSnapPoints = new List<Transform>();
    public GameObject Assembly;
    public Transform NewObjectSpawn;
    public List<GameObject> ObjectsToSpawn;
    Vector3[] spawnRotations = { new Vector3(285.08551f, 270.003845f, 179.999817f), new Vector3(0, 0, 0), new Vector3(0, 0, 0) };

    void Awake()
    {
        // Enforce only one instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
            return;
        }

       
    }

    void Start()
    {
        //CurrentProduct = GameObject.Find("RobotBodyPrefab");

        
    }

    public void SpawnNewObject()
    {
       if (CurrentProduct == null)
        {
            int randint = Random.Range(0, ObjectsToSpawn.Count);
            Quaternion rotation = Quaternion.Euler(spawnRotations[randint]);
            CurrentProduct = Instantiate(ObjectsToSpawn[randint], NewObjectSpawn.position, rotation);

            if (CurrentProduct != null && CurrentProduct.transform.childCount > 1)
            {
                foreach (Transform t in CurrentProduct.transform.GetChild(1))
                {
                    productSnapPoints.Add(t);
                }
            }
        }

    }
    private void Update()
    {
        Debug.Log(CurrentProduct);
    }

}
