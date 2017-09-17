using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(TPhysicsRaycaster))]
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
		Raycaster.OnPointerEnter
			.Subscribe (x => {
			if (pointer != null)
				pointer.OnPointerEnter (x, true);
		});

		Raycaster.OnPointerExit
					.Subscribe (x => {
			if (pointer != null)
				pointer.OnPointerExit (x);
		});
		Raycaster.OnPointerHover
					.Subscribe (x => {
			if (pointer != null)
				pointer.OnPointerHover (x, true);
		});
	}
}
