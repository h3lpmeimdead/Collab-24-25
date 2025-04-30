using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [Header("Player Components")]
    public PlayerShooting shootingPlayer;
    public GrapplingGun grapplingPlayer;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [Header("Follow Settings")]
    public float followSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10f);

    void LateUpdate()
    {
        Transform target = GetActivePlayerTransform();

        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            _virtualCamera.Follow = target;
        }
    }

    Transform GetActivePlayerTransform()
    {
        if (shootingPlayer != null && shootingPlayer.IsActive)
            return shootingPlayer.transform;

        if (grapplingPlayer != null && grapplingPlayer.IsActive)
        {
            return grapplingPlayer.transform.parent != null
                ? grapplingPlayer.transform.parent
                : grapplingPlayer.transform;
        }

        return null;
    }
}
