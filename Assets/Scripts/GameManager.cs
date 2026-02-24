using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AudioSource otherSounds;
    public bool isPaused;

    public Image pauseUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(PlayerScript.Instance != null)
            if (PlayerScript.Instance.isDead) return;
        if (otherSounds == null) return;
        if (pauseUI == null) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResume();
        }
    }
    public void ShootSound(AudioClip shoot)
    {
        otherSounds.PlayOneShot(shoot);
    }

    public void ReloadSound(AudioClip reload)
    {
        otherSounds.PlayOneShot(reload);
    }

    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        Debug.Log("Button Pressed");
        SceneManager.LoadScene("Home");
    }

    public void PauseResume()
    {
        if (isPaused)
        {
            pauseUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            pauseUI.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }
}
