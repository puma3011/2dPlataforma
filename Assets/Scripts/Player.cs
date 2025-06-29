using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public float movement;
    public float speed = 5f;
    public float jumpHeight = 7f;
    public bool isGround = true;
    private bool facingRight = true;

    public Text appleText;
    public int currentApples = 0;
    public GameObject panel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        appleText.text = currentApples.ToString();
        
        movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement * Time.deltaTime * speed, 0f, 0f);
        Flip();
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            Jump();
            animator.SetBool("Jump", true);
            isGround = false;
        }

        if (Mathf.Abs(movement) > .1f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (movement < .1f)
        {
            animator.SetFloat("Run", 0f);
        }
    }

    void Jump()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpHeight;
        rb.linearVelocity = velocity;
    }

    void Flip()
    {
        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }

        if (other.collider.tag == "Saw" || other.collider.tag == "Spike")
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Apple")
        {
            currentApples++;
            Destroy(other.gameObject);
        }

        if (other.tag == "Cup")
        {
            SceneManager.LoadScene(0);
        }
    }

    void Die()
    {
        Debug.Log("Player murió");
        panel.SetActive(true);
        Destroy(this.gameObject);
    }
}
