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
    public int currentSongId = -1, nextSongId = 0;
    public List<AudioClip> clips_precombat, clips_incombat, clips_postcombat;
    private GAMESTATE lastGameState;
    private bool changingSongNow = false;
    private float lastUpdateTime;

    private void OnEnable()
    {
        currentSongId = -1;
        lastGameState = refGameManager.GameState;
        lastUpdateTime = Time.time + 3;        
        ChangeSong(); // play our first song
        changingSongNow = true;
    }

    public void FixedUpdate()
    {     
        if (Time.time <= lastUpdateTime + 3 || changingSongNow) // check for song finished
            return;

        if (lastGameState != refGameManager.GameState || !aSource_Music.isPlaying)
        {
            print("game state not aligning");
            changingSongNow = true;
            ChangeSong();
        }

        lastUpdateTime = Time.time;
    }// end of LateUpdate()


    public void ChangeSong()
    {
        Vector2Int songRange = new Vector2Int(-1, 1);
        AudioClip nextClip = null;
        int maxCount = 0;
        switch (refGameManager.GameState)
        {
            case GAMESTATE.INCOMBAT:
                maxCount = clips_incombat.Count;
                print($"number: {maxCount}");
                songRange.y = Random.Range(songRange.x, maxCount);
                nextSongId = Random.Range(songRange.x, songRange.y);
                if (nextSongId <= 0)
                    nextSongId = 0;
                if (nextSongId >= maxCount)
                    nextSongId = maxCount-1;
                nextClip = clips_incombat[nextSongId];
                break;
            case GAMESTATE.POSTCOMBAT:
                maxCount = clips_incombat.Count;
                print($"number: {maxCount}");
                songRange.y = Random.Range(songRange.x, maxCount);
                nextSongId = Random.Range(songRange.x, songRange.y);
                if (nextSongId <= 0)
                    nextSongId = 0;
                if (nextSongId >= maxCount)
                    nextSongId = maxCount-1;
                nextClip = clips_postcombat[nextSongId];
                break;
            case GAMESTATE.PRECOMBAT:
                maxCount = clips_incombat.Count;
                print($"number: {maxCount}");
                songRange.y = Random.Range(songRange.x, maxCount);
                nextSongId = Random.Range(songRange.x, songRange.y);
                if (nextSongId <= 0)
                    nextSongId = 0;
                if (nextSongId >= maxCount)
                    nextSongId = maxCount-1;
                nextClip = clips_precombat[nextSongId];
                break;
            default:
                changingSongNow = false;
                lastUpdateTime = Time.time;
                break;
        }

        // if we are changing songs but we're not changing GAME STATES then dont pick the same song
        if (nextSongId == currentSongId && !aSource_Music.isPlaying)
        {
            ChangeSong();
            return;
        }
        else
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
            yield return new WaitForSeconds(0.025f);
        }
        //change song
        if(!aSource_Music.isPlaying)
            aSource_Music.Stop();       
        aSource_Music.clip = _nextClip;
        aSource_Music.Play();

        //loop to raise volume
        for (float i = -50; i <= currentVolume; i++)
        {
            AMG_Master.SetFloat("VolumeMaster", i);
            yield return new WaitForSeconds(0.0025f);
        }

        lastGameState = refGameManager.GameState;
        print("Music Done Transitioning");
        yield return new WaitForSeconds(2f); // forced time before song will be allowed to be changed
        changingSongNow = false;
    }// end of FadeMusic()


}// END OF GameMusicManager class
