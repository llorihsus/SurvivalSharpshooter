using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int winKillCount = 10; // Number of kills needed to win
    [SerializeField] public GameObject winMenu;
    [SerializeField] public TMP_Text killCountText; // kill count text
    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private GameObject deathMenu;


    private bool isPaused = false;

    private int killCount = 0;

    private void Start()
    {
        UpdateKillCountText();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (winMenu.activeSelf || deathMenu.activeSelf) return;

            if (isPaused)
                ResumeGame();
            else
                OpenEscapeMenu();
        }
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
    }

    private void HandleEnemyKilled()
    {
        killCount++;

        Debug.Log("Kill Count: " + killCount + " / " + winKillCount);

        UpdateKillCountText();

        if (killCount >= winKillCount)
        {
            WinSequence();
        }
    }

    private void UpdateKillCountText()
    {
        if (killCountText != null)
        {
            killCountText.text = "Kills: " + killCount + " / " + winKillCount;
        }
    }

    private void WinSequence()
    {
        if (winMenu != null)
        {
            if (killCountText != null)
            {
                killCountText.text = "You killed " + killCount + " Enemies!";
            }

            winMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f; // THIS is the key
        }

        Debug.Log("YOU WIN!");
    }

    public void ResetKillCount()
    {
        killCount = 0;
        UpdateKillCountText();
    }

    public void OpenEscapeMenu()
    {
        if (escapeMenu != null)
        {
            escapeMenu.SetActive(true);
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPaused = true;
    }

    public void ResumeGame()
    {
        if (escapeMenu != null)
        {
            escapeMenu.SetActive(false);
        }

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }
}