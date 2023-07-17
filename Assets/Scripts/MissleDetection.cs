using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleDetection : MonoBehaviour
{
    [SerializeField]
    private HomingMissle _missle;
    void Start()
    {
        _missle = transform.GetComponentInParent<HomingMissle>();
        if (_missle == null)
        {
            Debug.Log("Parent not found");
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            _missle.EnemyFound();
        }
    }
}
