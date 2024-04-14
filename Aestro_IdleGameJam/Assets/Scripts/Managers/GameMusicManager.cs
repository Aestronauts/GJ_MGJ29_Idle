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
    public int currentSongId = 0;
    public List<AudioClip> clips_precombat, clips_incombat, clips_postcombat;


    public void ChangeSong()
    {
        Vector2Int songRange = new Vector2Int(0, 1);

        switch (refGameManager.GameState)
        {
            case GAMESTATE.INCOMBAT:
                songRange.y = Random.Range(songRange.x, clips_incombat.Count-1);
                currentSongId = Random.Range(songRange.x, songRange.y);
                break;
            case GAMESTATE.POSTCOMBAT:
                songRange.y = Random.Range(songRange.x, clips_postcombat.Count-1);
                currentSongId = Random.Range(songRange.x, songRange.y);
                break;
            case GAMESTATE.PRECOMBAT:
                songRange.y = Random.Range(songRange.x, clips_precombat.Count-1);
                currentSongId = Random.Range(songRange.x, songRange.y);
                break;
            default:
                break;
        }

    }// end of ChangeSong()



    // function to fade the master sound
    private IEnumerator FadeMusic()
    {
        yield return new WaitForSeconds(0);


    }// end of FadeMusic()


}// END OF GameMusicManager class
