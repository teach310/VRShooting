using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunEventTriigger : MonoBehaviour {

	public UnityEvent onPointerEnter = new UnityEvent();
	public UnityEvent onPointerExit = new UnityEvent();
	public UnityEvent onPointerHover = new UnityEvent();
	public UnityEvent onShot = new UnityEvent();

	public void DoPointerEnter(){
		onPointerEnter.Invoke ();
	}

	public void DoPointerExit(){
		onPointerExit.Invoke ();
	}

	public void DoPointerHover(){
		onPointerHover.Invoke ();
	}

	public void DoShot(){
		onShot.Invoke ();
	}
}
