using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    private const float waitTime = 0.5f;

    [SerializeField] private float speed;
    [SerializeField] private float maxHealth;
    private float health;

    public float Speed { get { return speed; } set { speed = value; } }
    public float MaxHealth { get { return health; } }
    public float Health { get { return health; } set { health = value; } }

    private bool tagged;
    private bool frozen;
    private bool currentlyEffected;

    public bool Tagged { get { return tagged; } }
    public bool CurrentlyEffected { get { return currentlyEffected; } set { currentlyEffected = value; } }

    protected Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;

    private Color normalColor;
    private Color tagColor;

    public Color NormalColor { get { return normalColor; } set { normalColor = value; } }
    public Color TagColor { get { return tagColor; } set { tagColor = value; } }

    private void OnEnable()
    {
        GameManager.gameEndEvent += GameEnd;
    }

    private void OnDisable()
    {
        GameManager.gameEndEvent -= GameEnd;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        normalColor = spriteRenderer.color;
        tagColor = Color.black;
    }

    private void Start()
    {
        health = maxHealth;
    }

    protected virtual void Update()
    {
        if (tagged && !frozen)
        {
            health -= Time.deltaTime;

            if(health <= 0)
            {
                frozen = true;

                GameManager.Instance.KillCharacter(transform);

                Destroy(gameObject);
            }
        }
    }

    // Given the character is not frozen, moves the rigidbody by the input direction, scaled by speed and made frame rate independent
    public void Move(Vector2 direction)
    {
        if (!frozen)
        {
            Vector2 movement = Vector2.ClampMagnitude(direction, 1) * speed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + movement);
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!frozen && tagged && collision.transform.CompareTag("Character"))
        {
            collision.transform.GetComponent<BaseController>().Tag(true);

            tagged = false;

            spriteRenderer.color = normalColor;
        }
    }

    // Called when the tagged character collides with this character
    // Tags this character and pauses them
    public void Tag(bool pauseMovement)
    {
        tagged = true;
        GameManager.Instance.Tagged = transform;

        spriteRenderer.color = tagColor;

        if (pauseMovement)
        {
            StartCoroutine(PauseMovement(waitTime));
        }
    }

    // Pauses the ability to move for the length of waitTime
    public IEnumerator PauseMovement(float wait)
    {
        frozen = true;

        yield return new WaitForSeconds(wait);

        frozen = false;
    }

    // Freezes remaining character when the game has ended and stops health decreasing
    private void GameEnd(Transform winner)
    {
        frozen = true;
        tagged = false;
    }

    // Updates the sprite renderer color in order for invisibility powerup to work
    public void UpdateColor()
    {
        spriteRenderer.color = tagged ? tagColor : normalColor;
    }
}