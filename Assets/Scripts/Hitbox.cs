using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private RamEnemy ramEnemy;
    [SerializeField]
    private BoxCollider2D boxCollider;
    void Start()
    {
        ramEnemy = transform.GetComponentInParent<RamEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(this.boxCollider);
            ramEnemy.PlayerHit();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.boxCollider);
            ramEnemy.LaserHit();

        }

        if (other.tag == "Missle")
        {
            Destroy(this.boxCollider);
            ramEnemy.LaserHit();
        }

        if (other.tag == "MegaLaser")
        {
            Destroy(this.boxCollider);
            ramEnemy.MegaHit();
        }
    }
}
