using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager _instance;
    public static MainMenuManager Instance_MainMenuManager { get { return _instance; } }

    public AudioMixer AMG_Master;
    public float audioVol_All, audioVol_Music, audioVol_Sfx, audoVol_Atmosphere;
    public Slider sliderVol_All, sliderVol_Music, sliderVol_Sfx, sliderVol_Atmosphere;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    public void ChangeSliderVolume(Slider _sliderObj)
    {
        if (_sliderObj == sliderVol_All)
            audioVol_All = sliderVol_All.value;
        if (_sliderObj == sliderVol_Music)
            audioVol_Music = sliderVol_Music.value;
        if (_sliderObj == sliderVol_Sfx)
            audioVol_Sfx = sliderVol_Sfx.value;
        if (_sliderObj == sliderVol_Atmosphere)
            audoVol_Atmosphere = sliderVol_Atmosphere.value;

        // update the mixer
        UpdateMixerGroups();
    }// end of ChangeSliderVolume()

    public void UpdateMixerGroups()
    {
        AMG_Master.SetFloat("VolumeMaster", sliderVol_All.value);
        AMG_Master.SetFloat("VolumeMusic", sliderVol_Music.value);
        AMG_Master.SetFloat("VolumeSfx", sliderVol_Sfx.value);
        AMG_Master.SetFloat("VolumeAtmospheric", audoVol_Atmosphere);
    }


    public void SceneLoader(string _sceneToLoad) // NOTE - May want to wait for animation timeline to finish first
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene(_sceneToLoad);
    }

}// end of MainMenuManager class
