using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Action<Transform> gameEndEvent;


    [Header("Characters")]
    [SerializeField] private GameObject characterPrefab;

    private List<Transform> remainingCharacters = new();

    public List<Transform> RemainingCharacters { get { return remainingCharacters; } }

    private Transform taggedCharacter;

    public Transform TaggedCharacter { get { return taggedCharacter; } set { taggedCharacter = value; } }


    [Header("Game Settings")]
    [SerializeField] private Vector2 spawnArea;

    [SerializeField] private int aiCharacters;
    

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

        SpawnCharacters();
    }

    private void Start()
    {
        PickTagged();
    }

    // Spawns total number of ai characters at random but unique positions, adding them to remainingCharacters
    private void SpawnCharacters()
    {
        Transform player = PlayerController.Instance.transform;
        remainingCharacters.Add(player);

        List<Vector2> usedSpawnPoints = new();
        usedSpawnPoints.Add(player.position);

        for (int i = 0; i < aiCharacters; i++)
        {
            Vector2 spawnPoint = RandomSpawnPoint();

            while(!SpawnPointValid(spawnPoint, usedSpawnPoints))
            {
                spawnPoint = RandomSpawnPoint();
            }

            usedSpawnPoints.Add(spawnPoint);

            Transform spawnedCharacter = Instantiate(characterPrefab, spawnPoint, Quaternion.identity).transform;
            spawnedCharacter.name = (i + 1).ToString();

            remainingCharacters.Add(spawnedCharacter);
        }
    }

    // Returns a random spawn point within game bounds
    private Vector2 RandomSpawnPoint()
    {
        return new Vector2(UnityEngine.Random.Range(-spawnArea.x, spawnArea.x), UnityEngine.Random.Range(-spawnArea.y, spawnArea.y));
    }

    // Returns whether the potential spawn point is too close to a previous spawn point
    private bool SpawnPointValid(Vector2 potentialSpawnPoint, List<Vector2> currentSpawnPoints)
    {
        foreach(Vector2 spawnPoint in currentSpawnPoints)
        {
            if(Vector3.SqrMagnitude(spawnPoint - potentialSpawnPoint) < 1)
            {
                return false;
            }
        }

        return true;
    }

    // Removes character from remainingCharacters, checks if the game should end
    public void RemoveCharacter(Transform character)
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

    // Randomly picks a tagged character from the remainingCharacters
    private void PickTagged()
    {
        int randomIndex = UnityEngine.Random.Range(0, remainingCharacters.Count);

        remainingCharacters[randomIndex].GetComponent<BaseController>().Tag();
    }
}
