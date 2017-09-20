using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
public class ScreenGun : ScreenPresenter
{
	public Button button;

	public override UniRx.IObservable<UniRx.Unit> Initialize()
	{
		button.OnClickAsObservable()
			.Subscribe(_=>InputHandler.instance.SendTouch())
			.AddTo(this.gameObject);
		return base.Initialize();
	}
}
