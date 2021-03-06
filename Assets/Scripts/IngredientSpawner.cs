using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpawnerState
{
    Balanced,
    Procedural
}
public class IngredientSpawner : MonoBehaviour
{
    public List<Ingredient> ingredientsList = new List<Ingredient>();

    public List<Transform> spawnPoints = new List<Transform>();
    public int amountToSpawn = 4;

    [HideInInspector]
    public List<Ingredient> availableIngredients = new List<Ingredient>();
    
    public event Action OnIngredientsSpawned;

    public SpawnerState spawnerType;

    private void Start()
    {
        SpawnIngredients();
    }

    private void Update()
    {
        if (availableIngredients.Count == 0)
        {
            SpawnIngredients();
        }
    }

    public void SpawnIngredients()
    {
        int amountSpawned = 0;

        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        if (spawnerType == SpawnerState.Procedural)
        {
            //first make sure there is at least one of every ingredient out
            foreach (Ingredient ingredient in ingredientsList)
            {
                Transform randSpawnPoint = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];

                Ingredient instance = Instantiate(ingredient, randSpawnPoint.position, Quaternion.identity);

                availableSpawnPoints.Remove(randSpawnPoint);
                availableIngredients.Add(instance);
                amountSpawned++;
            }

            //then spawn random ones at the remaining spawn points
            for (int i = 0; i < amountToSpawn - amountSpawned; i++)
            {
                Transform randSpawnPoint = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];

                Ingredient randIngredient = ingredientsList[Random.Range(0, ingredientsList.Count)];
            
                Ingredient instance = Instantiate(randIngredient, randSpawnPoint.position, Quaternion.identity);
            
                availableIngredients.Add(instance);
                availableSpawnPoints.Remove(randSpawnPoint);
            }   
        }
        else
        {
            int totalSpawnPoints = availableSpawnPoints.Count;
            
            int rest = totalSpawnPoints % 3;

            int amountPerIngredients = (totalSpawnPoints - rest) / 3;
            
            foreach (Ingredient ingredient in ingredientsList)
            {
                for (int i = 0; i < amountPerIngredients; i++)
                {
                    Transform randSpawnPoint = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];
                    
                    Ingredient instance = Instantiate(ingredient, randSpawnPoint.position, Quaternion.identity);
            
                    availableIngredients.Add(instance);
                    availableSpawnPoints.Remove(randSpawnPoint);
                }
            }

            for (int i = 0; i < rest; i++)
            {
                Transform randSpawnPoint = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];

                Ingredient randIngredient = ingredientsList[Random.Range(0, ingredientsList.Count)];
            
                Ingredient instance = Instantiate(randIngredient, randSpawnPoint.position, Quaternion.identity);
            
                availableIngredients.Add(instance);
                availableSpawnPoints.Remove(randSpawnPoint);
            }
        }

        OnIngredientsSpawned?.Invoke();
    }

    public List<Ingredient> GetAvailableIngredientsOfType(Ingredients type)
    {
        List<Ingredient> ingredients = new List<Ingredient>();

        foreach (Ingredient ingredient in availableIngredients)
        {
            if(ingredient.ingredientType == type)
                ingredients.Add(ingredient);
        }

        return ingredients;
    }
}
