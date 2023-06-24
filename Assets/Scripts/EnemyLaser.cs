using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _laserspeed = 10.0f;

    void Update()
    {
        transform.Translate(Vector3.down * _laserspeed * Time.deltaTime);

        if (transform.position.y <= -7.1f)
        {

            Destroy(this.gameObject);

        }

    }


}
