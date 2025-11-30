

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


/// Controla el comportamiento del jugador, incluyendo movimiento, salud,
/// daño recibido y los límites dentro del escenario.
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float health = 100f;
    public Slider healthBar;
    public GameObject gameOverPanel;
    private float maxHealth;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private bool isInvincible = false;
    private float invincibilityDuration = 2f;

    [Header("Límites del Escenario")]
    public float limiteMinX = -74.5f;
    public float limiteMaxX = 74.5f;
    public float limiteMinY = -29f;
    public float limiteMaxY = 30f;

    [Header("Audio")]
    public AudioClip hurtSound;
    public AudioClip deathSound;
    private AudioSource audioSource;


    /// Inicializamos componentes y establecer valores iniciales.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        maxHealth = health;
        UpdateHealthBar();

        isInvincible = true;
        Invoke("EndInvincibility", invincibilityDuration);
    }

    /// Se llama por el Player Input component cuando se detecta una acción de movimiento.
    /// Lee el valor del input (un Vector2) y lo almacena.
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // Actualiza los parámetros del Animator para cambiar la animación según la dirección.
        if (animator != null)
        {
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY", moveInput.y);
        }
    }

    /// Se ejecuta en un intervalo de tiempo fijo.
    /// Aquí se aplica el movimiento al Rigidbody2D.
    void FixedUpdate()
    {
        if (animator != null)
        {
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY", moveInput.y);
        }

        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    // Limita los valores de X e Y para que no se salgan de los límites definidos.
    void LateUpdate()
    {
        Vector2 posicionActual = rb.position;

        float xSujetado = Mathf.Clamp(posicionActual.x, limiteMinX, limiteMaxX);
        float ySujetado = Mathf.Clamp(posicionActual.y, limiteMinY, limiteMaxY);

        rb.position = new Vector2(xSujetado, ySujetado);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 esquinaSuperiorIzquierda = new Vector3(limiteMinX, limiteMaxY, 0);
        Vector3 esquinaSuperiorDerecha = new Vector3(limiteMaxX, limiteMaxY, 0);
        Vector3 esquinaInferiorIzquierda = new Vector3(limiteMinX, limiteMinY, 0);
        Vector3 esquinaInferiorDerecha = new Vector3(limiteMaxX, limiteMinY, 0);

        Gizmos.DrawLine(esquinaSuperiorIzquierda, esquinaSuperiorDerecha);
        Gizmos.DrawLine(esquinaSuperiorDerecha, esquinaInferiorDerecha);
        Gizmos.DrawLine(esquinaInferiorDerecha, esquinaInferiorIzquierda);
        Gizmos.DrawLine(esquinaInferiorIzquierda, esquinaSuperiorIzquierda);
    }


    /// Función para reducir la vida del jugador.
    public void TakeDamage(float damage)
    {
        if (isInvincible) return;
        health -= damage;
        Debug.Log("Vida del jugador: " + health);

        UpdateHealthBar();

        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    /// Mejora la velocidad de movimiento del jugador.
    public void UpgradeMoveSpeed()
    {
        moveSpeed *= 1.1f;
        Debug.Log("Nueva velocidad de movimiento: " + moveSpeed);

        LevelSystem.instance.HideUpgradePanel();
    }

    /// Actualiza la barra de vida en la UI para reflejar la salud actual.
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = health / maxHealth;
        }
    }

    void EndInvincibility()
    {
        isInvincible = false;
        Debug.Log("¡La invencibilidad ha terminado!");
    }

    /// Gestiona la muerte del jugador.
    void Die()
    {
        Debug.Log("¡El jugador ha muerto!");
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f);
        }

        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}