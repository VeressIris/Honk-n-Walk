using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseController : MonoBehaviour
{
    //!CURRENTLY NOT WORKING, THEREFORE NOT IN USE!
    [SerializeField] private BoxCollider2D coll;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Transform initialPos;
    Vector3 collOffset;
    bool moving = false;

    private void Start()
    {
        collOffset = new Vector3(0, 0.75f, 0);
    }

    void Update()
    {
        RaycastHit2D ray = Physics2D.BoxCast(coll.bounds.max, coll.bounds.size + collOffset, 0f, Vector2.right, 0.35f, obstacleMask);
        if (ray.collider != null && !moving)
        {
            moving = true;

            StartCoroutine(MoveGoose());

            Debug.Log("obstacle incoming");
        }
    }

    private IEnumerator MoveGoose()
    {
        StartCoroutine(MoveGooseToTarget(target.position));
        
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(MoveGooseToTarget(initialPos.position));
        
        moving = false;
    }

    private IEnumerator MoveGooseToTarget(Vector3 target)
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);

            yield return null;
        }
    }
}
