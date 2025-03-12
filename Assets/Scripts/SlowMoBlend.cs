using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class SlowMoBlend : MonoBehaviour
{
    public float transitionSpeed = 0.5f;

    private float targetWeight = 0f;

    private void OnEnable()
    {
        targetWeight = 1f;
    }

    private void OnDisable()
    {
        GetComponent<PostProcessVolume>().weight = 0f;
    }

    void Update()
    {
        // Smooth Transition
        GetComponent<PostProcessVolume>().weight = Mathf.Lerp(GetComponent<PostProcessVolume>().weight, targetWeight, Time.deltaTime * transitionSpeed);
    }
}
