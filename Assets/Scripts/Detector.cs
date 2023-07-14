using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField]
    private RamEnemy ramEnemy;

    void Start()
    {
        ramEnemy = transform.GetComponentInParent<RamEnemy>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ramEnemy.ChasePlayer();
        }
    }
}
