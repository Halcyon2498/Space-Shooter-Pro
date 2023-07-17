using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedHit : MonoBehaviour
{
    [SerializeField]
    private SpeedEnemy _speedEnemy;
    [SerializeField]
    private BoxCollider2D _boxCollider;
    void Start()
    {
        _speedEnemy = transform.GetComponentInParent<SpeedEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(this._boxCollider);
            _speedEnemy.PlayerHit();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this._boxCollider);
            _speedEnemy.LaserHit();
        }

        if (other.tag == "MegaLaser")
        {
            Destroy(this._boxCollider);
            _speedEnemy.MegaHit();
        }
    }
}
