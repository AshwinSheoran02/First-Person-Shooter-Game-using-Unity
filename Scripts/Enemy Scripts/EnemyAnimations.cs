using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{

    private Animator anim;


    void Awake(){
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    public void Walk(bool walk){
        anim.SetBool(AnimationTags.WALK_PARAMETER , walk);
    }
    public void Run(bool run){
        anim.SetBool(AnimationTags.RUN_PARAMETER , run);
    }

    public void Attack(){
        anim.SetTrigger(AnimationTags.ATTACK_TRIGGER);
    }

    public void Dead(){
        anim.SetTrigger(AnimationTags.DEAD_TRIGGER);
    }
}
