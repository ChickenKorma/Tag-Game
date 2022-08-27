using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    private Camera cam;

    private Slider healthbar;

    private BaseController character;

    private Transform characterTransform;

    public Transform CharacterTransform { set { characterTransform = value; } }

    
    void Start()
    {
        cam = Camera.main;

        character = characterTransform.GetComponent<BaseController>();

        healthbar = GetComponent<Slider>();
        healthbar.maxValue = character.MaxHealth;
    }

    void Update()
    {
        if (characterTransform)
        {
            healthbar.value = character.Health;

            Vector3 screenPosition = cam.WorldToScreenPoint(characterTransform.position);
            screenPosition.y += 20;

            healthbar.GetComponent<RectTransform>().position = screenPosition;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
