using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BaseController : MonoBehaviour
{
    [Header("Components")]
    protected Rigidbody2D rb;

    private SpriteRenderer bodySpriteRenderer;

    private TrailRenderer bodyTrailRenderer;

    private Animator iceAnimator;

    private AudioSource freezeSound;
    private AudioSource unfreezeSound;
    private AudioSource tagSound; 
    private AudioSource deathSound;


    [Header("Colors")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color taggedColor;

    [SerializeField] private Gradient normalTrailGradient;
    [SerializeField] private Gradient taggedTrailGradient;


    [Header("Stats")]
    private static readonly float taggedFreezeTime = 1;

    private float speed;
    private static readonly float minSpeed = 4; // Speed at max health
    private static readonly float maxSpeed = 7.75f; // Speed at near to zero health

    private static readonly float startingHealth = 10;
    private float health;

    public float StartingHealth { get { return startingHealth; } }
    public float Health { get { return health; } }


    [Header("States")]
    private bool tagged;
    private bool frozen;

    public bool Tagged { get { return tagged; } }


    [Header("Misc")]
    public UnityEvent cameraShake;

    [SerializeField] private AnimationClip unfreezeClip;


    protected virtual void OnEnable()
    {
        GameManager.gameEndEvent += GameEnd;
    }

    protected virtual void OnDisable()
    {
        GameManager.gameEndEvent -= GameEnd;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        bodySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        bodySpriteRenderer.color = normalColor;

        bodyTrailRenderer = GetComponentInChildren<TrailRenderer>();
        bodyTrailRenderer.colorGradient = normalTrailGradient;

        iceAnimator = GetComponentInChildren<Animator>();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        freezeSound = audioSources[0];
        unfreezeSound = audioSources[1];
        tagSound = audioSources[2];
        deathSound = audioSources[3];
    }

    private void Start()
    {
        speed = minSpeed;
        health = startingHealth;
    }

    protected virtual void Update()
    {
        // Decreases health if this character is tagged and checks for death
        if (tagged && !frozen)
        {
            health -= Time.deltaTime;

            speed = Mathf.Clamp(minSpeed + ((maxSpeed - minSpeed) * (1 - (health / startingHealth))), minSpeed, maxSpeed);

            if(health <= 0)
            {
                frozen = true;

                StartCoroutine(DelayedDeath());
            }
        }
    }

    // Given the character is not frozen, moves the rigidbody by the input direction, scaled by speed
    public void Move(Vector3 direction)
    {
        if (!frozen)
        {
            Vector2 movement = Vector3.ClampMagnitude(direction, 1) * speed * Time.fixedDeltaTime;

            Vector3 newPosition = Vector3.Lerp(rb.position, rb.position + movement, 0.9f);

            rb.MovePosition(newPosition);
        } 
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Checks if the collided object is another character and tags them if possible
        if (!frozen && tagged && collision.transform.CompareTag("Character"))
        {
            collision.transform.GetComponent<BaseController>().Tag();

            tagged = false;

            bodySpriteRenderer.color = normalColor;
            bodyTrailRenderer.colorGradient = normalTrailGradient;

            tagSound.Play();
        }
    }

    // Tags this character and freezes them
    public void Tag()
    {
        tagged = true;
        GameManager.Instance.TaggedCharacter = transform;

        bodySpriteRenderer.color = taggedColor;
        bodyTrailRenderer.colorGradient = taggedTrailGradient;

        cameraShake.Invoke();

        StartCoroutine(FreezeCharacter());
    }

    // Freezes this character for taggedFreezeTime
    private IEnumerator FreezeCharacter()
    {
        frozen = true;

        iceAnimator.SetBool("Frozen", true);
        freezeSound.Play();

        yield return new WaitForSeconds(taggedFreezeTime - unfreezeClip.length);

        iceAnimator.SetBool("Frozen", false);
        unfreezeSound.Play();

        yield return new WaitForSeconds(unfreezeClip.length);

        frozen = false;
    }

    // Destroys this gameobject, allowing death sound to play fully
    private IEnumerator DelayedDeath()
    {
        deathSound.Play();

        yield return new WaitForSeconds(deathSound.clip.length);

        GameManager.Instance.RemoveCharacter(transform);

        Destroy(gameObject);
    }

    // Freezes this character when the game has ended
    private void GameEnd(Transform winner)
    {
        frozen = true;
        tagged = false;
    }
}
