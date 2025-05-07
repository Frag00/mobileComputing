using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstEnemySkeletonScript : MonoBehaviour
{
    public int maxHealth = 5;
    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;
    public bool facingLeft = true;
    public bool inRange = false;
    public Transform player;
    public float attackRange = 5f;
    public float retrieveDistance = 1.7f;
    public float chaseSpeed = 4f;
    public Animator animator;
    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(FindObjectOfType<GameManager>().isGameActive == false)
        {
            return;
        }

        if(maxHealth <= 0)
        {
            Die();
        }

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

        if (inRange)
        {
            if (player.position.x > transform.position.x && facingLeft) {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (player.position.x < transform.position.x && !facingLeft)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }

            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                animator.SetBool("Attack", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack", true);
            }
        }

        else
        {
            transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

            RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

            if (!hit && facingLeft)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (!hit && !facingLeft)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }
        }
    }

    public void Attack()
    {
        SoundEffectManager.Play("EnemyAttack");
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<PlayerScript>() != null)
            {
                collInfo.gameObject.GetComponent<PlayerScript>().takeDamage(1);
            }
        }
    }

    public void PlayChasingSound()
    {
        SoundEffectManager.Play("EnemyChase");
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) {
            return;
        }
        animator.SetTrigger("TakesDamage");
        SoundEffectManager.Play("EnemyHurt");
        maxHealth -= damage;
    }
    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null) {  return; }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position,Vector2.down * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRange);
        if(attackPoint == null) { return;}
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPoint.position,attackRadius);  
    }

    public void Die()
    {

        Debug.Log(this.transform.name + " Died");
        animator.SetBool("Died", true);
        Destroy(this.gameObject,1);
        
    }
}
