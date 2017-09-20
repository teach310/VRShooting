using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(PhotonView))]
public class InputHandler : Photon.MonoBehaviour {

    static protected InputHandler m_Instance;
    static public InputHandler instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject o = new GameObject("InputHandler");
                DontDestroyOnLoad(o);
                m_Instance = o.AddComponent<InputHandler>();
            }

            return m_Instance;
        }
    }

    Subject<Unit> onTouch = new Subject<Unit>();
    public IObservable<Unit> OnTouch{
        get{return onTouch;}
    }

    public void SendTouch(){
        photonView.RPC("DoTouch", PhotonTargets.All);
        PhotonNetwork.SendOutgoingCommands();
    }


	#if UNITY_EDITOR
	// デバッグ用
	public void DoTouchInEditor(){
		onTouch.OnNext(Unit.Default);
	}
	#endif

    [PunRPC]
    void DoTouch(){
        if(PhotonNetwork.isMasterClient){
            instance.onTouch.OnNext(Unit.Default);
        }
    }
}
