using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEnemy : MonoBehaviour
{

    [SerializeField]
    private float _turnDegrees = 20f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    [SerializeField]
    Transform[] waypoints;
    [SerializeField]
    float movespeed = 10f;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private AudioSource _audioSource;
    int waypointIndex = 0;


    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_player == null)
        {
            Debug.Log("Player is null");
        }

        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _turnDegrees) * Time.deltaTime);
        Move();
        
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, movespeed * Time.deltaTime);

        if (transform.position == waypoints[waypointIndex].transform.position)
        {
            waypointIndex += 1;
        }

        if (waypointIndex == waypoints.Length)
        {
            Destroy(this.gameObject);
            _spawnManager._enemiesLeft--;
        }
    }

    public void DodgeLaser()
    {
        int LeftorRight = Random.Range(0, 2);
        if (LeftorRight == 0)
        {
            StartCoroutine(DodgeLeft());
        }
        else
        {
            StartCoroutine(DodgeRight());
        }
    }

    IEnumerator DodgeLeft()
    {
        Vector2 currentPos = transform.position;
        Vector2 distance = new Vector2(transform.position.x - 2, transform.position.y);
        float time = 0f;

        while(time < 1)
        {
            time += Time.deltaTime * 3f;
            transform.position = Vector2.Lerp(currentPos, distance, time);
            yield return null;
        }
    }

    IEnumerator DodgeRight()
    {
        Vector2 currentPos = transform.position;
        Vector2 distance = new Vector2(transform.position.x + 2, transform.position.y);
        float time = 0f;

        while (time < 1)
        {
            time += Time.deltaTime * 3f;
            transform.position = Vector2.Lerp(currentPos, distance, time);
            yield return null;
        }
    }

    public void PlayerHit()
    {
        _player.Damage();
        EnemyDestroySequence();
    }

    public void LaserHit()
    {
        _player.addScore(500);
        EnemyDestroySequence();
    }

    public void MegaHit()
    {
        _player.addScore(500);
        EnemyDestroySequence();
    }


    private void EnemyDestroySequence()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _spawnManager._enemiesLeft--;
        movespeed = 0f;
        Destroy(this.gameObject);
    }
}
