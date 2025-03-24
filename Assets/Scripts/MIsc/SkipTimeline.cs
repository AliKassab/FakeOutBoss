using UnityEngine;
using UnityEngine.Playables;

public class SkipTimeline : MonoBehaviour
{
    [SerializeField] PlayableDirector timeline;
    [SerializeField] float timeOffset;
    public void SkipCutscene()
    {
        if (timeline != null)
        {
            timeline.time = timeline.duration - timeOffset;
            timeline.Evaluate();
        }
    }
}
