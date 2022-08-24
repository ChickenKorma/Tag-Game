using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Action<Transform> gameEndEvent;

    private Transform tagged;

    public Transform Tagged { get { return tagged; } set { tagged = value; } }

    [SerializeField] private GameObject[] pickupPrefabs;
    [SerializeField] private GameObject characterPrefab; 

    [SerializeField] private int pickupSpawnIndex;
    [SerializeField] private int totalPickups;
    [SerializeField] private int totalCharacters;

    [SerializeField] private Vector2 spawnArea;

    [SerializeField] private float tagPickTime;

    private List<Transform> activePickups = new();
    private List<Transform> visibleCharacters = new();
    private List<Transform> remainingCharacters = new();  

    public List<Transform> ActivePickups { get { return activePickups; } }
    public List<Transform> VisibleCharacters { get { return visibleCharacters; } }
    public List<Transform> RemainingCharacters { get { return remainingCharacters; } }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        for (int i = 0; i < totalPickups; i++)
        {
            SpawnPickup();
        }

        remainingCharacters.Add(PlayerController.Instance.transform);
        visibleCharacters.Add(PlayerController.Instance.transform);

        SpawnCharacters();

        PickTagged();
    }

    // Spawns total number of characters at random but unique positions, adding them to remaining and visible character lists
    private void SpawnCharacters()
    {
        List<Vector2> spawnPoints = new();

        spawnPoints.Add(PlayerController.Instance.transform.position);

        for (int i = 0; i < totalCharacters; i++)
        {
            Vector2 spawnPoint = new(UnityEngine.Random.Range(-spawnArea.x, spawnArea.x), UnityEngine.Random.Range(-spawnArea.y, spawnArea.y));

            while(!SpawnPointValid(spawnPoint, spawnPoints))
            {
                spawnPoint = new(UnityEngine.Random.Range(-spawnArea.x, spawnArea.x), UnityEngine.Random.Range(-spawnArea.y, spawnArea.y));
            }

            spawnPoints.Add(spawnPoint);

            Transform spawnedCharacter = Instantiate(characterPrefab, spawnPoint, Quaternion.identity).transform;

            remainingCharacters.Add(spawnedCharacter);
            visibleCharacters.Add(spawnedCharacter);
        }
    }

    // Returns whether the potential spawn point is too close to a previous spawn point
    private bool SpawnPointValid(Vector2 potentialPoint, List<Vector2> currentSpawnPoints)
    {
        foreach(Vector2 spawnPoint in currentSpawnPoints)
        {
            if(Vector3.SqrMagnitude(spawnPoint - potentialPoint) < 1)
            {
                return false;
            }
        }

        return true;
    }

    // Removes character from remaining and visible character lists
    public void KillCharacter(Transform character)
    {
        remainingCharacters.Remove(character);
        visibleCharacters.Remove(character);

        if(remainingCharacters.Count > 1)
        {
            PickTagged();
        }
        else
        {
            gameEndEvent?.Invoke(remainingCharacters[0]);
        }
    }

    // Removes character from visible list
    public void HideCharacter(Transform character)
    {
        visibleCharacters.Remove(character);
    }

    // Adds character to visible list if it isn't already
    public void ShowCharacter(Transform character)
    {
        if (!visibleCharacters.Contains(character))
        {
            visibleCharacters.Add(character);
        }
    }

    // Randomly picks the next tagged character from the remaining characters list
    private void PickTagged()
    {
        foreach (Transform character in remainingCharacters)
        {
            StartCoroutine(character.GetComponent<BaseController>().PauseMovement(tagPickTime));
        }

        int chosenIndex = UnityEngine.Random.Range(0, remainingCharacters.Count);

        remainingCharacters[chosenIndex].GetComponent<BaseController>().Tag(false);
    }

    // Spawn the next pickup prefab and add it to the active pickup list
    private void SpawnPickup()
    {
        Vector2 spawnPoint = new(UnityEngine.Random.Range(-spawnArea.x, spawnArea.x), UnityEngine.Random.Range(-spawnArea.y, spawnArea.y));

        Transform spawnedPickup = Instantiate(pickupPrefabs[pickupSpawnIndex], spawnPoint, Quaternion.identity).transform;
        activePickups.Add(spawnedPickup);

        pickupSpawnIndex = (pickupSpawnIndex + 1) % pickupPrefabs.Length;
    }

    // Remove pickup from the active pickup list and spawns new pickup
    public void RemovePickup(Transform pickup)
    {
        activePickups.Remove(pickup);

        SpawnPickup();
    }
}
