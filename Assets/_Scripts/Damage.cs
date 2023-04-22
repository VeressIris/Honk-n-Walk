using UnityEngine;

public class Damage : MonoBehaviour
{
    private GameManager gameManager;

    private Rigidbody2D playerRb;
    private PlayerController playerController;

    private BoxCollider2D groundCollider;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = playerRb.gameObject.GetComponent<PlayerController>();

        groundCollider = GameObject.Find("Ground").GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            //TO DO: play sound effect
            
            playerController.health--;
            
            gameManager.HideHearts(playerController.health);

            playerRb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse); //bounce player

            if (playerController.health == 0)
            {
                gameManager.gameOver = true;

                playerRb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse); //bounce player
                groundCollider.enabled = false; //make player fall through the ground
            }
        }
        else if (coll.gameObject.CompareTag("Goose") && this.gameObject.CompareTag("Enemy"))
        {
            gameManager.gameOver = true;
        }
    }
}
