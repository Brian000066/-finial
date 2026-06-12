using UnityEngine;

public class PlayerCharacterLoader : MonoBehaviour
{
    public Sprite[] seaturtleSprites;
    public Sprite[] whaleSprites;
<<<<<<< HEAD
    public Sprite[] polarBearSprites;

    public PolygonCollider2D turtleCollider;
    public CapsuleCollider2D whaleCollider;
    public BoxCollider2D polarBearCollider;
=======
>>>>>>> b0b843043e5ca1bc77246e8da03535189231dce3

    private SpriteRenderer sr;
    private TurtleWalkAnimator animatorScript;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animatorScript = GetComponent<TurtleWalkAnimator>();

        int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);

<<<<<<< HEAD
        turtleCollider.enabled = false;
        whaleCollider.enabled = false;
        polarBearCollider.enabled = false;

=======
>>>>>>> b0b843043e5ca1bc77246e8da03535189231dce3
        if (selected == 0)
        {
            sr.sprite = seaturtleSprites[0];
            animatorScript.walkSprites = seaturtleSprites;
<<<<<<< HEAD

            turtleCollider.enabled = true;
=======
>>>>>>> b0b843043e5ca1bc77246e8da03535189231dce3
        }
        else if (selected == 1)
        {
            sr.sprite = whaleSprites[0];
            animatorScript.walkSprites = whaleSprites;
<<<<<<< HEAD

            whaleCollider.enabled = true;
        }
        else if (selected == 2)
        {
            sr.sprite = polarBearSprites[0];
            animatorScript.walkSprites = polarBearSprites;

            polarBearCollider.enabled = true;
=======
>>>>>>> b0b843043e5ca1bc77246e8da03535189231dce3
        }
    }
}