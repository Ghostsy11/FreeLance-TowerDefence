using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] bool isPaused;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject MainMenuUi;
    [SerializeField] GameObject selectingMode;
    [SerializeField] GameObject achevmeant;
    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (!isPaused)
        {
            PauseMenu();
        }
        else
        {
            UnPauseMenu();
        }
    }


    private void PauseMenu()
    {
        isPaused = true;
        menuPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void UnPauseMenu()
    {
        isPaused = false;
        menuPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void LoadSceneFromPauseMenu()
    {

        if (MainMenuUi != null)
        {
            MainMenuUi.SetActive(true);
            menuPanel.SetActive(false);
            selectingMode.SetActive(false);
            achevmeant.SetActive(false);
            // Time.timeScale = 1f;
            //isPaused = false;
        }

    }

}
