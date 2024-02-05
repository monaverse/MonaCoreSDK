namespace Mona.SDK.Core
{
    public class MonaCoreConstants
    {
        public const string TAG_PLAYER = "Player";
        public const string TAG_SPACE = "Space";
        public const string LAYER_LOCAL_PLAYER = "LocalPlayer";
        public const string LAYER_DEFAULT = "Default";
        public const string LAYER_PHYSICS_GROUP_A = "PhysicsGroupA";
        public const string MONA_TAG_PLAYER_CAMERA = "Camera";

        public const string STATE_CHANGED_EVENT = "OnMonaStateChangedEvent";
        public const string STATE_AUTHORITY_CHANGED_EVENT = "OnMOnaStateAuthorityChangedEvent";
        public const string TICK_EVENT = "OnTick";
        public const string LATE_TICK_EVENT = "OnLateTick";
        public const string FIXED_TICK_EVENT = "OnFixedTick";
        public const string MONA_BODY_FIXED_TICK_EVENT = "OnMonaBodyFixedTickEvent";
        public const string LOCAL_INPUT_EVENT = "OnMonaLocalInputEvent";
        public const string INPUT_EVENT = "OnMonaInputTickEvent";
        public const string INPUTS_EVENT = "OnMonaInputsTickEvent";
        public const string REGISTER_NETWORK_SETTINGS_EVENT = "OnMonaRegisterNetworkSettignsEvent";

        public const string VALUE_CHANGED_EVENT = "OnMonaValueChanged";

        public const string ON_CURSOR_VISIBILITY_CHANGED_EVENT = "OnCursorVisibilityChangedEvent";
        public const string ON_CHANGE_CURSOR_VISIBILITY_EVENT = "OnChangeCursorVisibilityEvent";

        public const string ON_PLAYER_JOINED_EVENT = "OnPlayerJoinedEvent";
        public const string ON_PLAYER_LEFT_EVENT = "OnPlayerLeftEvent";
        public const string ON_PLAYER_NAME_EVENT = "OnPlayerNameEvent";
        public const string ON_PLAYER_INPUT_EVENT = "OnPlayerInputEvent";

        public const string FLOAT_TYPE_LABEL = "Number";
        public const string STRING_TYPE_LABEL = "String";
        public const string BOOL_TYPE_LABEL = "Boolean";
        public const string VECTOR2_TYPE_LABEL = "Vector2";
        public const string VECTOR3_TYPE_LABEL = "Vector3";
        public const string REFERENCE_TYPE_LABEL = "Reference";

        public const string NETWORK_SPAWNER_STARTED_EVENT = "OnNetworkSpawnerStartedEvent";
        public const string MONA_BODIES_BEFORE_START_EVENT = "OnMonaBodiesBeforeStartEvent";
        public const string MONA_BODIES_START_EVENT = "OnMonaBodiesStartEvent";
        public const string MONA_BODIES_CLAIM_HOST_EVENT = "OnMonaBodiesClaimHostEvent";
        public const string MONA_BODY_SPAWNED = "OnBodySpawned";
        public const string MONA_BODY_DESPAWNED = "OnBodyDespawned";
        public const string MONA_BODY_PARENT_CHANGED_EVENT = "OnMonaBodyParentChangedEvent";

    }
}