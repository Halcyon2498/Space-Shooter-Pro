using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet1 : MonoBehaviour
{
    private Vector3 _bulletDir;
    private float _bulletSpeed;


    private void OnEnable()
    {
        Invoke("OnDestroy", 5f);
    }

    void Start()
    {
        _bulletSpeed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_bulletDir * _bulletSpeed * Time.deltaTime);
    }

    public void SetBulDirection(Vector3 dir)
    {
        _bulletDir = dir;
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
