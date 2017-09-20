using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ConnectionClient : Photon.MonoBehaviour {

	static protected ConnectionClient m_Instance;
	static public ConnectionClient instance
	{
		get
		{
			if (m_Instance == null)
			{
				GameObject o = new GameObject("Connection");
				DontDestroyOnLoad(o);
				m_Instance = o.AddComponent<ConnectionClient>();
			}

			return m_Instance;
		}
	}

	// フェーズ
	bool isJoinedLobby = false;
	bool onFinishRandomJoin = false;
	bool isJoinedRoom = false;
	public bool IsJoinedRoom{
		get{ return isJoinedRoom; }
	}

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
		yield return TryJoinRoom (observer);
	}

	public IEnumerator TryJoinRoom(IObserver<string> observer){
		observer.OnNext("Connecting..");
		PhotonNetwork.JoinRandomRoom();
		// 接続結果を待つ
		yield return new WaitUntil(()=> onFinishRandomJoin);
		if (isJoinedRoom)
			observer.OnNext ("Joined Room");
		else
			observer.OnNext ("Gear not found");

		observer.OnCompleted();
	}

	void OnPhotonRandomJoinFailed()
	{
		onFinishRandomJoin = true;
	}

	void OnJoinedRoom()
	{
		onFinishRandomJoin = true;
		isJoinedRoom = true;
	}
}
