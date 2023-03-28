using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviour
{
    static public GameController Instance;
    [SerializeField]
    private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if (playerPrefab == null)
        { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

            Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {


            if (TankController.LocalPlayerInstance == null)
            {


                Vector3 spawnLocation = new Vector3(0, PhotonNetwork.CurrentRoom.PlayerCount*5, 0);
                PhotonNetwork.Instantiate(this.playerPrefab.name, spawnLocation, Quaternion.identity, 0);
            }
            else
            {

      
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [PunRPC]
    public void sendRespawn(int respawnObjectID)
    {
        //PhotonNetwork.Destroy(respawnObject);
        GameObject respawnObject = PhotonView.Find(respawnObjectID).gameObject;
        respawnObject.SetActive(false);
        StartCoroutine(respawnAfterTime(2f, respawnObject));
    }
    public void respawn(int respawnObjectID)
    {
        Debug.Log("Initial Sent");
        PhotonView photonView = PhotonView.Get(gameObject.GetComponent<PhotonView>());
        photonView.RPC("sendRespawn", RpcTarget.All, respawnObjectID);
    }
    public IEnumerator respawnAfterTime(float waitTime,GameObject respawnObject)
    {
        Debug.Log("INSIDE COROUTINE");
        yield return new WaitForSeconds(waitTime);
        respawnObject.transform.position = new Vector3(0, 0, 0);
        respawnObject.SetActive(true);
        Debug.Log("OUTSIDE COROUTINE");

    }
}
