# MonaCoreSDK 0.13.0

**Full Changelog**: https://github.com/monaverse/MonaCoreSDK/compare/0.12.0...0.13.0

# MonaCoreSDK 0.12.0

**Full Changelog**: https://github.com/monaverse/MonaCoreSDK/compare/0.11.0...0.12.0

# MonaCoreSDK 0.11.0

**Full Changelog**: https://github.com/monaverse/MonaCoreSDK/compare/0.10.0...0.11.0

# MonaCoreSDK 0.10.0

**Full Changelog**: https://github.com/monaverse/MonaCoreSDK/compare/0.9.0...0.10.0

# MonaCoreSDK 0.9.0

**Full Changelog**: https://github.com/monaverse/MonaCoreSDK/compare/0.8.0...0.9.0

# MonaCoreSDK 0.8.0

**Full Changelog**: https://github.com/monaverse/MonaCoreSDK/compare/0.7.0...0.8.0

# MonaCoreSDK 0.7.0

**Full Changelog**: https://github.com/monaverse/MonaCoreSDK/compare/0.6.0...0.7.0

# MonaCoreSDK 0.6.0

**Full Changelog**: https://github.com/monaverse/MonaCoreSDK/compare/0.5.0...0.6.0

# MonaCoreSDK 0.5.0

**Full Changelog**: https://github.com/monaverse/MonaCoreSDK/compare/0.4.0...0.5.0

# MonaCoreSDK 0.4.0

### Bug Fixes
- Fixed Input lag
- Fixed grounding. now more robust. detects grounding on other objects than ground.
- fixed Rigidbody movement behaving abnormally. Moved movement into fixedupdate context.

# MonaCoreSDK 0.3.0

### Bug Fixes
- adjust non kinematic movement to rigidbody.position vs rigidbody.Move for kinematic
- null check on mock player
- null check on rigidbody

# MonaCoreSDK 0.2.0

### Bug Fixes
- Fixes to `MonaBody`

### New Code
- Allow firing of  `OnPause()` / `OnResume()` events on `IMonaBody`
- Exposed a number of new methods in `MonaBody` for use by the brains sdk and visuals cripting. Full list in the `IMonaBody` interface
- Added `DontGoThroughThings` to NetworkedRigidbody `MonaBody` objects to protect against collider passthrough with fast moving objects
- Mona Body Variables now has EasyUI support. Numbers can be exposed as UI elements with constraints and on screen and in world positioning.
- Allow for remote animation triggers over the network.
- Introduced Asset system for exposing Audio / Animations / Avatars and Prefabs to the Mona Brains SDK and managing them in the Unity Editor via scriptable objects


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
