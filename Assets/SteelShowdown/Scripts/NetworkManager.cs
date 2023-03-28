using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private Text feedbackText;
    [SerializeField]
    private byte maxPlayersPerRoom = 2;
    [SerializeField]
    InputField playerNameInput;

    bool isConnecting;
    string gameVersion = "1";

    const string namePrefKey = "PLAYER_NAME";

    void Awake()
    {

        PhotonNetwork.AutomaticallySyncScene = true;

    }
    private void Start()
    {
        string name = "";
        if(playerNameInput!=null)
        {
            if(PlayerPrefs.HasKey(namePrefKey))
            {
                name = PlayerPrefs.GetString(namePrefKey);
                playerNameInput.text = name;
            }
        }
    }
    public void setPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(namePrefKey, value);
    }


    public void Connect()
    {        
        feedbackText.text = "";
        isConnecting = true;

        controlPanel.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            LogFeedback("Joining Room...");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {

            LogFeedback("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
        }
    }

    void LogFeedback(string message)
    {
        if (feedbackText == null)
            return;
        feedbackText.text += System.Environment.NewLine + message;
    }
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            LogFeedback("Connected to server successfully.");
            PhotonNetwork.JoinRandomRoom();
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        LogFeedback("Creating new session");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        LogFeedback("Disconnected:  " + cause);
        isConnecting = false;
        controlPanel.SetActive(true);
    }
    public override void OnJoinedRoom()
    {
        LogFeedback("Joined session with  " + PhotonNetwork.CurrentRoom.PlayerCount + " Player");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            PhotonNetwork.LoadLevel("Map01");

        
    }
}