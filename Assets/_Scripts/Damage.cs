using UnityEngine;
using UnityEngine.InputSystem;

public class Damage : MonoBehaviour
{
    private GameObject player;
    private Animator playerAnim;
    private Rigidbody2D playerRb;
    private PlayerInput playerInput;
    private PlayerController playerController;
    private GameManager gameManager;
    private CameraShake shakeScript;
    private PostProcessingFX postProcessing;

    [SerializeField] private PlaySFX sfx;
    [SerializeField] private AudioClip damageSFX;
    [SerializeField] private AudioClip deadGooseSFX;
    [SerializeField] private AudioClip deathSFX;

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

        shakeScript = GameObject.FindWithTag("MainCamera").GetComponentInChildren<CameraShake>();
        postProcessing = shakeScript.gameObject.GetComponentInParent<PostProcessingFX>();

        player = GameObject.FindWithTag("Player");
        playerAnim = player.GetComponent<Animator>();
        playerRb = player.GetComponent<Rigidbody2D>();
        playerInput = player.GetComponent<PlayerInput>();
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            if (!sfx.audioSrc.isPlaying)
            {
                sfx.PlaySound(damageSFX, 0.8f);
            }

            playerController.health--;

            gameManager.HideHearts(playerController.health);

            //disable collider (to avoid taking damage twice)
            this.gameObject.GetComponent<Collider2D>().enabled = false; ;

            //bonus effects:
            shakeScript.ShakeCamera(1f, 0.225f);
            StartCoroutine(postProcessing.PlayerDamageEffect(0.5f));
            playerRb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse); //bounce player

            if (playerController.health == 0)
            {
                sfx.PlaySound(deathSFX, 0.85f);

                gameManager.gameOver = true;
                
                KillPlayer();
            }
        }
        else if (coll.gameObject.CompareTag("Goose") && this.gameObject.CompareTag("Enemy"))
        {
            if (!sfx.audioSrc.isPlaying)
            {
                sfx.PlaySound(deadGooseSFX, 0.7f);
            }

            gameManager.gameOver = true;

            shakeScript.ShakeCamera(1.75f, 0.275f);
            KillGoose(coll);
        }
    }

    private void KillGoose(Collider2D coll)
    {
        GameObject goose = coll.gameObject;
        goose.AddComponent<Rigidbody2D>();
        Rigidbody2D gooseRb = goose.GetComponent<Rigidbody2D>();
        gooseRb.gravityScale = 2f;
        BoxCollider2D gooseCollider = goose.GetComponent<BoxCollider2D>();
        Vector3 gooseScale = goose.transform.localScale;
        Animator gooseAnim = goose.GetComponent<Animator>();

        goose.transform.localScale = new Vector3(gooseScale.x, gooseScale.y * -1, gooseScale.z); //flip goose upside down

        gooseRb.bodyType = RigidbodyType2D.Dynamic; //make goose fall
        gooseRb.AddForce(Vector2.up * 2.5f, ForceMode2D.Impulse); //make goose bounce upwards before falling

        gooseCollider.enabled = false; //let goose fall through floor
        gooseAnim.Play("GooseDead");

        //despawn goose when it goes off screen
        if (goose.transform.position.y < -8)
        {
            Destroy(goose);
        }

        playerInput.enabled = false;
        playerController.enabled = false;
        playerAnim.Play("Idle");
    }

    private void KillPlayer()
    {
        BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();
        
        playerCollider.size = new Vector2(0.25f, 0.08f); //change box collider size so player doesn't appear to float
        playerInput.enabled = false;
    }
}
