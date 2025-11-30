using UnityEngine;
using UnityEngine.UI;

/// Gestiona la mecánica de ataque del jugador, incluyendo la selección de tipo de ataque,
/// la búsqueda de enemigos y el disparo de proyectiles.
public class PlayerAttack : MonoBehaviour
{
    public enum AttackType { Normal, ShieldBreaker, Freeze }
    public AttackType currentAttackType = AttackType.Normal;
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    public Transform firePoint;

    [Header("UI Buttons")]
    public Image normalAttackBtn;
    public Image shieldAttackBtn;
    public Image freezeAttackBtn;

    private float activeAlpha = 1f;
    private float inactiveAlpha = 0.5f;

    private float nextFireTime = 0f;
    private Transform target;

    [Header("Audio")]
    public AudioClip shootSound; 
    private AudioSource audioSource; 

    /// Se ejecuta en cada frame. Busca al enemigo más cercano y dispara.
    void Start()
    {
        UpdateButtonVisuals(AttackType.Normal);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        FindNearestEnemy();
        
        if (target != null && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }


    public void SelectNormalAttack()
    {
        currentAttackType = AttackType.Normal;
        Debug.Log("Ataque seleccionado: Normal");
        UpdateButtonVisuals(AttackType.Normal);
    }

    public void SelectShieldBreakerAttack()
    {
        currentAttackType = AttackType.ShieldBreaker;
        Debug.Log("Ataque seleccionado: Rompe-Escudos");
        UpdateButtonVisuals(AttackType.ShieldBreaker);
    }

    public void SelectFreezeAttack()
    {
        currentAttackType = AttackType.Freeze;
        Debug.Log("Ataque seleccionado: Congelante");
        UpdateButtonVisuals(AttackType.Freeze);
    }

    void UpdateButtonVisuals(AttackType type)
    {
        // Primero, ponemos TODOS en modo "inactivo" (transparente)
        SetImageAlpha(normalAttackBtn, inactiveAlpha);
        SetImageAlpha(shieldAttackBtn, inactiveAlpha);
        SetImageAlpha(freezeAttackBtn, inactiveAlpha);

        // Luego, buscamos cuál es el activo y lo ponemos opaco
        switch (type)
        {
            case AttackType.Normal:
                SetImageAlpha(normalAttackBtn, activeAlpha);
                break;
            case AttackType.ShieldBreaker:
                SetImageAlpha(shieldAttackBtn, activeAlpha);
                break;
            case AttackType.Freeze:
                SetImageAlpha(freezeAttackBtn, activeAlpha);
                break;
        }
    }

    void SetImageAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }

    /// Busca a todos los enemigos en la escena y asigna el más cercano como 'target'.
    void FindNearestEnemy()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (EnemyController enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy;
        }
        else
        {
            target = null;
        }
    }

    /// Crea y dispara un proyectil en dirección al objetivo.
    void Shoot()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Projectile projectileScript = projectileGO.GetComponent<Projectile>();

        Animator projectileAnimator = projectileGO.GetComponent<Animator>();

        if (projectileScript != null && target != null)
        {
            // Calcula la dirección desde el punto de disparo hacia el enemigo.
            Vector2 directionToEnemy = target.position - firePoint.position;

            projectileScript.SetDirection(directionToEnemy);

            projectileScript.attackType = currentAttackType;

            if (projectileAnimator != null)
            {
                projectileAnimator.SetInteger("AttackState", (int)currentAttackType);
            }

        }

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound); 
        }

    }

    public void UpgradeDamage()
    {
        var projectileScript = projectilePrefab.GetComponent<Projectile>();
        projectileScript.damage *= 1.2f;
        Debug.Log("Daño del proyectil: " + projectileScript.damage);

        LevelSystem.instance.HideUpgradePanel();
    }

    public void UpgradeFireRate()
    {
        fireRate *= 1.15f;
        Debug.Log("Cadencia de fuego: " + fireRate);

        LevelSystem.instance.HideUpgradePanel();
    }
}