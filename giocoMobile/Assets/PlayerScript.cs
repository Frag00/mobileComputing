using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public int maxHealth = 3;
    public Text health;
    public float moveSpeed = 5f;
    private float movement;
    private bool lookingRight = true;
    public Rigidbody2D rigidBody;
    public float jumpHeight = 5f;
    public bool isGround = true;
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
        if(maxHealth <= 0)
        {
            Die();
        }

        health.text = maxHealth.ToString();

        movement = Input.GetAxis("Horizontal");

        if(movement < 0 && lookingRight)
        {
            transform.eulerAngles = new Vector3(0f,-180f,0f);
            lookingRight = false;
        }
        else
        {
            if (!lookingRight && movement>0) {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                lookingRight = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGround) {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }

        if (Mathf.Abs(movement) > 0.1f)
        {
            animator.SetFloat("RunSpeed", 1f);
        }
        else if (Mathf.Abs(movement) < 0.1f)
        {
            animator.SetFloat("RunSpeed", 0f);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
    }

    void Jump()
    {
        rigidBody.AddForce(new Vector2(0f, jumpHeight),ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }

    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
            Debug.Log(collInfo.gameObject.name + "Takes Damage");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) {  return; }
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    public void takeDamage(int damage)
    {
        if(maxHealth <= 0) { return; }
        maxHealth -= damage;

    }

    public void Die()
    {
        Debug.Log("Die");
    }
}
