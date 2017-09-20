using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ConnectionMasterClient : Photon.MonoBehaviour {

	static protected ConnectionMasterClient m_Instance;
	static public ConnectionMasterClient instance
	{
		get
		{
			if (m_Instance == null)
			{
				GameObject o = new GameObject("Connection");
				DontDestroyOnLoad(o);
				m_Instance = o.AddComponent<ConnectionMasterClient>();
			}

			return m_Instance;
		}
	}

	// フェーズ
	bool isJoinedLobby = false;
	bool isJoinedRoom = false;

	Room room;
	public static readonly int maxPlayers = 2;

	void Start(){
		PhotonNetwork.ConnectUsingSettings ("v0.1");
	}

	void OnJoinedLobby(){
		isJoinedLobby = true;
	}

	// 2人そろうまで待つ
	public IEnumerator Join(IObserver<string> observer){
		observer.OnNext ("Ready...");

		if (PhotonNetwork.insideLobby)
			isJoinedLobby = true;
		// ロビーに接続するまで待つ
		yield return new WaitUntil(()=> isJoinedLobby);
		observer.OnNext("Connecting..");
		CreateRoom ();

		// ルームに接続するまで待つ
		yield return new WaitUntil(()=> isJoinedRoom);
		observer.OnNext("Joined Room");

		// 2人そろうまで待つ
		yield return new WaitUntil(()=>room.PlayerCount == 2);
		observer.OnNext("Connected");
		observer.OnCompleted();
	}

	void CreateRoom(){
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsVisible = true;
		roomOptions.IsOpen = true;
		roomOptions.MaxPlayers = 2;
		PhotonNetwork.CreateRoom (null, roomOptions, null);
	}

	void OnJoinedRoom()
	{
		room = PhotonNetwork.room;
		isJoinedRoom = true;
	}
}
