using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string triggerName = "Play";
    

    private void Awake()
    {
        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger(triggerName);
        }
    }
}
