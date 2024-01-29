# MonaCoreSDK 0.2.0

### Bug Fixes
- `SetColor(Color color)` on `MonaBody` now sets color on all child `Renderer` objects of a `MonaBodyBase`.
- Move NetworkSpawner event listener to `Awake()` from `Start()` on `MonaBody`

### New Code
- Allow firing of  `OnPause()` / `OnResume()` events on `IMonaBody`
- Expose `SetActive(bool active)` to `IMonaBody`
- Add `SetVisible(bool visible)`/`GetVisible()` to `IMonaBody`
- Allow `GameObject` with `MockEventBus` to simulate local player join
- Add `Children()` method to `IMonaBody` to return child `IMonaBody`
- Add `GetParent()` method to `IMonaBody`
- Added `DontGoThroughThings` to NetworkedRigidbody `MonaBody` objects to protect against collider passthrough with fast moving objects
- Added `ResetLayer()` to `IMonaBody` to reset a layer to its default layer ("PhysicsGroupA" for NetworkedRigidbodies and "Default" for the rest)
- Added `ApplyForce(Vector3 force, ForceMode forceMode)` to `IMonaBody`
- Added `SetDragType(DragType dragType)` to `IMonaBody`
- Added `SetDrag(float drag)` to `IMonaBody`
- Added `SetAngularDrag(float drag)` to `IMonaBody`
- Added `SetVelocity(Vector3 velocity)` to `IMonaBody`
- Added `SetAngularVelocity(Vector3 velocity)` to `IMonaBody`
- Added `SetFriction(float friction)` to `IMonaBody`
- Added `SetBounce(float bounce)` to `IMonaBody`
- Added `SetUseGravity(bool useGravity)` to `IMonaBody`
- Added `SetOnlyApplyDragWhenGrounded(bool apply)` to `IMonaBody`

# MonaCoreSDK 0.1.0

This first release of Mona Core SDK establishes a foundation for Networked Objects in Monaverse Spaces

### Bug Fixes
- Fixes bug in Mona Reactors where players joining late did not get the current state of a mona reactor.

**Mona Bodies**
Introducing <MonaBody> gameobject behavior.  These behaviors can be attached to any <GameObject> in the Space scene.
- <MonaBody> can be set to sync the <Transform> of a <GameObject> over the network, or alternatively, the <Rigidbody> of a <GameObject> over the network.
- A<MonaBody> can now have one or more <MonaTag>s assigned to them for use in the MonaBrainSDK

**Mona State**
- Mona State allows for properties assigned to the mona body to be synced over the network.
- Supported property types are Int|Float|String|Bool|Vector2|Vector3