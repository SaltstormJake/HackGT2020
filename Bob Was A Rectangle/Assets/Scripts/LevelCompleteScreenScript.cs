using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelCompleteScreenScript : MonoBehaviour
{
    [SerializeField] private Button nextLevelButton = null;
    [SerializeField] private Button mainMenuButton = null;
    [SerializeField] string nextLevel = null;
    [SerializeField] string mainMenu = null;
    [SerializeField] GameObject panel;
    private void Awake()
    {
        nextLevelButton.onClick.AddListener(OnClickNextLevel);
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
    }

    private void Start()
    {
        panel.SetActive(false);
    }

    void OnClickNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    void OnClickMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void SetAll(bool active)
    {
        panel.SetActive(active);

        /*GameObject[] menuObjects = GameObject.FindGameObjectsWithTag("LevelFinishScreenOnly");
        foreach (GameObject g in menuObjects)
            g.SetActive(active);*/
    }

}
