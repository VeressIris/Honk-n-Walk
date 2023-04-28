using UnityEngine;

public class Despawn : MonoBehaviour
{
    private GameObject cam;

    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
    }

    private void Update()
    {
        if (transform.position.x < cam.transform.position.x - 20 || // -20 to make sure the object despawns behind the camera regardless of resolution
            transform.position.y < -8) // -8 is where it goes off screen vertically
        {
            Destroy(gameObject);
        }
    }
}
