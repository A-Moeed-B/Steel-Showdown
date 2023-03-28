using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;
using System;

public class TankController : MonoBehaviourPunCallbacks, IPunObservable
{

    Rigidbody2D rigidbody;
    float verticalMovement;
    float horizontalMovement;
    [SerializeField]
    private float rotationalSpeed;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject UIPrefab;
    public static GameObject LocalPlayerInstance;
    [SerializeField]
    private GameObject bulletObject;
    [SerializeField]
    private Transform spawnPoint;
    public int score;
    Text scoreText;
    // Start is called before the first frame update
    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }
        DontDestroyOnLoad(gameObject);
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        if(this.UIPrefab!=null)
        {
            GameObject uiInstance = Instantiate(this.UIPrefab);
            uiInstance.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
    }
    private void FixedUpdate()
    {
        if(photonView.IsMine)
        {
            Vector3 direction = transform.right * verticalMovement * speed;
            rigidbody.AddForce(direction);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            verticalMovement = Input.GetAxis("Vertical");
            horizontalMovement = -Input.GetAxis("Horizontal");
            transform.Rotate(new Vector3(0, 0, horizontalMovement * rotationalSpeed*Time.deltaTime));
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject bullet=PhotonNetwork.Instantiate(bulletObject.name, spawnPoint.position, spawnPoint.rotation, 0);
                bullet.GetComponent<Bullet>().parent = this;
            }
        }
    }
    public void increaseScore()
    {
        score++;
        scoreText.text = "Your Score: " + score;
        Debug.Log("Score: " + score);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(this.score);
        }
        else
        {
            this.score = (int)stream.ReceiveNext();
        }
    }
   
}
