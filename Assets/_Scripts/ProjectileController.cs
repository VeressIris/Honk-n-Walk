using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Rigidbody2D rb;
    private float speed = 10f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Audio")]
    [SerializeField] private PlaySFX sfx;
    [SerializeField] private AudioClip killEnemySFX;

    void Start()
    {    
        rb.velocity = transform.right * speed;
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))
        {
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; //stop projectile from moving

            sfx.PlaySound(killEnemySFX, 0.55f);

            animator.Play("Impact");
            
            KillEnemy(coll);
        }
    }

    private void KillEnemy(Collider2D coll)
    {
        sfx.PlaySound(killEnemySFX, 0.78f);

        Rigidbody2D enemyRb = coll.gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D enemyCollider = coll.gameObject.GetComponent<BoxCollider2D>();
        Vector3 enemyScale = coll.gameObject.transform.localScale;
        Animator enemyAnimator = coll.gameObject.GetComponent<Animator>();

        coll.gameObject.transform.localScale = new Vector3(enemyScale.x, enemyScale.y * -1, enemyScale.z); //flip enemy upside down
        
        enemyRb.bodyType = RigidbodyType2D.Dynamic; //make enemy fall
        enemyRb.gravityScale = 2f;
        enemyRb.AddForce(Vector2.up * 2.5f, ForceMode2D.Impulse); //make enemy bounce upwards before falling
        
        enemyCollider.enabled = false; //let enemy fall through floor

        //stop animating (currently only the crow has animation so the if statement is necessary)
        if (enemyAnimator != null)
        {
            enemyAnimator.Play("CrowDead");
        }
    }

    //called in the animation event
    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
