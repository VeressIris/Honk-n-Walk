using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    public float speed = 2f;

    void Update()
    {
        if (!gameManager.gameOver)
        {
            transform.position = new Vector3(transform.position.x + 1f * speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }
}
