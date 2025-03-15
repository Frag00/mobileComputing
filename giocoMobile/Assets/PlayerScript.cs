using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float movement;
    private bool lookingRight = true;
    public Rigidbody2D rigidBody;
    public float jumpHeight = 5f;
    public bool isGround = true;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
}
