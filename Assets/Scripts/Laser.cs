using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserspeed = 15.0f;

<<<<<<< HEAD
=======
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 1.05f, 0);
    }

    // Update is called once per frame
>>>>>>> 56686b94c8f9145068e5aca2326c6b35c1f53afe
    void Update()
    {
        transform.Translate(Vector3.up * _laserspeed * Time.deltaTime);

        if (transform.position.y >= 7.1f)
        {
<<<<<<< HEAD

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);

=======
            Destroy(this.gameObject);
>>>>>>> 56686b94c8f9145068e5aca2326c6b35c1f53afe
        }

    }

}
