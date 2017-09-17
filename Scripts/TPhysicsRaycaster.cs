using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

//public class TRay{
//	Ray ray;
//
//	public TRay(Vector3 origin, Vector3 direction){
//		ray = new Ray (origin, direction);
//	}
//
//
//	class CastResult{
//		public RaycastHit Hit{ get;}
//		public bool IsHit{ get;}
//		public CastResult(RaycastHit hit, bool isHit){
//			Hit = hit;
//			IsHit = isHit;
//		}
//	}
//
//	public IObservable<CastResult> Cast(float maxDistance = Mathf.Infinity){
//		RaycastHit hit;
//		Observable.Return<CastResult> (
//			if(Physics.Raycast(ray, maxDistance){
//			}else{
//				
//			}
//		);
//	}
//}

public class TPhysicsRaycaster : MonoBehaviour {

	#if UNITY_EDITOR
	public bool drawDebugRays = false;
	#endif


	[SerializeField] float distance;
	[SerializeField] Ray lastRay;
	[SerializeField] RaycastHit lastRaycastHit;

	BoolReactiveProperty hitRp = new BoolReactiveProperty(false);

	public IObservable<RaycastResult> OnPointerEnter{
		get{ return hitRp
				.Where (x => x)
				.Select (x => GetResult ());
			}
	}

	public IObservable<GameObject> OnPointerExit{
		get{
			return hitRp
				.Pairwise()
				.Where (x => !x.Current && x.Previous)
				.Select (x => lastRaycastHit.collider.gameObject);
		}
	}

	public IObservable<RaycastResult> OnPointerHover{
		get{
			return this.UpdateAsObservable ()
				.Where (_ => hitRp.Value)
				.Select (x => GetResult ());
		}
	}
	void Start(){
		this.UpdateAsObservable ()
			.Subscribe (_ => {
				Cast();
				MaybeDrawDebugRaysForEditor(Color.red);
			});
	}

	void SetListener(){
//		hitRp
//			.Where(x=>x)
//			.Subscribe (_ => {
//				if(pointer != null)
//					pointer.OnPointerEnter(GetResult(), true);
//			});
//		hitRp
//			.Where (x => !x)
//			.Skip(1) // 初期値はスキップ
//			.Subscribe (_ => {
//				if(pointer != null)
//					pointer.OnPointerExit(lastRaycastHit.collider.gameObject);
//		});
//
//		this.UpdateAsObservable()
//			.Where(_=>hitRp.Value)
//			.Subscribe(_=>{
//				if(pointer != null)
//					pointer.OnPointerHover(GetResult(), true);
//			});

		this.UpdateAsObservable ()
			.Subscribe (_ => {
				Cast();
				MaybeDrawDebugRaysForEditor(Color.red);
		});
	}

	protected RaycastResult GetResult(){
		return new RaycastResult () {
			gameObject = lastRaycastHit.collider.gameObject,
			module = null,
			worldPosition = lastRaycastHit.point,
			worldNormal = lastRaycastHit.normal
		};
	}

	void Cast(){
		lastRay = new Ray (this.transform.position, this.transform.forward);
		RaycastHit hit;
		bool isHit = Physics.Raycast (lastRay, out hit, distance);
		if (isHit) {
			lastRaycastHit = hit;
		}
		hitRp.Value = isHit;
	}

	private void MaybeDrawDebugRaysForEditor(Color color) {
		
		#if UNITY_EDITOR
		if (drawDebugRays) {
			Debug.DrawRay(lastRay.origin, lastRay.direction * distance, color);
		}
		#endif  // UNITY_EDITOR
	}

}
