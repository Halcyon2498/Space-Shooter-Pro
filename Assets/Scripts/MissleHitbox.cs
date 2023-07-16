using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleHitbox : MonoBehaviour
{
    [SerializeField]
    private HomingMissle missle;
    [SerializeField]
    private BoxCollider2D boxCollider;

    void Start()
    {
        missle = transform.GetComponentInParent<HomingMissle>();
        if(missle == null)
        {
            Debug.Log("Parent not found");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(this.boxCollider);
            missle.OnImpact();
        }
    }
}
