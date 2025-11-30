using UnityEngine;

/// Controla el comportamiento de un enemigo: movimiento, salud, escudo,
/// y la lógica al morir.
public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    public float health = 100f;
    public float shield = 50f;
    public float damage = 50f;
    public float xpValue = 15f;
    public float damageInterval = 0.5f;
    public int scoreValue = 100;
    public GameObject shieldVisual;

    private float nextDamageTime = 0f;
    private bool isFrozen = false;
    private float freezeTimer = 0f;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    [Header("Audio")]
    public AudioClip shieldBreakSound;
    private AudioSource audioSource;

    [Header("Audio Damage/Death")]
    public AudioClip hurtSound;
    public AudioClip deathSound;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();

        if (shieldVisual != null)
        {
            shieldVisual.SetActive(shield > 0);
        }

        UpdateShieldVisuals();

        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        // Si el enemigo está congelado, comienza el temporizador de descongelación.
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0)
            {
                isFrozen = false;
                UpdateShieldVisuals();

                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
    }

    /// Mueve al enemigo hacia el jugador.
    void FixedUpdate()
    {
        if (isFrozen || player == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;

        rb.velocity = direction * speed;
    }

    /// Reduce la vida del enemigo.
    public void TakeDamage(float damage)
    {
        if (shield > 0)
        {
            Debug.Log("¡Escudo bloqueó el daño!");
            return;
        }

        health -= damage;
        Debug.Log("Vida del enemigo: " + health);

        if (health > 0 && audioSource != null && hurtSound != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(hurtSound);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    /// Reduce el escudo del enemigo.
    public void TakeShieldDamage(float s_damage)
    {
        if (shield > 0)
        {
            shield -= s_damage;

            if (shield < 0)
            {
                shield = 0;
                
                if (shieldVisual != null) shieldVisual.SetActive(false);

                if (audioSource != null && shieldBreakSound != null)
                {
                    audioSource.PlayOneShot(shieldBreakSound);
                }

                UpdateShieldVisuals();

            }

            if (shield == 0)
            {              
                if (shieldVisual != null) shieldVisual.SetActive(false);

                if (audioSource != null && shieldBreakSound != null)
                {
                    audioSource.PlayOneShot(shieldBreakSound);
                }

                UpdateShieldVisuals();

            }

            Debug.Log("Escudo del enemigo: " + shield + " " + "Vida del enemigo: " + health);
        }

        if (shieldVisual != null)
        {
            shieldVisual.SetActive(false);
        }

    }

    /// Congela al enemigo por una duración determinada.
    public void Freeze(float duration)
    {
        isFrozen = true;
        freezeTimer = duration;
        Debug.Log("¡Enemigo congelado!" + " " + "Vida del enemigo: " + health);

        spriteRenderer.color = Color.cyan;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    /// Actualiza el color del sprite del enemigo para indicar si tiene escudo o no.
    void UpdateShieldVisuals()
    {
        if (isFrozen)
        {
            return;
        }

        if (shield > 0)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    /// Se ejecuta continuamente mientras otro collider esté en contacto con este.
    /// Se usa para aplicar daño constante al jugador.
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= nextDamageTime)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }

                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    void Die()
    {
        if (LevelSystem.instance != null)
        {
            LevelSystem.instance.AddXP(xpValue);
        }

        LevelSystem.instance.AddScore(scoreValue);

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f); 
        }

        Destroy(gameObject);
    }
}