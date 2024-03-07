using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Debug stuff")]
    public GameObject bullethole;
    public int damage;

    public float fireRate;

    private float nextFire;

    public Camera camera;

    // Update is called once per frame
    void Update()
    {
        if(nextFire > 0)
            nextFire -= Time.deltaTime;

        if(Input.GetButton("Fire1") && nextFire <= 0)
        {
           nextFire = 1 /fireRate;
            Fire();

        }
        
    }

    void Fire()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        RaycastHit hit;

        if(Physics.Raycast(ray.origin,ray.direction, out hit,100f))
        {
            //update: make bullet impact plus effect
            PhotonNetwork.Instantiate(bullethole.name, hit.point+(hit.normal * .01f),Quaternion.FromToRotation(Vector3.back,hit.normal));

            if(hit.transform.gameObject.GetComponent<Health>())
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage",RpcTarget.All,damage);
            }
        }
    }
}
