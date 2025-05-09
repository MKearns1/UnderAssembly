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
    public List<Transform> productSnapPoints = new List<Transform>();
    public GameObject Assembly;
    public Transform NewObjectSpawn;
    public List<GameObject> ObjectsToSpawn;
    public List<List<GameObject>> TemplatePrefabs;
    Vector3[] spawnRotations = { new Vector3(285.08551f, 270.003845f, 179.999817f), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
    public ObjectTemplateScript NewProductTemplate;
    public List<ProductType> templates = new List<ProductType>();
    UnityEngine.UI.Text MonitorProductTextMesh;

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
        //GameObject.Find("EvaluatorTrigger").GetComponent<EvaluatorScript>();
        SpawnNewObject();
        MonitorProductTextMesh = GameObject.Find("DisplayTitleText").GetComponent<UnityEngine.UI.Text>();
    }

    public void SpawnNewObject()
    {
        if (CurrentProduct == null)
        {



            int randint = Random.Range(0, ObjectsToSpawn.Count);
            // Quaternion rotation = Quaternion.Euler(spawnRotations[randint]);
            CurrentProduct = Instantiate(ObjectsToSpawn[randint], NewObjectSpawn.position, Quaternion.identity);
            int randvariant = Random.Range(0, templates[randint].Variants.Count);
            GameObject DesiredTemplate = Instantiate(templates[randint].Variants[randvariant].Prefab, Vector3.zero, Quaternion.identity);
            NewProductTemplate = DesiredTemplate.GetComponent<ObjectTemplateScript>();
        }
    }
    private void Update()
    {

        for (int i = 0; i < productSnapPoints.Count; i++)
        {
            if (productSnapPoints[i] == null)
            {
                productSnapPoints.RemoveAt(i);
            }
        }

        MonitorProductTextMesh.text = "Directive: " + NewProductTemplate.TemplateTitle;

        // input.ToInputAction().performed += PressedPrimaryButton;
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
}
