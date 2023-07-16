using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleDetection : MonoBehaviour
{
    [SerializeField]
    private HomingMissle missle;
    void Start()
    {
        missle = transform.GetComponentInParent<HomingMissle>();
        if (missle == null)
        {
            Debug.Log("Parent not found");
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            missle.EnemyFound();
        }
    }
}
