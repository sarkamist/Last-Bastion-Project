using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public GameObject namePanel;
    public GameObject[] subPanels;
    public AudioManagerSc audioManager;
    private int counter = 0;

    public KeyCode panelKey = KeyCode.Escape;

    void Update()
    {
        if (Input.GetKeyDown(panelKey) && counter % 2 == 0)
        {
            namePanel.SetActive(!namePanel.activeSelf);
            audioManager.PlaySFX(audioManager.openMenu);
            counter++;
        }
        else if (Input.GetKeyDown(panelKey) && counter % 2 != 0)
        {
            namePanel.SetActive(!namePanel.activeSelf);
            audioManager.PlaySFX(audioManager.closeMenu);
            counter = 0;
        }
    }

    public void OpenPanel()
    {
        namePanel.SetActive(true);
        audioManager.PlaySFX(audioManager.openMenu);
    }
    public void ClosePanel()
    {
        namePanel.SetActive(false);
        audioManager.PlaySFX(audioManager.closeMenu);
    }

    public void OpenSubPanel(GameObject targetPanel)
    {
        foreach (GameObject panel in subPanels) 
        {
            panel.SetActive(false);
        }
        targetPanel.SetActive(true);
        audioManager.PlaySFX(audioManager.openMenu);
    }

    public void CloseSubPanels()
    {
        foreach(GameObject panel in subPanels)
        {
            panel.SetActive(false);
            audioManager.PlaySFX(audioManager.closeMenu);
        }
    }

    public void SceneLoader(string sceneName)
    {
        if (sceneName == "MainScene")
        {
            Destroy(RoundManager.Instance?.gameObject);
            Destroy(EnemySpawner.Instance?.gameObject);
        }
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void ExitGame()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}
