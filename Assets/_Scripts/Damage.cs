using UnityEngine;
using UnityEngine.InputSystem;

public class Damage : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private CameraShake shakeScript;
    private Animator playerAnim;
    private PlayerInput playerInput;
    private PostProcessingFX postProcessing;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = playerRb.gameObject.GetComponent<PlayerController>();
        playerAnim = playerRb.gameObject.GetComponent<Animator>();
        playerInput = playerRb.gameObject.GetComponent<PlayerInput>();
        
        shakeScript = GameObject.FindWithTag("MainCamera").GetComponentInChildren<CameraShake>();
        postProcessing = GameObject.FindWithTag("MainCamera").GetComponent<PostProcessingFX>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            //TO DO: play sound effect
            playerController.health--;

            gameManager.HideHearts(playerController.health);

            //bonus effects:
            shakeScript.ShakeCamera(1.685f, 0.225f);
            StartCoroutine(postProcessing.PlayerDamageEffect(0.5f));
            playerRb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse); //bounce player

            if (playerController.health == 0)
            {
                gameManager.gameOver = true;
                
                KillPlayer();
            }
        }
        else if (coll.gameObject.CompareTag("Goose") && this.gameObject.CompareTag("Enemy"))
        {
            gameManager.gameOver = true;

            shakeScript.ShakeCamera(2f, 0.25f);
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
        PlayerController playerController = playerRb.gameObject.GetComponent<PlayerController>();

        goose.transform.localScale = new Vector3(gooseScale.x, gooseScale.y * -1, gooseScale.z); //flip goose upside down

        gooseRb.bodyType = RigidbodyType2D.Dynamic; //make goose fall
        gooseRb.AddForce(Vector2.up * 2.5f, ForceMode2D.Impulse); //make goose bounce upwards before falling

        gooseCollider.enabled = false; //let goose fall through floor
        gooseAnim.Play("GooseDead");

        playerInput.enabled = false;
        playerController.enabled = false;
        playerAnim.Play("Idle");
    }

    private void KillPlayer()
    {
        BoxCollider2D playerCollider = playerRb.gameObject.GetComponent<BoxCollider2D>();
        
        playerCollider.size = new Vector2(0.25f, 0.08f); //change box collider size so player doesn't appear to float
        playerInput.enabled = false;
    }
}
