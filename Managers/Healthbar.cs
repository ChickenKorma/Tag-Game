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

    private GameObject background;
    private GameObject fill;
    
    void Start()
    {
        cam = Camera.main;

        character = characterTransform.GetComponent<BaseController>();

        healthbar = GetComponent<Slider>();
        healthbar.maxValue = character.MaxHealth;

        background = transform.GetChild(0).gameObject;
        fill = transform.GetChild(1).gameObject;
    }

    void Update()
    {
        if (characterTransform)
        {
            Visibility(GameManager.Instance.VisibleCharacters.Contains(characterTransform));

            healthbar.value = character.Health;

            Vector3 screenPosition = cam.WorldToScreenPoint(characterTransform.position);
            screenPosition.y += 39;

            healthbar.GetComponent<RectTransform>().position = screenPosition;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Hides the healthbar visuals depending on input bool, if the character is not the player
    private void Visibility(bool visible)
    {
        if(character.GetType() != typeof(PlayerController))
        {
            background.SetActive(visible);
            fill.SetActive(visible);
        }
    }
}
