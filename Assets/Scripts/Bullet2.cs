using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    private float _laserspeed = 10.0f;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void Update()
    {

        transform.Translate(Vector3.down * _laserspeed * Time.deltaTime);

        if (transform.position.y <= -9f)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);

        }
    }
}
