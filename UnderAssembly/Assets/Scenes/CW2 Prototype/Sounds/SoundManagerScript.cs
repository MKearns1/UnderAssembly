using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static SoundManagerScript Instance;

    public GameObject SoundObj;

    public AudioClip Ambience;
    public AudioClip ConveyorMove;
    public AudioClip SprayGunSound;
    public AudioClip AttachSound;
    public AudioClip LeverSound;
    public AudioClip ButtonSound;
    public AudioClip TurnPageSound;
    public AudioClip DispenseDyeSound;
    public AudioClip MixPaintSound;


    // Start is called before the first frame update
    void Start()
    {
        PlaySound("Ambience", gameObject, true, 1f);
    }

    private void Awake()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject PlaySound(string sound,GameObject ParentObject, bool loop,float Volume)
    {
        GameObject newSound = null;
        switch (sound)
        {
            case "Ambience":
                newSound = Instantiate(SoundObj, ParentObject.transform);
                newSound.GetComponent<SoundInstance>().audioclip = Ambience;
                newSound.GetComponent<SoundInstance>().loop = loop;
                newSound.GetComponent<SoundInstance>().Volume = Volume;
                break;
            case "ConveyorMove":
                newSound = Instantiate(SoundObj, ParentObject.transform);
                newSound.GetComponent<SoundInstance>().audioclip = ConveyorMove;
                newSound.GetComponent<SoundInstance>().loop = loop;
                newSound.GetComponent<SoundInstance>().Volume = Volume;

                break;
            case "SprayGunSound":
                newSound = Instantiate(SoundObj, ParentObject.transform);
                newSound.GetComponent<SoundInstance>().audioclip = SprayGunSound;
                newSound.GetComponent<SoundInstance>().loop = loop;
                newSound.GetComponent<SoundInstance>().Volume = Volume;

                break;
            case "AttachSound":
                newSound = Instantiate(SoundObj, ParentObject.transform);
                newSound.GetComponent<SoundInstance>().audioclip = AttachSound;
                newSound.GetComponent<SoundInstance>().loop = loop;
                newSound.GetComponent<SoundInstance>().Volume = Volume;

                break;
            case "LeverSound":
                newSound = Instantiate(SoundObj, ParentObject.transform);
                newSound.GetComponent<SoundInstance>().audioclip = LeverSound;
                newSound.GetComponent<SoundInstance>().loop = loop;
                newSound.GetComponent<SoundInstance>().Volume = Volume;

                break;
            case "ButtonSound":
                newSound = Instantiate(SoundObj, ParentObject.transform);
                newSound.GetComponent<SoundInstance>().audioclip = ButtonSound;
                newSound.GetComponent<SoundInstance>().loop = loop;
                newSound.GetComponent<SoundInstance>().Volume = Volume;

                break;
            case "TurnPage":
                newSound = Instantiate(SoundObj, ParentObject.transform);
                newSound.GetComponent<SoundInstance>().audioclip = TurnPageSound;
                newSound.GetComponent<SoundInstance>().loop = loop;
                newSound.GetComponent<SoundInstance>().Volume = Volume;

                break;
            case "DispenseDye":
                newSound = Instantiate(SoundObj, ParentObject.transform);
                newSound.GetComponent<SoundInstance>().audioclip = DispenseDyeSound;
                newSound.GetComponent<SoundInstance>().loop = loop;
                newSound.GetComponent<SoundInstance>().Volume = Volume;

                break;
            case "MixPaint":
                newSound = Instantiate(SoundObj, ParentObject.transform);
                newSound.GetComponent<SoundInstance>().audioclip = MixPaintSound;
                newSound.GetComponent<SoundInstance>().loop = loop;
                newSound.GetComponent<SoundInstance>().Volume = Volume;

                break;
        }

        return newSound;
    }
}
