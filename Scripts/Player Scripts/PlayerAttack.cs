using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private WeaponManager weapon_Manager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim;
    private bool zoomed;
    private Camera mainCam;

    private GameObject crosshair;

    private bool is_Aiming;

    [SerializeField]
    private GameObject arrow_Prefab , spear_Prefab;

    [SerializeField]
    private Transform arrow_bow_start_position;

    private Vector3 scaleChange;

    void Awake(){
        weapon_Manager = GetComponent<WeaponManager>();

        zoomCameraAnim = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);

        mainCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();

        
    }

    void WeaponShoot(){
         if(weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE) {

            // if we press and hold left mouse click AND
            // if Time is greater than the nextTimeToFire
            if(Input.GetMouseButton(0) && Time.time > nextTimeToFire) {

                nextTimeToFire = Time.time + 1f / fireRate;

                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                 BulletFired();

            }


            // if we have a regular weapon that shoots once
        }
        else {

            if(Input.GetMouseButtonDown(0)) {

                // handle axe
                if(weapon_Manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG) {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation(); 
                }

                // handle shoot
                if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET) {

                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFired();
                }
                else{
                    if (is_Aiming){
                        weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                        if (weapon_Manager.GetCurrentSelectedWeapon().bulletType==WeaponBulletType.ARROW){
                            ThrowArrowOrSpear(true);

                        }
                        if (weapon_Manager.GetCurrentSelectedWeapon().bulletType==WeaponBulletType.SPEAR){
                            ThrowArrowOrSpear(false);

                        }

                    }

                }
            }
        }
    }

    void ZoomInAndOut() {

        // we are going to aim with our camera on the weapon
        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM) {

            // if we press and hold right mouse button
            if(Input.GetMouseButtonDown(1)) {

                zoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);

                // crosshair.SetActive(false);
                // crosshair.Scale(2);
                // scaleChange = new Vector3(2f, 2f, 2f);
                // crosshair.transform.localScale += scaleChange;
                // crosshair.gameObject.transform.localScale += new Vector3(1, 1, 0);

            }
            if(Input.GetMouseButtonDown(1)) {
                crosshair.gameObject.transform.localScale += new Vector3(1, 1, 0);
            }

            // when we release the right mouse button click
            if (Input.GetMouseButtonUp(1)) {

                zoomCameraAnim.Play(AnimationTags.ZOOM_OUT_ANIM);

                // crosshair.SetActive(true);
                crosshair.gameObject.transform.localScale -= new Vector3(1, 1, 0);
            }

        } // if we need to zoom the weapon

        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM) {

            if(Input.GetMouseButtonDown(1)) {

                weapon_Manager.GetCurrentSelectedWeapon().Aim(true);

                is_Aiming = true;

            }

            if (Input.GetMouseButtonUp(1)) {

                weapon_Manager.GetCurrentSelectedWeapon().Aim(false);

                is_Aiming = false;

            }

        } // weapon self aim

    }

    void ThrowArrowOrSpear(bool throwArrow){

        if (throwArrow){
            GameObject arrow = Instantiate(arrow_Prefab);
            arrow.transform.position = arrow_bow_start_position.position;

            arrow.GetComponent<ArrowBowScript>().Launch(mainCam);
        }
        else{
             GameObject spear = Instantiate(spear_Prefab);
            spear.transform.position = arrow_bow_start_position.position;

            spear.GetComponent<ArrowBowScript>().Launch(mainCam);

        }
    }

     void BulletFired() {

        RaycastHit hit;

        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit)) {

            if(hit.transform.tag == Tags.ENEMY_TAG) {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }

        }

    } // bullet fired

} // class

