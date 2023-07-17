using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private RamEnemy _ramEnemy;
    [SerializeField]
    private BoxCollider2D _boxCollider;
    void Start()
    {
        _ramEnemy = transform.GetComponentInParent<RamEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(this._boxCollider);
            _ramEnemy.PlayerHit();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this._boxCollider);
            _ramEnemy.LaserHit();

        }

        if (other.tag == "MegaLaser")
        {
            Destroy(this._boxCollider);
            _ramEnemy.MegaHit();
        }
    }
}
