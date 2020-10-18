using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    [SerializeField] AudioClip music;
    private AudioSource sound;

    private void Awake()
    {
        sound = gameObject.GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        sound.clip = music;
        sound.loop = true;
        sound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
