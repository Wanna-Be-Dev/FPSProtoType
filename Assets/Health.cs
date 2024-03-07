using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;

    [Header("UI")]
    public TMP_Text healthText;

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        health -= _damage;
        healthText.text = health.ToString();

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
