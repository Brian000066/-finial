using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public static bool isChatting = false;

    public GameObject chatUI;
    public Rigidbody2D playerRb;

    private bool playerNear = false;
    private bool chatOpen = false;

    void Start()
    {
        chatUI.SetActive(false);
        isChatting = false;
    }

    void Update()
    {
        if (playerNear && !chatOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenChat();
        }

        if (chatOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseChat();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            playerRb = other.GetComponent<Rigidbody2D>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;

            // 如果正在聊天，不要因為離開 Trigger 就關掉
            if (!chatOpen)
            {
                chatUI.SetActive(false);
            }
        }
    }

    void OpenChat()
    {
        chatOpen = true;
        isChatting = true;
        chatUI.SetActive(true);

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
        }
    }

    void CloseChat()
    {
        chatOpen = false;
        isChatting = false;
        chatUI.SetActive(false);
    }
}