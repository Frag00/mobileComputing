using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnemySkeletonScript : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;
    public bool facingLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left*Time.deltaTime*moveSpeed);

        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position,Vector2.down,distance,layerMask) ;

        if (!hit && facingLeft)
        {
            transform.eulerAngles = new Vector3 (0,-180,0);
            facingLeft = false;
        }
        else if(!hit && !facingLeft)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingLeft = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null) {  return; }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position,Vector2.down * distance);
    }
}
