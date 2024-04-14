using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// <para>
/// The music handler for 
/// </para>
/// </summary>
[RequireComponent(typeof(Game_Manager))]
public class GameMusicManager : MonoBehaviour
{
    public Game_Manager refGameManager;

    [Space]
    public AudioSource aSource_Music;
    public AudioMixer AMG_Master;
    public int currentSongId = 0, nextSongId = 0;
    public List<AudioClip> clips_precombat, clips_incombat, clips_postcombat;
    private GAMESTATE lastGameState;
    private bool changingSongNow = false;
    private float lastUpdateTime;

    private void OnEnable()
    {
        lastGameState = refGameManager.GameState;
        ChangeSong();
    }

    public void LateUpdate()
    {
        if (lastUpdateTime > Time.time + 3 || changingSongNow)
            return;

        if (lastGameState != refGameManager.GameState)
        {
            changingSongNow = true;
            ChangeSong();
        }

        lastUpdateTime = Time.time;
    }// end of LateUpdate()


    public void ChangeSong()
    {
        Vector2Int songRange = new Vector2Int(0, 1);
        AudioClip nextClip = null;

        switch (refGameManager.GameState)
        {
            case GAMESTATE.INCOMBAT:
                songRange.y = Random.Range(songRange.x, clips_incombat.Count-1);
                nextSongId = Random.Range(songRange.x, songRange.y);
                nextClip = clips_incombat[nextSongId];
                break;
            case GAMESTATE.POSTCOMBAT:
                songRange.y = Random.Range(songRange.x, clips_postcombat.Count-1);
                nextSongId = Random.Range(songRange.x, songRange.y);
                nextClip = clips_postcombat[nextSongId];
                break;
            case GAMESTATE.PRECOMBAT:
                songRange.y = Random.Range(songRange.x, clips_precombat.Count-1);
                nextSongId = Random.Range(songRange.x, songRange.y);
                nextClip = clips_precombat[nextSongId];
                break;
            default:
                break;
        }

        if(nextSongId == currentSongId)
        {
            ChangeSong();
            return;
        }

        StartCoroutine(FadeMusic(nextClip));
    }// end of ChangeSong()



    // function to fade the master sound
    private IEnumerator FadeMusic(AudioClip _nextClip)
    {
        print("Transitioning Music");

        float currentVolume = 0;
        AMG_Master.GetFloat("VolumeMusic", out currentVolume);

        //loop to lower volume
        for (float i = currentVolume; i >= -30; i--)
        {
            AMG_Master.SetFloat("VolumeMaster", i);
            yield return new WaitForSeconds(0.05f);
        }
        //change song
        aSource_Music.Stop();       
        aSource_Music.clip = _nextClip;
        aSource_Music.Play();

        //loop to raise volume
        for (float i = -50; i <= currentVolume; i++)
        {
            AMG_Master.SetFloat("VolumeMaster", i);
            yield return new WaitForSeconds(0.05f);
        }

        changingSongNow = false;
    }// end of FadeMusic()


}// END OF GameMusicManager class
