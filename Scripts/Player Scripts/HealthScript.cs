using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class HealthScript : MonoBehaviour
{

    private EnemyAnimations enemy_Anim;
    private NavMeshAgent navAgent;
    private EnemyController enemy_Controller;

    public float health = 100f;

    public bool is_Player , is_Boar , is_Cannibal;

    public bool is_Dead;

    void Awake(){
        if(is_Boar || is_Cannibal){
            enemy_Anim = GetComponent<EnemyAnimations>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

        }

        if (is_Player){

        }

    }

    public void ApplyDamage(float damage){
        if (is_Dead){
            return;
        }
        health -= damage;

        if (is_Player){

        }
        if(is_Boar || is_Cannibal) {
            if(enemy_Controller.Enemy_State == EnemyState.PATROL) {
                enemy_Controller.chase_distance = 50f;
            }
        }

        if (health>= 0f){
            playerDied();
            is_Dead = true;
        }
        

    }

    void playerDied(){
        if (is_Cannibal){
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward*10f);

            enemy_Controller.enabled = false;
            navAgent.enabled = false;
            enemy_Anim.enabled = false;


        }

        if (is_Boar){
             GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward*10f);

            enemy_Controller.enabled = false;
            navAgent.enabled = false;
            enemy_Anim.enabled = false;
            // navAgent.velocity = Vector3.zero;
            // navAgent.isStopped = true;
            // enemy_Controller.enabled = false;
            // enemy_Anim.Dead();
        }

        if(is_Player){
            print("Lmao Ded");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
            for (int i = 0 ; i<enemies.Length ; i++){
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);


        }

        if(tag == Tags.PLAYER_TAG) {

            Invoke("RestartGame", 3f);

        } else {

            Invoke("TurnOffGameObject", 3f);

        }

    } // player died

    void RestartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    void TurnOffGameObject() {
        gameObject.SetActive(false);
    
    }

    // Update is called once per frame
     
}
