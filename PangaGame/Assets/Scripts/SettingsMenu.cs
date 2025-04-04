using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GameObject.FindGameObjectsWithTag("BSO")[0].GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundOff()
    {
        audio.mute = true;
        return;
    }

    public void SoundOn()
    {
        audio.mute = false;
        return;
    }
}
