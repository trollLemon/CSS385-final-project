using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAPI : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip extraClip;
    // Start is called before the first frame update
    public float volume=0.5f;
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

    public void OneShot()
    {
    audioSource.PlayOneShot(audioSource.clip, volume);
    }

    public void OneShotSpecial()
    {
     	    if(extraClip == null){
	    Debug.Log("No extra Clip defined");
	    }
	    audioSource.PlayOneShot(extraClip, volume);
     

    }

    public void StopAudio()
    {
	if(!playing) return;
	audioSource.Stop();
 	playing = false;   
    }
}
