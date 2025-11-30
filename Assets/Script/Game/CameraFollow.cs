using UnityEngine;

/// Hace que la cámara siga suavemente al jugador.
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.01f;
    public Vector3 offset;
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // Calcula la posición deseada de la cámara.
        Vector3 desiredPosition = target.position + offset;

        // Interpola suavemente desde la posición actual de la cámara a la posición deseada.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Aplica la nueva posición a la cámara.
        transform.position = smoothedPosition;
    }
}