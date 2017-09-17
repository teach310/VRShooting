using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TBasePointer : MonoBehaviour {

	/// Returns the transform that represents this pointer.
	/// It is used by GvrBasePointerRaycaster as the origin of the ray.
	public virtual Transform PointerTransform {
		get {
			return transform;
		}
	}

	/// Returns the max distance from the pointer that raycast hits will be detected.
	public abstract float MaxPointerDistance { get; }

	/// Called when the pointer is facing a valid GameObject. This can be a 3D
	/// or UI element.
	///
	/// **raycastResult** is the hit detection result for the object being pointed at.
	/// **isInteractive** is true if the object being pointed at is interactive.
	public abstract void OnPointerEnter(RaycastResult raycastResult, bool isInteractive);

	/// Called every frame the user is still pointing at a valid GameObject. This
	/// can be a 3D or UI element.
	///
	/// **raycastResult** is the hit detection result for the object being pointed at.
	/// **isInteractive** is true if the object being pointed at is interactive.
	public abstract void OnPointerHover(RaycastResult raycastResultResult, bool isInteractive);

	/// Called when the pointer no longer faces an object previously
	/// intersected with a ray projected from the camera.
	/// This is also called just before **OnInputModuleDisabled**
	/// previousObject will be null in this case.
	///
	/// **previousObject** is the object that was being pointed at the previous frame.
	public abstract void OnPointerExit(GameObject previousObject);

	/// Called when a click is initiated.
	public abstract void OnPointerClickDown();

	/// Called when click is finished.
	public abstract void OnPointerClickUp();

	/// Return the radius of the pointer. It is used by GvrPointerPhysicsRaycaster when
	/// searching for valid pointer targets. If a radius is 0, then a ray is used to find
	/// a valid pointer target. Otherwise it will use a SphereCast.
	/// The *enterRadius* is used for finding new targets while the *exitRadius*
	/// is used to see if you are still nearby the object currently pointed at
	/// to avoid a flickering effect when just at the border of the intersection.
	///
	/// NOTE: This is only works with GvrPointerPhysicsRaycaster. To use it with uGUI,
	/// add 3D colliders to your canvas elements.
	public abstract void GetPointerRadius(out float enterRadius, out float exitRadius);

	protected virtual void Start(){
	}
}
