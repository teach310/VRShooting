using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent (typeof(TPhysicsRaycaster))]
public class Gun : MonoBehaviour
{
	
	[SerializeField] TBasePointer pointer;

	TPhysicsRaycaster raycaster;

	TPhysicsRaycaster Raycaster {
		get {
			if (raycaster == null)
				raycaster = this.GetComponent<TPhysicsRaycaster> ();
			return raycaster;
		}
	}

	void Start ()
	{
		Raycaster
			.OnPointerEnter
			.Where(x=>x.gameObject.GetComponent<GunEventTriigger> () != null)
			.Subscribe (x => {
			if (pointer != null) {
				pointer.OnPointerEnter (x, true);
			}
			var trigger = x.gameObject.GetComponent<GunEventTriigger> ();
			trigger.DoPointerEnter ();
		});

		Raycaster
			.OnPointerExit
			.Where(x=>x.GetComponent<GunEventTriigger> () != null)
			.Subscribe (x => {
			if (pointer != null)
				pointer.OnPointerExit (x);
			var trigger = x.GetComponent<GunEventTriigger> ();
			trigger.DoPointerExit ();
		});
		
		Raycaster
			.OnPointerHover
			.Where(x=>x.gameObject.GetComponent<GunEventTriigger> () != null)
			.Subscribe (x => {
			if (pointer != null)
				pointer.OnPointerHover (x, true);
			var trigger = x.gameObject.GetComponent<GunEventTriigger> ();
			trigger.DoPointerHover ();
		});

		InputHandler.instance
			.OnTouch
			.Where (_ => Raycaster.IsPointerHover)
			.Subscribe (_ => {
			var trigger = Raycaster.GetResult ().gameObject.GetComponent<GunEventTriigger> ();
			if (trigger != null)
				trigger.DoShot ();
			}).AddTo(this.gameObject);

		#if UNITY_EDITOR
		this.UpdateAsObservable()
			.Where(_=>Input.GetMouseButtonDown(0))
			.Subscribe(_=>InputHandler.instance.DoTouchInEditor());
		#endif
	}
}
