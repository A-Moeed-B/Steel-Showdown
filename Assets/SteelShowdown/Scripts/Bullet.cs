using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Bullet : MonoBehaviour
{
    Rigidbody2D rigidbody;
    [SerializeField]
    float bulletVelocity=5;
    int counter = 0;
    [SerializeField]
    int maxHits;
    public TankController parent;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right*bulletVelocity, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            PhotonNetwork.Destroy(collision.gameObject);
            PhotonNetwork.Destroy(gameObject);
            return;
        }
        if(collision.gameObject.tag=="Tank")
        {
            GameObject.Find("GameController").GetComponent<GameController>().respawn(collision.gameObject.GetComponent<PhotonView>().ViewID);
            if(collision.gameObject!=parent)
              parent.increaseScore();
            PhotonNetwork.Destroy(gameObject);
            return;
        }
        if (counter >= maxHits)
        {
            PhotonNetwork.Destroy(gameObject);
            return;
        }
        counter++;
        Debug.Log("Hit");
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-1, .5f), UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 1));
        rigidbody.AddForce((transform.right+randomDirection) * bulletVelocity/1.5f, ForceMode2D.Impulse);

    }
    // Update is called once per frame
    void Update()
    {
    
     
    }
}
