using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimDataSetter : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void ResetTextEffect()
    {
        anim.SetBool("Effect Now", false);
    }
}
