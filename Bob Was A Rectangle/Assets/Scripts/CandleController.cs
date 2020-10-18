using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleController : MonoBehaviour
{
    [SerializeField] 
    GameObject candleController;
    [SerializeField] 
    GameObject darkness;

    public void Start() {
        DontDestroyOnLoad(candleController);
    }

    public void LightAll()
    {
        /*for (int i = 0; i < candleController.gameObject.transform.GetChildCount(); i++)
        {
            CandleScript cs = candleController.gameObject.transform.GetChild(i).GameObject.GetComponent<CandleScript>();

            cs.CandleInterval();
        }*/

        foreach (Transform t in candleController.transform)
        {
            StartCoroutine(t.gameObject.GetComponent<CandleScript>().CandleInterval());
        }
    }
}
