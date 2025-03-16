using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum Animations
{
    Walking,
    Sitting,
    Standing,
    Typing,
    Yelling,
    YellingOut,
    Leaning,
    Dancing,
    Looking,
    SittingAngry,
    Drinking
}

public class AnimationQueuer : MonoBehaviour
{

    [SerializeField] List<Animations> animationStates = new();
    private Animator animator;
    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlayAnimations());
    }

    IEnumerator PlayAnimations()
    {
        while (true)
        {
            animator.Play(animationStates[index].ToString());
            if (animationStates.Count == 1)
                yield break;
            float delay = GetAnimationDelay(animationStates[index++]);
            yield return new WaitForSeconds(delay);
            if (index >= animationStates.Count)
                index = 0;
        }
    }

    private float GetAnimationDelay(Animations animationName)
    {
        for (int i = 0; i<GameData.AnimationClips.Count; i++)
            if (animationName == GameData.AnimationClips[i].name)
                return GameData.AnimationClips[i].clip.length;

        return 1f;
    }
}
