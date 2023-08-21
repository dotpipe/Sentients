using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // The player that the camera will follow
    private Vector3 offset; // The offset at which the camera follows the player

    private void Start()
    {
        CalculateOffset();
    }

    private void LateUpdate()
    {
        FollowPlayer();
        LookAtPlayer();
        AdjustPosition();
    }

    private void CalculateOffset()
    {
        offset = transform.position - player.transform.position;
    }

    private void FollowPlayer()
    {
        transform.position = player.transform.position + offset;
    }

    private void LookAtPlayer()
    {
        transform.LookAt(player.transform);
    }

    private void AdjustPosition()
    {
        Vector3 newPosition = player.transform.position + new Vector3(1, 1, -1) * offset.magnitude;
        newPosition.y = Mathf.Max(newPosition.y, player.transform.position.y + offset.y);
        transform.position = newPosition;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        CalculateOffset();
    }
}