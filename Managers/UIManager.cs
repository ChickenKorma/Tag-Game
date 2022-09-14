using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject characterInfoPrefab;

    [SerializeField] private Transform gameUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject pauseUI;

    [SerializeField] private TMP_Text winText;

    private AudioSource buttonPress;

    private bool paused;
    private bool gameOver;

    private void OnEnable()
    {
        GameManager.gameEndEvent += GameEnd;

        InputManager.pauseEvent += PauseGame;
    }

    private void OnDisable()
    {
        GameManager.gameEndEvent -= GameEnd;

        InputManager.pauseEvent -= PauseGame;
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
    }

    private void Start()
    {
        Time.timeScale = 1;

        menuUI.SetActive(false);
        pauseUI.SetActive(false);

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
        gameOver = true;

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

    // Toggles paused bool, enabling/disabling the pause screen and freezing/unfreezing the time scale accordingly
    public void PauseGame()
    {
        if (!gameOver)
        {
            if (paused)
            {
                Time.timeScale = 1;

                pauseUI.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;

                pauseUI.SetActive(true);
            }

            paused = !paused;
        }
    }
}
