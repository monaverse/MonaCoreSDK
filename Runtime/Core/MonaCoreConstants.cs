namespace Mona.SDK.Core
{
    public class MonaCoreConstants
    {
        public const string DEFAULT_IPFS_GATEWAY = "https://ipfs.io/ipfs/";
        public const float WAIT_FOR_NETWORK_SPAWNER_TIMEOUT = 30f;

        public const string TAG_PLAYER = "Player";
        public const string TAG_SPACE = "Space";
        public const string TAG_PORTAL = "Portal";
        public const string LAYER_LOCAL_PLAYER = "LocalPlayer";
        public const string LAYER_DEFAULT = "Default";
        public const string LAYER_PHYSICS_GROUP_A = "PhysicsGroupA";
        public const string MONA_TAG_PLAYER_CAMERA = "Camera";

        public const string STATE_CHANGED_EVENT = "OnMonaVariablesChangedEvent";
        public const string STATE_AUTHORITY_CHANGED_EVENT = "OnMonaStateAuthorityChangedEvent";
        public const string TICK_EVENT = "OnTick";
        public const string LATE_TICK_EVENT = "OnLateTick";
        public const string FIXED_TICK_EVENT = "OnFixedTick";
        public const string MONA_BODY_HAS_INPUT_EVENT = "OnMonaBodyHasInputEvent";
        public const string MONA_BODY_FIXED_TICK_EVENT = "OnMonaBodyFixedTickEvent";
        public const string MONA_BODY_ANIMATION_TRIGGERED_EVENT = "OnMonaBodyAnimationTriggeredEvent";
        public const string MONA_BODY_SCALE_CHANGED_EVENT = "OnMonaBodyScaleChangedEvent";
        public const string LOCAL_INPUT_EVENT = "OnMonaLocalInputEvent";
        public const string INPUT_EVENT = "OnMonaInputTickEvent";
        public const string INPUTS_EVENT = "OnMonaInputsTickEvent";
        public const string REGISTER_NETWORK_SETTINGS_EVENT = "OnMonaRegisterNetworkSettignsEvent";
        public const string ON_PLAYER_CHANGE_AVATAR_EVENT = "OnPlayerChangeAvatar";
        public const string ON_CHANGE_SPAWN_EVENT = "OnChangeSpawn";
        public const string ON_CHANGE_AVATAR_EVENT = "OnChangeAvatar";
        public const string ON_CHANGE_SPACE_EVENT = "OnChangeSpace";

        public const string VALUE_CHANGED_EVENT = "OnMonaValueChanged";

        public const string ON_CURSOR_VISIBILITY_CHANGED_EVENT = "OnCursorVisibilityChangedEvent";
        public const string ON_CHANGE_CURSOR_VISIBILITY_EVENT = "OnChangeCursorVisibilityEvent";

        public const string MONA_BODY_RIGIDBODY_CHANGED_EVENT = "OnMonaBodyRigidBodyChangedEvent";

        public const string ON_PLAYER_JOINED_EVENT = "OnPlayerJoinedEvent";
        public const string ON_PLAYER_LEFT_EVENT = "OnPlayerLeftEvent";
        public const string ON_PLAYER_NAME_EVENT = "OnPlayerNameEvent";
        public const string ON_PLAYER_INPUT_EVENT = "OnPlayerInputEvent";

        public const string ON_PORTAL_CHANGE_EVENT = "OnPortalChangeEvent";

        public const string FLOAT_TYPE_LABEL = "Number";
        public const string STRING_TYPE_LABEL = "String";
        public const string BOOL_TYPE_LABEL = "Boolean";
        public const string VECTOR2_TYPE_LABEL = "Vector2";
        public const string VECTOR3_TYPE_LABEL = "Vector3";
        public const string BODY_ARRAY_TYPE_LABEL = "Body Array";
        public const string REFERENCE_TYPE_LABEL = "Reference";

        public const string BODY_TYPE_LABEL = "Mona Body Prefab";
        public const string ANIMATION_TYPE_LABEL = "Animation Clip";
        public const string AUDIO_TYPE_LABEL = "Audio Clip";

        public const string NETWORK_SPAWNER_STARTED_EVENT = "OnNetworkSpawnerStartedEvent";
        public const string NETWORK_SPAWNER_INITIALIZED_EVENT = "OnNetworkSpawnerInitializedEvent";
        public const string MONA_BODIES_BEFORE_START_EVENT = "OnMonaBodiesBeforeStartEvent";
        public const string MONA_BODIES_START_EVENT = "OnMonaBodiesStartEvent";
        public const string MONA_BODIES_CLAIM_HOST_EVENT = "OnMonaBodiesClaimHostEvent";
        public const string MONA_BODY_INSTANTIATED = "OnMonaBodyInstantiated";
        public const string MONA_ASSET_PROVIDER_ADDED = "OnMonaAssetProviderAdd";
        public const string MONA_ASSET_PROVIDER_REMOVED = "OnMonaAssetProviderRemoved";
        public const string MONA_BODY_SPAWNED = "OnBodySpawned";
        public const string MONA_BODY_DESPAWNED = "OnBodyDespawned";
        public const string MONA_BODY_PARENT_CHANGED_EVENT = "OnMonaBodyParentChangedEvent";
        public const string MONA_BODY_EVENT = "OnMonaBodyEvent";

    }
}