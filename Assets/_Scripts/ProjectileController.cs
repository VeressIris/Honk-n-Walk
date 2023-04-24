using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 10f;
    private Animator animator;

    void Start()
    {    
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))
        {
            animator.Play("Impact");
            if (StoppedPlaying())
            {
                Destroy(gameObject);
            }

            KillEnemy(coll);
        }
    }

    private void KillEnemy(Collider2D coll)
    {
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

    bool StoppedPlaying()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Impact") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            return false;
        }

        return true;
    }
}
