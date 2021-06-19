using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoopPlay : MonoBehaviour
{
   public AudioSource begginingMusic;
   private AudioSource thisAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        thisAudioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!begginingMusic.isPlaying&&!thisAudioSource.enabled)
        {
            thisAudioSource.enabled = true;
        }    
    }
}
