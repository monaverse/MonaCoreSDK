# MonaCoreSDK 0.1.0

This first release of Mona Core SDK establishes a foundation for Networked Objects in Monaverse Spaces

### Bug Fixes
- Fixes bug in Mona Reactors where players joining late did not get the current state of a mona reactor.

### New Code

**Mona Bodies**
Introducing <MonaBody> gameobject behavior.  These behaviors can be attached to any <GameObject> in the Space scene.
- <MonaBody> can be set to sync the <Transform> of a <GameObject> over the network, or alternatively, the <Rigidbody> of a <GameObject> over the network.
- A<MonaBody> can now have one or more <MonaTag>s assigned to them for use in the MonaBrainSDK

**Mona State**
- Mona State allows for properties assigned to the mona body to be synced over the network.
- Supported property types are Int|Float|String|Bool|Vector2|Vector3