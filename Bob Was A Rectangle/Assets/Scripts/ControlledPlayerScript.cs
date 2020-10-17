using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlledPlayerScript : MovementScript
{
    [SerializeField]
    GameObject PausePanel;

    [SerializeField]
    GameObject HowToPlay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            StartCoroutine(Move(MoveDirection.WEST));
        if (Input.GetKey(KeyCode.D))
            StartCoroutine(Move(MoveDirection.EAST));
        if (Input.GetKey(KeyCode.W))
            StartCoroutine(Move(MoveDirection.NORTH));
        if (Input.GetKey(KeyCode.S))
            StartCoroutine(Move(MoveDirection.SOUTH));
    }

    public void Pause()
    {
        PausePanel.SetActive(!PausePanel.activeSelf);
        HowToPlay.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PausePanel.SetActive(false);
    }

    public void HowTo()
    {
        HowToPlay.SetActive(!HowToPlay.activeSelf);
        PausePanel.SetActive(false);
    }

    public void CloseHowTo()
    {
        PausePanel.SetActive(!PausePanel.activeSelf);
        HowToPlay.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
