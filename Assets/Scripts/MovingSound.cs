using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSound : MonoBehaviour
{
    private AudioSource audioComp;
    void Start()
    {
        gameObject.GetComponent<CharacterControler>().OnIsMovingChanged.AddListener(playSound);
        audioComp = gameObject.GetComponent<AudioSource>();
    }

    void playSound()
    {
        if(!audioComp.isPlaying)
        {
            audioComp.volume = Random.Range(0.8f, 1f);
            audioComp.pitch = Random.Range(0.8f, 1.1f);
            audioComp.Play();
        }
    }
}
