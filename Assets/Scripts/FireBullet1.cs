using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet1 : MonoBehaviour
{
    [SerializeField]
    private int bulletsAmount = 10;
    

    [SerializeField]
    private float startAngle = 90f, endAngle = 270f;

    void Start()
    {
        InvokeRepeating("Fire", 0f, 2f);
    }

    private void Fire()
    {
        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle;

        for(int i = 0; i < bulletsAmount + 1; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BulletPool.bulletPoolInstance.GetBullet();
            if (bul != null)
            {
                bul.transform.position = transform.position;
                bul.transform.rotation = transform.rotation;
                bul.SetActive(true);
                bul.GetComponent<BossBullet1>().SetBulDirection(bulDir);
                angle += angleStep;
            }
                     
        }
    }
}
