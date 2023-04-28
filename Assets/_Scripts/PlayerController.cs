using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public int health = 3;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private LayerMask groundMask;

    [Header("Shooting")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject shootingPoint;
    private bool canShoot = true;
    [SerializeField] private float cooldown = 0.65f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private Transform cam; // need this in order to spawn projectile as child of the camera in order for it to not lag behind camera

    [Header("SFX")]
    [SerializeField] private PlaySFX sfx;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip shootSFX;

    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        playerCollider.size = new Vector2(0.1422123f, 0.33f); //make sure player collider is set to the right size
    }

    void Start()
    {
        health = 3; //make sure player has full health
    }

    private void Update()
    {
        if (health > 0)
        {
            if (Grounded() && !Attacking())
            {
                animator.Play("Run");
            }
            else if (!Grounded())
            {
                animator.Play("Jump");
            }
        }
        else
        {
            animator.Play("Death");
        }
    }

    //invoked as Unity Event in player input component
    public void Jump()
    {
        if (Grounded())
        {
            sfx.PlaySound(jumpSFX, 1f);

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private bool Grounded()
    {
        RaycastHit2D ray = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, 0.2f, groundMask);

        return ray.collider != null;
    }

    //invoked as Unity Event in player input component
    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (ctx.started && canShoot)
        {
            sfx.PlaySound(shootSFX, 1.25f);

            animator.Play("Attack");
            StartCoroutine("ShootProjectile");
        }
    }
    //Shoot function overload so it can be used on the shoot button
    public void Shoot()
    {
        if (canShoot)
        {
            sfx.PlaySound(shootSFX, 0.75f);

            animator.Play("Attack");
            StartCoroutine("ShootProjectile");
        }
    }

    private IEnumerator ShootProjectile()
    {
        Instantiate(projectile, shootingPoint.transform.position, shootingPoint.transform.rotation, cam);
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    bool Attacking()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            return true;
        }

        return false;
    }
}
