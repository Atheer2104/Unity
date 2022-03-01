using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimation : MonoBehaviour
{
    public Animator animator;
    
    public void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    public void AnimationChange(int state)
    {
        animator.SetInteger("Animation", state);
    }
}
