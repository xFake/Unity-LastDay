using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    [SerializeField]
    GameObject panelMain;

    [SerializeField]
    TextMeshProUGUI textResumeButton;

    GameObject currentPanel = null;

    bool isMouseVisible = true;

    private void Start()
    {
        currentPanel = panelMain;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelMain.activeSelf == false && currentPanel == null)
            {
                Cursor.visible = true;
                panelMain.SetActive(true);
                Time.timeScale = 0;
                currentPanel = panelMain;
            }
            else if(panelMain.activeSelf)
            {
                panelMain.SetActive(false);
                Time.timeScale = 1;
                currentPanel = null;
            }
        }
    }

    public void ResumeButtonClicked()
    {
        if (!isMouseVisible)
        {
            Cursor.visible = false;
        }
        GameController.SetGameState(true);
        textResumeButton.text = "Resume";
        Time.timeScale = 1;
        panelMain.SetActive(false);
        currentPanel = null;
    }

    public void RestartButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitButtonClicked()
    {
        //if (Application.isEditor)
        //{
        //    UnityEditor.EditorApplication.isPlaying = false;
        //}
        //else
        //{
            Application.Quit();
        //}
    }
    
    public void ButtonChangeMenu(GameObject panelEnable)
    {
        currentPanel.SetActive(false);
        panelEnable.SetActive(true);
        currentPanel = panelEnable;
    }

    public void SetMouseVisible()
    {
        isMouseVisible = !isMouseVisible;
    }
}
