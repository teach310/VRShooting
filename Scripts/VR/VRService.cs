using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public static class VRService{

	public static void SetDefaultUI(){
		VRSettings.enabled = false;
	}

	public static void SetCardboardUI(){
		// カードボードをOnにする
		CoroutineHandler.StartStaticCoroutine(LoadCardBoard());
	}

	static IEnumerator LoadCardBoard(){
		VRSettings.LoadDeviceByName("cardboard");
		yield return null;
		VRSettings.enabled = true;
	}
}
