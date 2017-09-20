using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class ScreenConnectionGear : ScreenPresenter
{
	[SerializeField] TextMeshProUGUI label;

	public override UniRx.IObservable<UniRx.Unit> Initialize ()
	{
		label.text = string.Empty;
		return base.Initialize ();
	}

	public override UniRx.IObservable<UniRx.Unit> OnEndMoveIn ()
	{
		Observable.FromCoroutine<string>(observer => ConnectionMasterClient.instance.Join(observer))
			.Subscribe(x=>label.text = x, OnConnected)
			.AddTo(this.gameObject);
		return base.OnEndMoveIn ();
	}

	void OnConnected(){
		ScenePresenter.MoveScreen<ScreenGear> ();
	}

	public override IObservable<Unit> OnBackOut ()
	{
		if (PhotonNetwork.inRoom)
			PhotonNetwork.LeaveRoom ();
		return base.OnBackOut ();
	}
}
