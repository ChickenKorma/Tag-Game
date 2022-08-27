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

    [SerializeField] private GameObject characterPrefab; 

    [SerializeField] private int totalCharacters;

    [SerializeField] private Vector2 spawnArea;

    [SerializeField] private float tagPickTime;

    [SerializeField] private List<Transform> remainingCharacters = new();  

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

        remainingCharacters.Add(PlayerController.Instance.transform);

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
            Vector2 spawnPoint = new Vector2(UnityEngine.Random.Range(-spawnArea.x, spawnArea.x), UnityEngine.Random.Range(-spawnArea.y, spawnArea.y));

            while(!SpawnPointValid(spawnPoint, spawnPoints))
            {
                spawnPoint = new Vector2(UnityEngine.Random.Range(-spawnArea.x, spawnArea.x), UnityEngine.Random.Range(-spawnArea.y, spawnArea.y));
            }

            spawnPoints.Add(spawnPoint);

            Transform spawnedCharacter = Instantiate(characterPrefab, spawnPoint, Quaternion.identity).transform;

            remainingCharacters.Add(spawnedCharacter);
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

        if(remainingCharacters.Count > 1)
        {
            PickTagged();
        }
        else
        {
            gameEndEvent?.Invoke(remainingCharacters[0]);
        }
    }

    // Randomly picks the next tagged character from the remaining characters list
    private void PickTagged()
    {
        int chosenIndex = UnityEngine.Random.Range(0, remainingCharacters.Count);

        remainingCharacters[chosenIndex].GetComponent<BaseController>().Tag(tagPickTime);
    }
}
