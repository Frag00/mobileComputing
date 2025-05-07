using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    // per farlo stare fermo durante le animazioni di preghiera e pozione
    private bool canmove = true;

    /* per il dash */
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 7f;
    private float dashingTime = 0.1f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;


    /* per il respawn ******************************************/
    public Collider2D collider;
    private bool active = true;
    private Vector2 respawnPoint;
    public Collider2D ultimoCheckPoint;
    // ***********************************************************
    public int vitaMassimaTotale = 5;
    public Text potionText;
    public int currentPotions = 0;
    public Text gemText;
    public int currentGems = 0;
    public int maxHealth = 5;
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
        // per il trailrenderer
        tr = GetComponent<TrailRenderer>();
        //*************************************************
        // respawn e morte
        collider = GetComponent<Collider2D>();
        SetRespawnPoint(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) { return; }

        if(maxHealth <= 0)
        {
            Die();
        }

        if (isDashing) return;
        // Per consumare una pozione e recuperare vita
        if(maxHealth<vitaMassimaTotale && currentPotions > 0)
        {
            if (Input.GetKeyDown(KeyCode.N) && isGround)
            {
                canmove = false;
                animator.SetTrigger("Potion");
                SoundEffectManager.Play("PlayerHeal");
                currentPotions--;
                maxHealth++;
            }
        }

        // per il checkpoint
        if (ultimoCheckPoint!=null && Input.GetKeyDown(KeyCode.F) && isGround)
        {
            //disabilita movimento durante l'animazione
            canmove=false;
            //animazione preghiera
            animator.SetTrigger("Pray");
            // riproduci suono preghiera
            SoundEffectManager.Play("PlayerPray");
            // setrespawn
            SetRespawnPoint(ultimoCheckPoint.transform.position);
            //ridai vita
            maxHealth = vitaMassimaTotale;

        }

        // gestione di tutti i testi relativi a variabili
        potionText.text = currentPotions.ToString();
        gemText.text = currentGems.ToString();
        health.text = maxHealth.ToString();

        
            movement = Input.GetAxis("Horizontal");

            if (movement < 0 && lookingRight)
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                lookingRight = false;
            }
            else
            {
                if (!lookingRight && movement > 0)
                {
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    lookingRight = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGround && canmove)
            {
                Jump();
                isGround = false;
                animator.SetBool("Jump", true);
            }

            // per il dash

            if(Input.GetKeyDown(KeyCode.C) && canDash)
        {
            StartCoroutine(Dash());
        }
            /////////////////

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

    public void PlayWalkSound()
    {
        SoundEffectManager.Play("PlayerWalk");
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        if (canmove)
        {
            transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
        }
    }

    public void EnableMovement() { canmove = true; }

    void Jump()
    {
        rigidBody.AddForce(new Vector2(0f, jumpHeight),ForceMode2D.Impulse);
        SoundEffectManager.Play("PlayerJump");
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
        SoundEffectManager.Play("PlayerAttack");
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
            if(collInfo.gameObject.GetComponent<FirstEnemySkeletonScript>() != null)
            {
                collInfo.gameObject.GetComponent<FirstEnemySkeletonScript>().TakeDamage(1);
            }
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
        animator.SetTrigger("PTakesDamage");
        SoundEffectManager.Play("PlayerHit");
        maxHealth -= damage;

    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if(other.gameObject.tag == "Gem")
        {
            Collider2D gemCollider = other.GetComponent<Collider2D>();
            if (gemCollider != null) { 
                gemCollider.enabled = false;
            }
            currentGems ++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            SoundEffectManager.Play("Gem");
            Destroy(other.gameObject,1f);
        }
        if (other.gameObject.tag == "VictoryPoint")
        {
            FindObjectOfType<SceneManagement>().LoadLevel();
        }
        if(other.gameObject.tag == "Potion")
        {
            Collider2D potionCollider = other.GetComponent<Collider2D>();
            if (potionCollider != null)
            {
                potionCollider.enabled = false;
            }
            currentPotions++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("PCollected");
            SoundEffectManager.Play("Potion");
            Destroy(other.gameObject, 0.5f);
        }
        if(other.gameObject.tag == "Kneeler")
        {
            ultimoCheckPoint = other;
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other == ultimoCheckPoint)
        {
            ultimoCheckPoint = null;
        }
    }

    public void Die()
    {     /* non più in uso 
        Debug.Log("Player Died");
        animator.SetBool("isDead", true);
        FindObjectOfType<GameManager>().isGameActive = false;
        Destroy(this.gameObject); */
        DieByCollision();
    }



    // da qui in poi morte con respawn 
    public void MiniJump()
    {
        rigidBody.AddForce(new Vector2(0f, jumpHeight/2), ForceMode2D.Impulse);
    }
    public void DieByCollision()
    {
        if (maxHealth > 1)
        {
            active = false;
            collider.enabled = false;
            MiniJump();
            StartCoroutine(Respawn());
            maxHealth--;
            if (currentGems > 0) currentGems--;
            SoundEffectManager.Play("PlayerHit");
        }
        else
        { /*    implementare game over */
            SoundEffectManager.Play("PlayerDie");
            active = false;
            collider.enabled = false;
            MiniJump();
            StartCoroutine(Respawn());
            maxHealth = vitaMassimaTotale;
            currentGems = 0;
            currentPotions = 0;
             // non uso più Die();
    
        }

    }
    public void SetRespawnPoint(Vector2 position)
    {
        respawnPoint = position;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.3f);
        transform.position = respawnPoint;
        active = true;
        collider.enabled=true;
        MiniJump();
    }


    // per il dash
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0f;
        if(lookingRight) rigidBody.velocity = new Vector2(1 * dashingPower, 0f);
        else rigidBody.velocity = new Vector2(-1 * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        rigidBody.gravityScale = originalGravity;
        isDashing=false;
        tr.emitting = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
