using UnityEngine;
public class PlayerContext
{
    public readonly PlayerController Player;
    public readonly CharacterController Controller;
    public readonly Transform Transform;

    public readonly CoverMovement CoverMove;
    public readonly FreeMovement FreeMove;
    public readonly FpsMovement FpsMove;

    public PlayerContext(PlayerController player, CharacterController controller, 
        Transform transform, CoverMovement coverMove, FreeMovement freeMove, FpsMovement fpsMove)
    {
        Player     = player;
        Controller = controller;
        Transform  = transform;
        CoverMove  = coverMove;
        FreeMove   = freeMove;
        FpsMove    = fpsMove;
    }
}