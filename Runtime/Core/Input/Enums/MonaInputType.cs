namespace Mona.SDK.Core.Input.Enums
{
    public enum MonaInputType
    {
        None = 0,
        Jump = 1,
        Move = 2,
        Action = 3,
        Sprint = 4,
        Interact = 5,
        Look = 6,
        SwitchCamera = 7,
        Respawn = 8,
        Debug = 9,
        ToggleUI = 10,
        EmoteWheel = 11,
        EmojiTray = 12,
        ToggleNametags = 13,
        Escape = 14,
        OpenChat = 15,
        ToggleMouseCapture = 16,
        Key = 17
    }

    public enum MonaInputMoveType
    {
        AllDirections=0,
        EightWay = 1,
        FourWay = 2,
        Horizontal = 3,
        Vertical = 4,
        Up = 5,
        Down = 6,
        Left = 7,
        Right = 8,
        UpLeft = 9,
        UpRight = 10,
        DownLeft = 11,
        DownRight = 12
    }
}