using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
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

        [Tooltip("Objects active in this timeline")]
        public CinemachineVirtualCamera[] timelineObjects;

        [Tooltip("Should gameplay pause during this timeline?")]
        public bool pauseGameplay = true;
    }

    [SerializeField] private TimelineData[] timelines;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayTimeline(string timelineID)
    {
        Debug.Log($"Play {timelineID}");

        TimelineData timeline = System.Array.Find(
            timelines,
            t => t.timelineID == timelineID
        );

        if (timeline?.director == null)
        {
            Debug.LogError($"Timeline {timelineID} not configured properly!");
            return;
        }

        // Bind the currently spawned boss to the Timeline
        BindBoss(timeline.director);

        StartCoroutine(PlayTimelineRoutine(timeline));
    }

    private void BindBoss(PlayableDirector director)
    {
        // Make sure a BossBehavior exists
        if (BossBehavior.instance == null)
        {
            Debug.LogError("TimelineManager: No BossBehavior instance found!");
            return;
        }

        // Make sure the boss has been spawned
        if (BossBehavior.instance.BossAnimator == null)
        {
            Debug.LogError(
                "TimelineManager: BossAnimator has not been assigned yet!"
            );

            return;
        }

        // Get the Timeline asset
        TimelineAsset timeline = director.playableAsset as TimelineAsset;

        if (timeline == null)
        {
            Debug.LogError(
                $"TimelineManager: {director.name} does not have a TimelineAsset!"
            );

            return;
        }

        // Get the actual Boss Animator
        Animator currentBossAnimator =
            BossBehavior.instance.BossAnimator;

        // Go through every binding in the Timeline
        foreach (PlayableBinding binding in timeline.outputs)
        {
            // Get whatever object is currently bound to this track
            Object currentBinding =
                director.GetGenericBinding(binding.sourceObject);

            if (currentBinding == null)
                continue;

            // We're only interested in Animator bindings
            Animator existingAnimator =
                currentBinding as Animator;

            if (existingAnimator == null)
                continue;

            // Check if this Animator belongs to the Boss hierarchy
            if (IsPartOfBoss(existingAnimator))
            {
                // Replace the Timeline binding with the current boss
                director.SetGenericBinding(
                    binding.sourceObject,
                    currentBossAnimator
                );

                Debug.Log(
                    $"TimelineManager: Boss Timeline track rebound from " +
                    $"{existingAnimator.gameObject.name} to " +
                    $"{currentBossAnimator.gameObject.name}"
                );

                return;
            }
        }

        Debug.LogWarning(
            $"TimelineManager: Could not find a Boss Animation Track " +
            $"in Timeline '{director.name}'."
        );
    }

    private bool IsPartOfBoss(Animator animator)
    {
        if (BossBehavior.instance == null)
            return false;

        // Check whether the Animator exists somewhere underneath
        // the BossBehavior GameObject.
        return animator.transform.IsChildOf(
            BossBehavior.instance.transform
        );
    }

    private IEnumerator PlayTimelineRoutine(TimelineData timeline)
    {
        // Activate timeline cameras
        foreach (var vcam in timeline.timelineVCams)
        {
            if (vcam != null)
                vcam.enabled = true;
        }

        // Make Timeline continue playing when Time.timeScale = 0
        if (timeline.pauseGameplay)
        {
            timeline.director.timeUpdateMode =
                DirectorUpdateMode.UnscaledGameTime;
        }

        // Play Timeline
        timeline.director.Play();

        // Pause gameplay
        if (timeline.pauseGameplay)
        {
            Time.timeScale = 0f;
        }

        // Wait for Timeline to finish
        if (timeline.director.extrapolationMode != DirectorWrapMode.Loop)
        {
            yield return new WaitUntil(
                () => timeline.director.state != PlayState.Playing
            );

            CleanupTimeline(timeline);
        }
    }

    public void StopTimeline(string timelineID)
    {
        TimelineData timeline = System.Array.Find(
            timelines,
            t => t.timelineID == timelineID
        );

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
        {
            if (vcam != null)
                vcam.enabled = false;
        }

        // Restore timescale
        if (timeline.pauseGameplay)
        {
            Time.timeScale = 1f;
        }
    }
}