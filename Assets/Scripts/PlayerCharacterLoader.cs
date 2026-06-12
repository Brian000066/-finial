using UnityEngine;

public class PlayerCharacterLoader : MonoBehaviour
{
    public Sprite[] seaturtleSprites;
    public Sprite[] whaleSprites;

    private SpriteRenderer sr;
    private TurtleWalkAnimator animatorScript;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animatorScript = GetComponent<TurtleWalkAnimator>();

        int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);

        if (selected == 0)
        {
            sr.sprite = seaturtleSprites[0];
            animatorScript.walkSprites = seaturtleSprites;
        }
        else if (selected == 1)
        {
            sr.sprite = whaleSprites[0];
            animatorScript.walkSprites = whaleSprites;
        }
    }
}