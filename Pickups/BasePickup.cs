using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePickup : MonoBehaviour
{
    public enum PickupType
    {
        speedBoost,
        invisibility,
        health
    }

    protected PickupType type;

    public PickupType Type { get { return type; } }

    [SerializeField] private float effectLength;

    private SpriteRenderer spriteRenderer;

    private Collider2D pickupCollider;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        pickupCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            BaseController character = collision.GetComponent<BaseController>();

            if (!character.CurrentlyEffected)
            {
                spriteRenderer.enabled = false;
                pickupCollider.enabled = false;

                GameManager.Instance.RemovePickup(transform);

                StartCoroutine(EffectPeriod(character));
            }
        }
    }

    // Starts the effect and then reverts it after the effect length has passed
    IEnumerator EffectPeriod(BaseController character)
    {
        StartEffect(character);
        character.CurrentlyEffected = true;

        yield return new WaitForSeconds(effectLength);

        EndEffect(character);
        character.CurrentlyEffected = false;

        Destroy(gameObject);
    }

    // Applies effect to the character
    protected abstract void StartEffect(BaseController character);

    // Reverts character back to original state
    protected abstract void EndEffect(BaseController character);
}
