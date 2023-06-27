using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3.0f;

    [SerializeField]
    private int powerupID;



    // Start is called before the first frame update
    void Start()
    {
 
        transform.position = new Vector3(Random.Range(-9.5f, 9.5f), 9.5f, 0);

    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        if (transform.position.y <= -7.1f)
        {
            Destroy(this.gameObject);
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
                    default:
                        Debug.Log("Default Value");
                        break;
                }
 
            }
            Destroy(this.gameObject);
        }
    }

}
