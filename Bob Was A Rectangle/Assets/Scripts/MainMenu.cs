using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject panel;

    public void PlayGame()
    {
        SceneManager.LoadScene("SceneTutorial");
    }

    public void TutorialPanel()
    {
        panel.SetActive(!panel.activeSelf);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
