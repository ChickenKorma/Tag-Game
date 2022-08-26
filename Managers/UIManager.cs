using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Canvas gameEnd;
    [SerializeField] private Canvas gameplay;

    [SerializeField] private TextMeshProUGUI winText;

    [SerializeField] private GameObject healthbarPrefab;

    private void OnEnable()
    {
        GameManager.gameEndEvent += GameEnd;
    }

    private void OnDisable()
    {
        GameManager.gameEndEvent -= GameEnd;
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameEnd.enabled = false;

        GenerateHealthbars();
    }

    private void GenerateHealthbars()
    {
        foreach(Transform character in GameManager.Instance.RemainingCharacters)
        {
            Healthbar healthbar = Instantiate(healthbarPrefab, gameplay.transform).transform.GetComponent<Healthbar>();

            healthbar.CharacterTransform = character;
        }
    }

    private void GameEnd(Transform winner)
    {
        gameEnd.enabled = true;

        winText.text = "Winner: " + winner.name;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
