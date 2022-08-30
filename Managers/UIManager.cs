using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject characterInfoPrefab;

    [SerializeField] private Transform gameUI;
    [SerializeField] private GameObject menuUI;

    [SerializeField] private TMP_Text winText;

    private AudioSource buttonPress;

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

        buttonPress = GetComponent<AudioSource>();

        winText = menuUI.GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        menuUI.SetActive(false);

        GenerateCharacterInfo();
    }

    // Instantiates and sets up a character info prefab for each character
    private void GenerateCharacterInfo()
    {
        foreach(Transform character in GameManager.Instance.RemainingCharacters)
        {
            CharacterInfo characterInfo = Instantiate(characterInfoPrefab, gameUI).transform.GetComponent<CharacterInfo>();

            characterInfo.CharacterTransform = character;
        }
    }

    // Displays game over screen and the winning character's name
    private void GameEnd(Transform winner)
    {
        menuUI.SetActive(true);

        winText.text = "Winner: " + winner.name;
    }

    // Plays button press sound and reloads level scene
    public void RestartGame()
    {
        buttonPress.Play();

        SceneManager.LoadScene(1);
    }

    // Plays button press sound and loads/switches to menu scene
    public void MainMenu()
    {
        buttonPress.Play();

        SceneManager.LoadScene(0);
    }
}
