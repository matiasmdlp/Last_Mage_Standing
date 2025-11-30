using UnityEngine;

/// Define el comportamiento de un proyectil. Se encarga de su movimiento,
/// vida útil y la lógica de colisión con enemigos.
public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 100f;
    public float lifetime = 1f;

    public float shieldBreakDamage = 15f;
    public float freezeDuration = 0.3f;
    public float lowDamageMultiplier = 0.01f;

    [HideInInspector]
    public PlayerAttack.AttackType attackType;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// Se ejecuta al inicio. Inicia el contador para la autodestrucción.
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    /// Establece la dirección y velocidad del proyectil, y lo rota para que apunte en esa dirección.
    public void SetDirection(Vector2 direction)
    {
        direction.Normalize();

        rb.velocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void FixedUpdate()
    {

    }

    /// Se ejecuta cuando este objeto entra en colisión con otro.
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                switch (attackType)
                {
                    case PlayerAttack.AttackType.Normal:
                        enemy.TakeDamage(damage);
                        break;

                    case PlayerAttack.AttackType.ShieldBreaker:
                        enemy.TakeDamage(damage * lowDamageMultiplier);
                        enemy.TakeShieldDamage(shieldBreakDamage);
                        break;

                    case PlayerAttack.AttackType.Freeze:
                        enemy.Freeze(freezeDuration);
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}