using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GeneralScript : MonoBehaviour
{
    public static GeneralScript Instance;

    public GameObject CurrentProduct;
    public GameObject Assembly;
    public Transform NewObjectSpawn;
    public List<GameObject> ObjectsToSpawn;
    public List<List<GameObject>> TemplatePrefabs;
    Vector3[] spawnRotations = { new Vector3(285.08551f, 270.003845f, 179.999817f), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
    public ObjectTemplateScript NewProductTemplate;
    public List<ProductType> templates = new List<ProductType>();
    public int ProductsSuccessfullyMade;
    public int ProductsMade;
    public int ErrorsMade;
    public int Quota;
    public bool GameStarted;
    public int ComponentsUsed;
    UnityEngine.UI.Text MonitorProductTextMesh;


    public List<GameObject> PokeInteractors = new List<GameObject>();
    public List<GameObject> DirectInteractors = new List<GameObject>();
    public List<GameObject> RayInteractors = new List<GameObject>();
    public InputActionReference input;
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
        InitialGameStart();
    }

    public void SpawnNewObject()
    {
        if (CurrentProduct == null && GameStarted)
        {



            int randint = Random.Range(0, ObjectsToSpawn.Count);
            // Quaternion rotation = Quaternion.Euler(spawnRotations[randint]);
            CurrentProduct = Instantiate(ObjectsToSpawn[randint], NewObjectSpawn.position, Quaternion.identity);
            int randvariant = Random.Range(0, templates[randint].Variants.Count);
            GameObject DesiredTemplate = Instantiate(templates[randint].Variants[randvariant].Prefab, Vector3.zero, Quaternion.identity);
            DesiredTemplate.GetComponent<ObjectTemplateScript>().Initialize();
            NewProductTemplate = DesiredTemplate.GetComponent<ObjectTemplateScript>();
        }

    }
    private void Update()
    {
        if (ProductsMade >= Quota && GameStarted)
        {
            EndGame();
        }
    }

    private void PressedPrimaryButton(InputAction.CallbackContext context)
    {
        Debug.Log(context.performed);
        // throw new System.NotImplementedException();
    }

    [System.Serializable]
    public class ProductVariants
    {
        public GameObject Prefab;

    }
    [System.Serializable]
    public class ProductType
    {
        public List<ProductVariants> Variants;
    }

    public void StartGame()
    {
        GameStarted = true;

        foreach (GameObject g in RayInteractors)
        {
            g.SetActive(false);
        }
        foreach (GameObject g in DirectInteractors)
        {
            g.SetActive(true);
        }
        foreach (GameObject g in PokeInteractors)
        {
            g.SetActive(true);
        }
        GameObject.Find("InvisibleWalls").transform.GetChild(1).gameObject.SetActive(false);
        GameObject.Find("InvisibleWalls").transform.GetChild(2).gameObject.SetActive(false);
        GameObject.Find("InvisibleWalls").transform.GetChild(3).gameObject.SetActive(false);
        GameObject.Find("InvisibleWalls").transform.GetChild(4).gameObject.SetActive(false);

        SpawnNewObject();
    }

    public void InitialGameStart()
    {
        foreach (GameObject g in RayInteractors)
        {
            g.SetActive(true);
        }
        foreach (GameObject g in DirectInteractors)
        {
            g.SetActive(false);
        }
        foreach (GameObject g in PokeInteractors)
        {
            g.SetActive(false);
        }

        ComponentsUsed -= 12; //num of spawners

    }


    public void EndGame()
    {
        GameObject.Find("Display").GetComponent<TVscript>().EndSession();
        GameStarted = false;
    }

    public string CalculatePerformance()
    {
        float PercentageCorrect = ProductsSuccessfullyMade / Quota * 100;

        if (PercentageCorrect > 90)
        {
            return "EXEMPLARY";
        }
        else if (PercentageCorrect > 75)
        {
            return "EFFICIENT";
        }
        else if (PercentageCorrect > 60)
        {
            return "SATISFACTORY";
        }
        else if (PercentageCorrect > 45)
        {
            return "POOR";
        }
        else if (PercentageCorrect > 25)
        {
            return "LIABILITY";
        }
        else
        {
            return "ABYSMAL";
        }

    }

    public string CalculateCleanliness()
    {
        int NumberOfActiveObjectsLeft = 0;
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

        foreach (GameObject spawner in spawners)
        {
            if (spawner.GetComponent<DrawerScript>() != null)
            {
                NumberOfActiveObjectsLeft += spawner.GetComponent<DrawerScript>().ActiveObjects.Count;
            }
            else if (spawner.GetComponent<ColourChargeSpawner>() != null)
            {
                NumberOfActiveObjectsLeft += spawner.GetComponent<ColourChargeSpawner>().ActiveCharges.Count;
            }
        }

        NumberOfActiveObjectsLeft -= 12;

        if (NumberOfActiveObjectsLeft < 3) 
        {
            return "PRISTINE";
        }
        else if (NumberOfActiveObjectsLeft < 8)
        {
            return "ACCEPTABLE";
        }
        else if (NumberOfActiveObjectsLeft < 15)
        {
            return "MARGINAL";
        }
        else if (NumberOfActiveObjectsLeft < 20)
        {
            return "HAZARDOUS";
        }
        else
        {
            return "DERELICT";
        }
    }
}