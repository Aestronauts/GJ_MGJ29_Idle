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

    private void OnEnable()
    {
        lastGameState = refGameManager.GameState;
        ChangeSong();
    }

    public void LateUpdate()
    {
        if (lastGameState != refGameManager.GameState)
        {
            ChangeSong();
        }
    }


    public void ChangeSong()
    {
        Vector2Int songRange = new Vector2Int(0, 1);

        switch (refGameManager.GameState)
        {
            case GAMESTATE.INCOMBAT:
                songRange.y = Random.Range(songRange.x, clips_incombat.Count-1);
                nextSongId = Random.Range(songRange.x, songRange.y);
                break;
            case GAMESTATE.POSTCOMBAT:
                songRange.y = Random.Range(songRange.x, clips_postcombat.Count-1);
                nextSongId = Random.Range(songRange.x, songRange.y);
                break;
            case GAMESTATE.PRECOMBAT:
                songRange.y = Random.Range(songRange.x, clips_precombat.Count-1);
                nextSongId = Random.Range(songRange.x, songRange.y);
                break;
            default:
                break;
        }

        if(nextSongId == currentSongId)
        {
            ChangeSong();
            return;
        }

        StartCoroutine(FadeMusic());
    }// end of ChangeSong()



    // function to fade the master sound
    private IEnumerator FadeMusic()
    {
        print("Transitioning Music");

        float currentVolume = 0;
        AMG_Master.GetFloat("VolumeMaster", out currentVolume);

        //loop to lower volume
        for (float i = currentVolume; i >= -50; i--)
        {
            AMG_Master.SetFloat("VolumeMaster", i);
            yield return new WaitForSeconds(0.05f);
        }
        //change song



    }// end of FadeMusic()


}// END OF GameMusicManager class
