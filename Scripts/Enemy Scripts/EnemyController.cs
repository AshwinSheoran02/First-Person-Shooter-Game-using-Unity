using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState{
    PATROL , CHASE , ATTACK
}

public class EnemyController : MonoBehaviour
{

    private EnemyAnimations enemy_Anim;

    private NavMeshAgent navAgent;

    private EnemyState enemy_State;

    public float walk_speed = 0.5f;

    public float run_speed = 4f;

    public float chase_distance = 7f;

    private float current_chase_distant;

    public float attack_distance = 1.8f;

    public float chase_After_Attack_Distance=2f;
    public float patrol_Radius_Min = 20f;
    public float patrol_Radius_Max = 60f;

    public float patrol_For_This_Time = 15f;

    private float patrol_Timer;

    public float wait_Before_Attack = 2f;

    public float attack_Timer;

    private Transform target;

    public GameObject attack_Point;


    void Awake(){
        enemy_Anim = GetComponent<EnemyAnimations>();
        navAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
    }

    // Start is called before the first frame update
    void Start()
    {

        enemy_State = EnemyState.PATROL;

        patrol_Timer = patrol_For_This_Time;
        attack_Timer = wait_Before_Attack;

        current_chase_distant = chase_distance;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (enemy_State == EnemyState.PATROL){
            Patrol();
        }
        if (enemy_State == EnemyState.CHASE){
            Chase();
        }
        if (enemy_State == EnemyState.ATTACK){
            Attack();
        }
        
    }

    void Patrol(){
        navAgent.isStopped = false;
        navAgent.speed = walk_speed;

        patrol_Timer += Time.deltaTime;

        if (patrol_Timer> patrol_For_This_Time){
            SetNewRandomDestination();
            patrol_Timer = 0f;
        }

        if (navAgent.velocity.sqrMagnitude > 0){
            enemy_Anim.Walk(true);
        }
        else{
            enemy_Anim.Walk(false);
        }

        if (Vector3.Distance(transform.position , target.position)<=chase_distance){
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.CHASE;
        }

    }
    void Chase(){

        navAgent.isStopped = false;
        navAgent.speed = run_speed;
        navAgent.SetDestination(target.position);   // Moving towards the player
         if (navAgent.velocity.sqrMagnitude > 0){
            enemy_Anim.Run(true);
        }
        else{
            enemy_Anim.Run(false);
        }
        if (Vector3.Distance(transform.position , target.position) <= attack_distance){  // start attacking
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;

            if (chase_distance != current_chase_distant){
                chase_distance = current_chase_distant;
            }

        }
        else if (Vector3.Distance(transform.position , target.position) > chase_distance){
                //player runs away
                enemy_Anim.Run(false); 
                enemy_State = EnemyState.PATROL;
                patrol_Timer = patrol_For_This_Time;
                if (chase_distance != current_chase_distant){
                    chase_distance = current_chase_distant;
                }
        }

    }

    void Attack(){
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;
        attack_Timer+=Time.deltaTime;
        if (attack_Timer>wait_Before_Attack){
            enemy_Anim.Attack();
            attack_Timer = 0f;
        }
        if(Vector3.Distance(transform.position , target.position)>attack_distance+chase_After_Attack_Distance){
            enemy_State = EnemyState.CHASE;
        }


    }
    void SetNewRandomDestination(){
        float rand_radius = Random.Range(patrol_Radius_Min , patrol_Radius_Max);

        Vector3  randDir = Random.insideUnitSphere * rand_radius;
        randDir += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir , out navHit , rand_radius , -1);

        navAgent.SetDestination(navHit.position);

    }

    void Turn_On_AttackPoint() {
        attack_Point.SetActive(true);
    }

    void Turn_Off_AttackPoint() {
        if(attack_Point.activeInHierarchy) {
            attack_Point.SetActive(false);
        }
    }

    public EnemyState Enemy_State {
        get; set;
    }
}
