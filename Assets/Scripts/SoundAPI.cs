using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAPI : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called before the first frame update

    private bool playing;

    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
    }
	
    // Update is called once per frame
    void Update()
    {
    }

    public void StartAudio()
    {
	if(playing) return; //if the audio is playing already, dont start again
	audioSource.Play();
	playing = true;
    }

    public void StopAudio()
    {
	if(!playing) return;
	audioSource.Stop();
 	playing = false;   
    }
}
