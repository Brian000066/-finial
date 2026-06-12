using UnityEngine;

public class PlayerCharacterLoader : MonoBehaviour
{
    public Sprite[] seaturtleSprites;
    public Sprite[] whaleSprites;
    public Sprite[] polarBearSprites;

    public PolygonCollider2D turtleCollider;
    public CapsuleCollider2D whaleCollider;
    public BoxCollider2D polarBearCollider;

    private SpriteRenderer sr;
    private TurtleWalkAnimator animatorScript;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animatorScript = GetComponent<TurtleWalkAnimator>();

        int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);

        turtleCollider.enabled = false;
        whaleCollider.enabled = false;
        polarBearCollider.enabled = false;

        if (selected == 0)
        {
            sr.sprite = seaturtleSprites[0];
            animatorScript.walkSprites = seaturtleSprites;

            turtleCollider.enabled = true;
        }
        else if (selected == 1)
        {
            sr.sprite = whaleSprites[0];
            animatorScript.walkSprites = whaleSprites;

            whaleCollider.enabled = true;
        }
        else if (selected == 2)
        {
            sr.sprite = polarBearSprites[0];
            animatorScript.walkSprites = polarBearSprites;

            polarBearCollider.enabled = true;
        }
    }
}