using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool bulletPoolInstance;
    [SerializeField]
    private GameObject _pooledBullet;
    [SerializeField]
    private GameObject _bulletContainer;
    private bool _noMoreBullets = true;

    private List<GameObject> bullets;

    private void Awake()
    {
        bulletPoolInstance = this;
    }

    void Start()
    {
        bullets = new List<GameObject>();
    }

    public GameObject GetBullet()
    {
        if (bullets.Count > 0)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if(!bullets[i].activeInHierarchy)
                {
                    return bullets[i];
                }

            }
        }

        if (_noMoreBullets)
        {
            GameObject bul = Instantiate(_pooledBullet);
            bul.transform.parent = _bulletContainer.transform;
            bul.SetActive(false);
            bullets.Add(bul);
            return bul;
        }
        return null;
    }
}
