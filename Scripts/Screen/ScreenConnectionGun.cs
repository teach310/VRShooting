using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UnityEngine.UI;

public class ScreenConnectionGun : ScreenPresenter
{
	[SerializeField] TextMeshProUGUI label;
	[SerializeField] Button retryButton;

	public override IObservable<Unit> Initialize ()
	{
		retryButton.gameObject.SetActive (false);
		label.text = string.Empty;
		SetListener ();
		return base.Initialize ();
	}

	void SetListener ()
	{
		retryButton
			.OnClickAsObservable ()
			.Where (_ => !ConnectionClient.instance.IsJoinedRoom)
			.Subscribe (_ => OnClickRetry ());
	}

	public override UniRx.IObservable<UniRx.Unit> OnEndMoveIn ()
	{
		Observable.FromCoroutine<string>(observer => ConnectionClient.instance.Join(observer))
			.Subscribe(x=>label.text = x, OnConnected)
			.AddTo(this.gameObject);
		return base.OnEndMoveIn ();
	}

	void OnConnected(){
		if (ConnectionClient.instance.IsJoinedRoom)
			ScenePresenter.MoveScreen<ScreenGun> ();
		else
			retryButton.gameObject.SetActive (true);
	}

	void OnClickRetry(){
		retryButton.gameObject.SetActive (false);
		Observable.FromCoroutine<string>(observer => ConnectionClient.instance.TryJoinRoom(observer))
			.Subscribe(x=>label.text = x, OnConnected)
			.AddTo(this.gameObject);
	}
}
