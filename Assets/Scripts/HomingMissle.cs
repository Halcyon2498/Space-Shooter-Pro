using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    private GameObject _enemy;
    private int _speed = 10;
    public bool enemyFound = false;
    [SerializeField]
    private GameObject hitbox;
    [SerializeField]
    private GameObject detector;
    [SerializeField]
    private GameObject _explosionPrefab;
    private bool isChasing = false;


    void Start()
    {
   
    }

    void Update()
    {

        if (_enemy == null)
        {
            _enemy = FindClosestEnemy();
        }
        if (_enemy != null && isChasing == true)
        {
            ChaseEnemy();
        }
        else
        {
            transform.Translate(Vector3.up * (_speed / 2) * Time.deltaTime);
        }
      
        if (transform.position.y > 10f)
        {
            Destroy(this.gameObject);
        }
    }

    
    private GameObject FindClosestEnemy()
    {
        try
        {
            GameObject[] enemies;
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;

            foreach (GameObject enemy in enemies)
            {
                Vector3 distances = enemy.transform.position - position;
                float currentDis = distances.sqrMagnitude;
                if (currentDis < distance)
                {
                    closest = enemy;
                    distance = currentDis;
                }
            }
            return closest;
        }
        catch
        {
            return null;
        }

    }


    private void ChaseEnemy()
    {
        if (isChasing == true)
        {
            _speed = 15;
            transform.position = Vector3.MoveTowards(transform.position, _enemy.transform.position, _speed * Time.deltaTime);
            Vector3 offset = transform.position - _enemy.transform.position;
            transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), offset);
            if(transform.position == _enemy.transform.position)
            {
                OnImpact();
            }
        }


    }

    public void EnemyFound()
    {
        isChasing = true;
    }

    public void OnImpact()
    {
        Destroy(this.gameObject);
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
