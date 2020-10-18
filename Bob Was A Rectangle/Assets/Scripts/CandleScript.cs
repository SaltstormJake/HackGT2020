using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    [SerializeField] private Light candlelight = null;
    private AudioSource sound = null;
    [SerializeField] AudioClip candleLightingSound = null;
    [SerializeField] float candleIntensity = 1.0f;
    [SerializeField] float candleRange = 100.0f;
    [SerializeField] AudioClip candleOutSound = null;
    [SerializeField] GameObject darkness = null;

    private bool lit = false;
    private void Awake()
    {
        sound = gameObject.GetComponent<AudioSource>();
        candlelight = transform.GetChild(1).gameObject.GetComponent<Light>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CandleInterval(2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        candlelight.range = candleRange;
        candlelight.intensity = candleIntensity;
    }

    public IEnumerator CandleInterval(float t = 2.0f)
    {
        Debug.Log("Lighting");

        if (!lit)
        {
            Light();
            yield return new WaitForSeconds(t);
            Extinguish();
        }
    }

    public void Light()
    {
        if (!lit)
        {
            darkness.SetActive(false);
            sound.clip = candleLightingSound;
            sound.Play();
            candlelight.gameObject.SetActive(true);
            lit = true;
        }
    }

    public void Extinguish()
    {
        if (lit)
        {
            darkness.SetActive(true);
            sound.clip = candleOutSound;
            sound.Play();
            candlelight.gameObject.SetActive(false);
            lit = false;
        }
    }
}
