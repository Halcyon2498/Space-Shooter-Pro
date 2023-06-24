using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserspeed = 15.0f;

    void Update()
    {
        transform.Translate(Vector3.up * _laserspeed * Time.deltaTime);

        if (transform.position.y >= 7.1f)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);

        }

    }

}
