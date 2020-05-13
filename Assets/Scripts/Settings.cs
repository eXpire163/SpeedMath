using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Settings : MonoBehaviour
{

    public AudioMixer mixer;

    public Slider musicSlider;
    public Slider sfxSlider;

   
    

    private void Start()
    {
       

        //  IAmazonS3 s3Client = new AmazonS3Client(, RegionEndpoint.USEast1);

        float val = 0f;
        if (mixer.GetFloat("MasterVol", out val))
        {
            musicSlider.value = val;
        }
        if (mixer.GetFloat("SFXVol", out val))
        {
            sfxSlider.value = val;
        }
    }

    



    public void setMusicVolume(float valume)
    {
        Debug.Log(valume);
        mixer.SetFloat("MasterVol", valume);
    }
    public void setEfffectVolume(float valume)
    {
        Debug.Log(valume);
        mixer.SetFloat("SFXVol", valume);
    }
    public void done()
    {
        SceneManager.LoadScene("Board");
    }
}
