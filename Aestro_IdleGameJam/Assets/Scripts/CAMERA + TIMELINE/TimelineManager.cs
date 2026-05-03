using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System.Collections;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    [System.Serializable]
    public class TimelineData
    {
        [Tooltip("Unique identifier (e.g., 'IntroCutscene')")]
        public string timelineID;

        [Tooltip("Reference to the EXISTING PlayableDirector in your scene")]
        public PlayableDirector director;

        [Tooltip("Virtual cameras controlled by this timeline")]
        public CinemachineVirtualCamera[] timelineVCams;

        [Tooltip("Should gameplay pause during this timeline?")]
        public bool pauseGameplay = true;
    }

    [SerializeField] private TimelineData[] timelines;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void PlayTimeline(string timelineID)
    {
        TimelineData timeline = System.Array.Find(timelines, t => t.timelineID == timelineID);
        if (timeline?.director == null)
        {
            Debug.LogError($"Timeline {timelineID} not configured properly!");
            return;
        }

        StartCoroutine(PlayTimelineRoutine(timeline));
    }

    private IEnumerator PlayTimelineRoutine(TimelineData timeline)
    {
        // Activate timeline cameras
        foreach (var vcam in timeline.timelineVCams)
            vcam.Priority = 100;

        // Let the original director handle playback (with its own wrap mode)
        timeline.director.Play();

        if (timeline.pauseGameplay)
            Time.timeScale = 0f;

        // Only wait for completion if not looping
        if (timeline.director.extrapolationMode != DirectorWrapMode.Loop)
        {
            yield return new WaitUntil(() => timeline.director.state != PlayState.Playing);
            CleanupTimeline(timeline);
        }
    }

    public void StopTimeline(string timelineID)
    {
        var timeline = System.Array.Find(timelines, t => t.timelineID == timelineID);
        if (timeline?.director != null)
        {
            timeline.director.Stop();
            CleanupTimeline(timeline);
        }
    }

    private void CleanupTimeline(TimelineData timeline)
    {
        // Reset cameras
        foreach (var vcam in timeline.timelineVCams)
            vcam.Priority = 0;

        // Restore timescale
        if (timeline.pauseGameplay)
            Time.timeScale = 1f;
    }
}