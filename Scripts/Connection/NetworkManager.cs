using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ルームへの参加と，情報の保存
/// </summary>
public class NetworkManager : Photon.MonoBehaviour {

    static protected NetworkManager m_Instance;
    static public NetworkManager instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject o = new GameObject("NetworkManager");
                DontDestroyOnLoad(o);
                m_Instance = o.AddComponent<NetworkManager>();
            }

            return m_Instance;
        }
    }


    Room room;

    // フェーズ 
    // 慣れていないため使わなかったがTorisoupさんのPhotonRxを使ったほうがいい
    bool isJoinedLobby = false;
    bool isJoinedRoom = false;

    public static readonly int maxPlayers = 2;

    public void Start(){
        PhotonNetwork.ConnectUsingSettings("v0.1");
    }

    void OnJoinedLobby(){
        isJoinedLobby = true;
    }

    // 2人そろうまで待つ
    public IEnumerator Join(IObserver<string> observer){
        observer.OnNext("Ready..");

        // ロビーに接続するまで待つ
        yield return new WaitUntil(()=> isJoinedLobby);
        observer.OnNext("Connecting..");
        PhotonNetwork.JoinRandomRoom();

        // ルームに接続するまで待つ
        yield return new WaitUntil(()=> isJoinedRoom);
        observer.OnNext("Joined Room");
        // 2人そろうまで待つ
        yield return new WaitUntil(()=>room.PlayerCount == 2);
        observer.OnNext(string.Format("Connected as {0}", PhotonNetwork.isMasterClient ? "MasterClient" : "Client"));
        observer.OnCompleted();
    }

    void OnPhotonRandomJoinFailed()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom ("Online Typing", roomOptions, null);
    }

    void OnJoinedRoom()
    {
        room = PhotonNetwork.room;
        isJoinedRoom = true;
    }
}
