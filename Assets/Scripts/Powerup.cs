using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 5f;
    [SerializeField]
    private float _powerupVacuum = 7f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private int powerupID;
    public Transform _playerTransform;

    void Start()
    {

        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _explosionPrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }


    void Update()
    {

        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        if (transform.position.y <= -7.5f)
        {
            Destroy(this.gameObject);
        }

        if (Input.GetKey(KeyCode.C))
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, _powerupVacuum * Time.deltaTime);
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {              

                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoActive();
                        break;
                    case 4:
                        player.LivesActive();
                        break;
                    case 5:
                        player.MegaLaserActive();
                        break;
                    case 6:
                        player.FreezeActive();
                        break;
                    case 7:
                        player.RefillBarrage();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
 
            }
            Destroy(this.gameObject);
        }

        if(other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
