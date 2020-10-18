using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField]
    GameObject tutOne;
    [SerializeField]
    GameObject tutTwo;
    [SerializeField]
    GameObject tutThree;
    [SerializeField]
    GameObject tutFour;

    void Start()
    {
        tutTwo.SetActive(false);
        tutThree.SetActive(false);
        tutFour.SetActive(false);
    }

    public void EndOne()
    {
        tutOne.SetActive(false);
        tutTwo.SetActive(true);
        tutThree.SetActive(false);
        tutFour.SetActive(false);
    }

    public void EndTwo()
    {
        tutOne.SetActive(false);
        tutTwo.SetActive(false);
        tutThree.SetActive(true);
        tutFour.SetActive(false);
    }

    public void EndThree()
    {
        tutOne.SetActive(false);
        tutTwo.SetActive(false);
        tutThree.SetActive(false);
        tutFour.SetActive(true);
    }

    public void EndFour()
    {
        tutOne.SetActive(false);
        tutTwo.SetActive(false);
        tutThree.SetActive(false);
        tutFour.SetActive(false);
    }
}
