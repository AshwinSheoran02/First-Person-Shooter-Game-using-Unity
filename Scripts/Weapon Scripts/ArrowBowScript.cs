using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBowScript : MonoBehaviour
{

    private Rigidbody myBody;

    public float speed = 30f;

    public float deactivate_Timer = 3f;

    public float damage = 15f;


    void Awake(){
        myBody = GetComponent<Rigidbody>();

    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactiveGameObject" , deactivate_Timer);

        
    }

    public void Launch(Camera mainCamera){
        myBody.velocity = mainCamera.transform.forward*speed;

        transform.LookAt(transform.position + myBody.velocity );
    }

    void deactivateGameObject(){
        if (gameObject.activeInHierarchy){
        gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider target){ //on touching enemy

    }
}
