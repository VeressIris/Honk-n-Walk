using UnityEngine;

public class Despawn : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
    }

    private void Update()
    {
        if (transform.position.x < cam.transform.position.x - 10)
        {
            Destroy(gameObject);
        }
    }
}
