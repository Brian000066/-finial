using UnityEngine;

public class PlayerMove : MonoBehaviour 
{ 
    public float moveSpeed = 5f; 
    public float jumpForce = 8f; 

    private Rigidbody2D rb; 
    private bool isGrounded = false; 

    void Start() 
    { 
        rb = GetComponent<Rigidbody2D>(); 
    } 
    
    void Update() 
    { float moveX = 0f; 
        // 往右
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
        { 
            moveX = 1f; 
        } 
        // 往左
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
        { 
            moveX = -1f; 
        } 
        
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y); 
        // 跳躍
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded) 
        { 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); 
            isGrounded = false; 
        } 
    } 
    
    void OnCollisionEnter2D(Collision2D collision) 
    { 
        if (collision.gameObject.CompareTag("Ground")) 
        { 
            isGrounded = true; 
        } 
    } 
}