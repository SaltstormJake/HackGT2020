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

    public IEnumerator CandleInterval(float t)
    {
        Light();
        yield return new WaitForSeconds(t);
        Extinguish();
    }

    public void Light()
    {
        sound.clip = candleLightingSound;
        sound.Play();
        candlelight.gameObject.SetActive(true);
    }

    public void Extinguish()
    {
        sound.clip = candleOutSound;
        sound.Play();
        candlelight.gameObject.SetActive(false);
    }
}
