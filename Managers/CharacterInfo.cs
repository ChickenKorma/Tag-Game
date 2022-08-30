using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfo : MonoBehaviour
{
    private Camera cam;

    private RectTransform rectTransform;


    [Header("Character")]
    private BaseController characterController;

    private Transform characterTransform;

    public Transform CharacterTransform { set { characterTransform = value; } }


    [Header("Components")]
    private Slider healthbarSlider;
    

    private void Awake()
    {
        cam = Camera.main;

        rectTransform = GetComponent<RectTransform>();

        healthbarSlider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        characterController = characterTransform.GetComponent<BaseController>();

        healthbarSlider.maxValue = characterController.StartingHealth;       

        string name = characterTransform.name;

        if (!name.Equals("Player"))
        {
            GetComponentInChildren<TMP_Text>().text = name;
        }
    }

    void Update()
    {
        // Updates position and slider values, checks if character is dead
        if (characterTransform)
        {
            healthbarSlider.value = characterController.Health;

            Vector3 screenPosition = cam.WorldToScreenPoint(characterTransform.position);

            rectTransform.position = screenPosition;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}