using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelCompleteScreenScript : MonoBehaviour
{
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] string nextLevel;
    [SerializeField] string mainMenu;
    private void Awake()
    {
        nextLevelButton.onClick.AddListener(OnClickNextLevel);
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
    }

    void OnClickNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    void OnClickMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

}
