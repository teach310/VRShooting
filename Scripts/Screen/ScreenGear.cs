using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

public class ScreenGear : ScreenPresenter
{
	public override UniRx.IObservable<UniRx.Unit> Initialize ()
	{
		SetListener ();
		return base.Initialize ();
	}

	void SetListener(){
		InputHandler.instance.OnTouch.Subscribe (_ => {
			SceneManager.LoadScene ("Main");
		}).AddTo(this.gameObject);
	}
}
