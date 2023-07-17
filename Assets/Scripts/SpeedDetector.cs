using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDetector : MonoBehaviour
{
    [SerializeField]
    private SpeedEnemy _speedEnemy;
        
    void Start()
    {
        _speedEnemy = transform.GetComponentInParent<SpeedEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _speedEnemy.DodgeLaser();
        }
    }
}
