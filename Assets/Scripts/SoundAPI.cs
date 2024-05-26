using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAPI : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] extraClips;
    public AudioClip SpecialClip;
    // Start is called before the first frame update
    public float volume=0.5f;
    private bool playing;
    private int nextSound = 0;

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
     	    if(extraClips == null){
	    Debug.Log("No extra Clips defined");
	    }
	    audioSource.PlayOneShot(SpecialClip, volume);
    }

    public void PlayExtra(bool switchNext) 
    {
	audioSource.PlayOneShot(extraClips[nextSound]);

	if(switchNext)
	{
		nextSound++;
		nextSound = nextSound % extraClips.Length;
	}
    }
    public void StopAudio()
    {
	if(!playing) return;
	audioSource.Stop();
 	playing = false;   
    }
}
