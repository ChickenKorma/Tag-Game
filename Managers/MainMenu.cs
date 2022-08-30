using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource buttonPress;

    private void Awake()
    {
        buttonPress = GetComponent<AudioSource>();
    }

    // Plays the button press sound, loads and switches to the level
    public void StartGame()
    {
        buttonPress.Play();

        SceneManager.LoadScene(1);
    }
}
