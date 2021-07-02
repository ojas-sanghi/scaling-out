using Godot;

public class Blockade : Area2D
{
    float health = 150f;

    int numSprites;
    float healthPerSprite;

    int currentSpriteNum;
    Sprite currentSprite;
    float currentSpriteHealth;

    Node2D sprites;

    public override void _Ready()
    {
        Events.blockadeHit += OnBlockadeHit;

        sprites = GetNode<Node2D>("Sprites");

        Reset();
    }

    public override void _ExitTree()
    {
        Events.blockadeHit -= OnBlockadeHit;
    }

    void Reset()
    {
        health = 150;

        numSprites = sprites.GetChildCount();
        healthPerSprite = health / numSprites;

        currentSpriteNum = 0;
        currentSprite = (Sprite)sprites.GetChildren()[currentSpriteNum];
        currentSpriteHealth = healthPerSprite;
    }

    void OnBlockadeHit(float dmg)
    {
        health -= dmg;
        currentSpriteHealth -= dmg;

        // set transparency to health / max health
        if (currentSpriteHealth > 0)
        {
            currentSprite.Modulate = new Color(1, 1, 1, (currentSpriteHealth / healthPerSprite));
        }
        else // if destroyed, hide it and move to next sprite
        {
            currentSprite.Visible = false;
            currentSpriteNum++;

            // only try to get sprite if it exists
            if (currentSpriteNum < numSprites)
            {
                currentSprite = (Sprite)sprites.GetChildren()[currentSpriteNum];
                currentSpriteHealth = healthPerSprite;
            }
            else
            {
                Events.publishroundWon();
                return;
            }
        }
    }
}