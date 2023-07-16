using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedHit : MonoBehaviour
{
    [SerializeField]
    private SpeedEnemy speedEnemy;
    [SerializeField]
    private BoxCollider2D boxCollider;
    void Start()
    {
        speedEnemy = transform.GetComponentInParent<SpeedEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(this.boxCollider);
            speedEnemy.PlayerHit();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.boxCollider);
            speedEnemy.LaserHit();

        }

        if (other.tag == "Missle")
        { 
            Destroy(this.boxCollider);
            speedEnemy.LaserHit();
        }

        if (other.tag == "MegaLaser")
        {
            Destroy(this.boxCollider);
            speedEnemy.MegaHit();
        }
    }
}
