using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartLaser : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Rigidbody2D _rb;
   
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("Player is null");
        }
        
    }

    void Update()
    {
        if (_player != null)
        {
            Vector3 aim = (_player.transform.position - transform.position).normalized;
            _rb.AddForce(aim * 5f);
            Destroy(this.gameObject, 2.5f);
        }
    }

    private void OnBecameInvisible()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }

        Destroy(this.gameObject);
    }

}
