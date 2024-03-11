using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Weapon : MonoBehaviour
{

    [Header("Debug stuff")]
    public GameObject weapon;
    public GameObject bullethole;
    public Camera camera;

    PlayerManager playerManager;
    PhotonView PV;


    [Header("Weapon Settings")] // temp here need to make a class for diffrent types of weapons
    public int damage;
    public float fireRate;
    private float nextFire;

    [Header("Amou")] // temp
    public int mag = 5;
    public int ammo = 30;
    public int magSize = 30;

    [Header("UI")]
    public TMP_Text ammoText;
    public TMP_Text magText;

    [Header("Animation")]
    public Animation animation;
    public AnimationClip reload;

    [Header("Recoil Settings")]
    [Range(0,1)] public float RecoilPercent = 0.3f;
    [Range(0, 2)] public float recoverPercent = 0.7f;
    [Space]
    public float recoilUp = 0.2f;
    public float recoilBack = 0.3f;

    [Header("Sway movement Settings")]
    public float swayClamp = 0.09f;
    public float smoothing = 3f;

    private Vector3 weaponOrigin;

    // Recoil Var
    private Vector3 recoilVelocity = Vector3.zero;

    private float recoilLength;
    private float recoverLength;

    private bool recoiling;
    private bool recovering;

    private void Start()
    {

        weaponOrigin = weapon.transform.localPosition;
        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;

        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon != null)
        {
            WeaponSwaying();

            if (nextFire > 0)
                nextFire -= Time.deltaTime;

            if ((Input.GetButton("Fire1") && nextFire <= 0) && ammo > 0 && animation.IsPlaying(reload.name) == false)
            {
                nextFire = 1 / fireRate;
                ammo--;
                ammoText.text = ammo.ToString();
                Fire();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            if (recoiling)
                Recoil();

            if (recovering)
                Recovering();
        }
        
    }

    void Fire()
    {
        recoiling = true;
        recovering = false;

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        RaycastHit hit;

        if(Physics.Raycast(ray.origin,ray.direction, out hit,100f))
        {
            if(hit.transform.gameObject.GetComponent<Health>())
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage",RpcTarget.All, damage, PV.Owner);
            }else
            {
                //update: make bullet impact plus effect
                PhotonNetwork.Instantiate(bullethole.name, hit.point + (hit.normal * .01f), Quaternion.FromToRotation(Vector3.back, hit.normal));
            }
        }
    }


    void Reload()
    {
        animation.Play(reload.name);

        if(magSize > 0)
        {
            mag--;
            ammo = magSize;
        }

        magText.text = (mag * magSize).ToString();
        ammoText.text = ammo.ToString();
    }

    void Recoil()
    {
        Vector3 finalPosition = new Vector3(weaponOrigin.x, weaponOrigin.y + recoilUp, weaponOrigin.z - recoilBack);

        weapon.transform.localPosition = Vector3.SmoothDamp(weapon.transform.localPosition, finalPosition,ref recoilVelocity,recoilLength);

        if(weapon.transform.localPosition ==  finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }

    void Recovering()
    {
        Vector3 finalPosition = weaponOrigin;

        weapon.transform.localPosition = Vector3.SmoothDamp(weapon.transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);

        if (weapon.transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }

    void WeaponSwaying()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        input.x = Mathf.Clamp(input.x, -swayClamp, swayClamp);
        input.y = Mathf.Clamp(input.y, -swayClamp, swayClamp);

        Vector3 target = new Vector3(-input.x, -input.y, 0);
        
        weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, target + weaponOrigin, Time.deltaTime * smoothing);
    }
}
