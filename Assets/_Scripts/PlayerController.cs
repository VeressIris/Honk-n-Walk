using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public int health = 3;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10f;
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    [SerializeField] private LayerMask groundMask;

    [Header("Shooting")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject shootingPoint;
    private bool canShoot = true;
    [SerializeField] private float cooldown = 0.5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Shooting")]
    [SerializeField] private Transform cam; // need this in order to spawn projectile as child of the camera in order for it to not lag behind camera
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerCollider = GetComponent<BoxCollider2D>();
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
            //TO DO: play shoot sound
            animator.Play("Attack");
            StartCoroutine("ShootProjectile");
        }
    }
    //Shoot function overload so it can be used on the shoot button
    public void Shoot()
    {
        if (canShoot)
        {
            //TO DO: play shoot sound
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
