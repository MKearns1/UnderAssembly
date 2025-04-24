using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * TODO:
 * - replace update colour with callback code for efficiency and cleaner code
 * - add further documentation onto code
 * - add support for toggling attributes
 */

[AddComponentMenu("XRLab/XR Editor/Menu Material Manager")]
public class ObjectMaterialManager : MonoBehaviour
{
    #region Declarations
    #region Game Object References
    [Header("Text References")]
    [Tooltip("The Text reference for the label that shows which material index is currently selected")]
    [SerializeField] private Text selectedMaterialReadout;
    [Tooltip("The Text reference to the readout indicating the current colour of the material")]
    [SerializeField] private Text materialColourReadout;

    [Header("Slider References")]
    [Header("Colour")]
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider redSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider greenSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider blueSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider alphaSlider;

    [Header("Emissive Colour")]
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider emissiveRedSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider emissiveGreenSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider emissiveBlueSlider;

    [Header("Offset Settings")]
    [Tooltip("Slider for the texture offset on the X axis")]
    [SerializeField] private Slider offsetX;
    [Tooltip("Slider for the texture offset on the Y axis")]
    [SerializeField] private Slider offsetY;

    [Header("Additional Settings")]
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider metallicSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider specularSlider;
    #endregion

    public GameObject m_selectedGameObject; //the current game object that is selected by the user

    private int m_selectedMaterialIndex = 0; //the index for the material the user selects
    public Material m_selectedMat; //the material of the currently selected object

    private Color m_targetColour; //holder for the colour that the user will apply when modifying the base colour
    private Vector3 m_targetEmissiveColour; //the emissive colour holder 
    private bool m_loadingFromSelectedObject = false; //used to prevent unintentional glitches when loading in material data from newly selected objects
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        if (materialColourReadout == null)
            Debug.LogWarning("WARNING: Material text readout is unassigned, please assign it to a Text object to avoid instability");
    }

    /// <summary>
    /// sets the selected game object to the object passed to it. 
    /// </summary>
    /// <param name="obj">The new selected object</param>
    /// <param name="updateUI">If true, will update UI immediately after setting new selected object</param>
    public void SetSelectedObject(GameObject obj, bool updateUI)
    {
        m_selectedMaterialIndex = 0;
        m_selectedGameObject = obj;

        m_loadingFromSelectedObject = true;

        if (updateUI)
            UpdateUI();
    }

    /// <summary>
    /// updates the UI with all available information from the selected material
    /// </summary>
    private void UpdateUI()
    {
        if (m_selectedGameObject == null)
            return;
        if (m_selectedGameObject.GetComponent<SkinnedMeshRenderer>() != null)
        {
            m_selectedMat = m_selectedGameObject.GetComponent<SkinnedMeshRenderer>().materials[m_selectedMaterialIndex]; Debug.Log(m_selectedGameObject);

        }
        else
        {
            m_selectedMat = m_selectedGameObject.GetComponent<MeshRenderer>().materials[m_selectedMaterialIndex];
        }
        if (m_selectedMat.shader.name == "Universal Render Pipeline/Lit")
        {
            selectedMaterialReadout.text = ("Material " + (m_selectedMaterialIndex + 1) + "/" + m_selectedGameObject.GetComponent<MeshRenderer>().materials.Length);
            materialColourReadout.text = ("Colour: " + m_selectedMat.GetColor("_BaseColor"));

            if (m_loadingFromSelectedObject)
            {
                redSlider.value = m_selectedMat.GetColor("_BaseColor").r;
                greenSlider.value = m_selectedMat.GetColor("_BaseColor").g;
                blueSlider.value = m_selectedMat.GetColor("_BaseColor").b;
                alphaSlider.value = m_selectedMat.GetColor("_BaseColor").a;

                emissiveRedSlider.value = m_selectedMat.GetColor("_EmissionColor").r;
                emissiveGreenSlider.value = m_selectedMat.GetColor("_EmissionColor").g;
                emissiveBlueSlider.value = m_selectedMat.GetColor("_EmissionColor").b;

                metallicSlider.value = m_selectedMat.GetFloat("_Metallic");
                specularSlider.value = m_selectedMat.GetFloat("_Smoothness");

                offsetX.value = m_selectedMat.mainTextureOffset.x;
                offsetY.value = m_selectedMat.mainTextureOffset.y;

                m_loadingFromSelectedObject = false;
            }
        }
        else
        {
            Debug.Log("Warning: Shader " + m_selectedMat.shader.name + " is not supported, menu will not function correctly");
        }
    }

    /// <summary>
    /// Iterates through the array of materials on an object and sets the current index to the selected material
    /// </summary>
    /// <param name="nextMat">Makes the selection forward or backward</param>
    public void ChangeSelectedMaterial(bool nextMat)
    {
        int materialsLength;

        if (m_selectedGameObject.GetComponent<SkinnedMeshRenderer>() != null)
        {
            materialsLength = m_selectedGameObject.GetComponent<SkinnedMeshRenderer>().materials.Length;

        }
        else
        {
            materialsLength = m_selectedGameObject.GetComponent<MeshRenderer>().materials.Length;
        }


        if (nextMat)
        {
            if (m_selectedMaterialIndex < materialsLength - 1)
            {
                m_selectedMaterialIndex++;
            }
            else
            {
                m_selectedMaterialIndex = 0;
            }
        }
        else
        {
            if (m_selectedMaterialIndex > 0)
            {
                m_selectedMaterialIndex--;
            }
            else
            {
                m_selectedMaterialIndex = materialsLength - 1;
            }
        }

        m_loadingFromSelectedObject = true;
        UpdateUI();
    }

    /// <summary>
    /// take the colour values from the target colour and apply them to the target object
    /// </summary>
    public void UpdateColourValues()
    {
        if (m_loadingFromSelectedObject || m_selectedMat == null)
            return;

        m_targetColour.r = redSlider.value;
        m_targetColour.g = greenSlider.value;
        m_targetColour.b = blueSlider.value;
        m_targetColour.a = alphaSlider.value;

        m_selectedMat.SetColor("_BaseColor", m_targetColour);

        m_targetEmissiveColour.x = emissiveRedSlider.value;
        m_targetEmissiveColour.y = emissiveGreenSlider.value;
        m_targetEmissiveColour.z = emissiveBlueSlider.value;
        m_selectedMat.SetVector("_EmissionColor", m_targetEmissiveColour);
        m_selectedMat.EnableKeyword("_EMISSION");

        m_selectedMat.SetFloat("_Metallic", metallicSlider.value);
        m_selectedMat.SetFloat("_Smoothness", specularSlider.value);

        m_selectedMat.mainTextureOffset = new Vector2(offsetX.value, offsetY.value);

        UpdateUI();
    }

    private void OnEnable()
    {
        redSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        greenSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        blueSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        alphaSlider.onValueChanged.AddListener((value) => UpdateColourValues());

        emissiveRedSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        emissiveGreenSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        emissiveBlueSlider.onValueChanged.AddListener((value) => UpdateColourValues());

        offsetX.onValueChanged.AddListener((value) => UpdateColourValues());
        offsetY.onValueChanged.AddListener((value) => UpdateColourValues());

        metallicSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        specularSlider.onValueChanged.AddListener((value) => UpdateColourValues());
    }

    private void Update()
    {
        UpdateColourValues();
      
    }
}
