using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    private Light candlelight;
    private void Awake()
    {
        candlelight = transform.GetChild(1).gameObject.GetComponent<Light>();
    }
    // Start is called before the first frame update
    void Start()
    {
        candlelight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CandleInterval(float t)
    {
        Light();
        yield return new WaitForSeconds(t);
        Extinguish();
    }

    public void Light()
    {
        candlelight.gameObject.SetActive(true);
    }

    public void Extinguish()
    {
        candlelight.gameObject.SetActive(false);
    }
}
