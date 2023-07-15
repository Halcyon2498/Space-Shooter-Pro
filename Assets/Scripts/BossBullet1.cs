using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet1 : MonoBehaviour
{
    private Vector3 bulletDir;
    private float bulletSpeed;


    private void OnEnable()
    {
        Invoke("OnDestroy", 5f);
    }

    void Start()
    {
        bulletSpeed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(bulletDir * bulletSpeed * Time.deltaTime);
    }

    public void SetBulDirection(Vector3 dir)
    {
        bulletDir = dir;
    }

    private void OnDestroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
