using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleHitbox : MonoBehaviour
{
    [SerializeField]
    private HomingMissle _missle;
    [SerializeField]
    private BoxCollider2D _boxCollider;

    void Start()
    {
        _missle = transform.GetComponentInParent<HomingMissle>();
        if(_missle == null)
        {
            Debug.Log("Parent not found");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(this._boxCollider);
            _missle.OnImpact();
        }
    }
}
