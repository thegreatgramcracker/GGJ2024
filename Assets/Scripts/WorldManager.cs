using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public List<Animal> animals;

    public Dictionary<Animal, int> animalAmounts = new Dictionary<Animal, int>();

    public int changeSteps;

    public float startVarience;

    // Start is called before the first frame update
    void Start()
    {
        CreateStartingAnimals();
        while (changeSteps > 0)
        {
            ProcessEnvironmentChange();
            changeSteps--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateStartingAnimals()
    {
        foreach (Animal animal in animals)
        {
            animalAmounts[animal] = Mathf.RoundToInt(Random.Range(animal.startAmount * (1f - startVarience), animal.startAmount * (1f + startVarience)));
        }
        
    }

    public void ProcessEnvironmentChange()
    {
        Dictionary<Animal, int> animalChanges = new Dictionary<Animal, int>();
        //process natural growth
        foreach (Animal animal in animalAmounts.Keys)
        {
            //animal growth is based on amount of prey alive
            //animal decay is based on amount of predators alive
            float preyAbundanceModifier = 1f;
            float predatorAbundanceModifier = 1f;
            foreach (Animal prey in animal.dietAnimals)
            {
                preyAbundanceModifier += animalAmounts[prey] / prey.startAmount * Random.Range(0.4f, 0.6f);
            }
            foreach (Animal predator in animalAmounts.Keys)
            {
                if (predator.dietAnimals.Contains(animal))
                {
                    predatorAbundanceModifier += animalAmounts[predator] / predator.startAmount * Random.Range(0.4f, 0.6f);
                }
            }

            int animalChange = Mathf.RoundToInt(animalAmounts[animal] * (animal.growthRate / 100f) * preyAbundanceModifier) - 
                Mathf.RoundToInt(animalAmounts[animal] * (animal.decayRate / 100f) * predatorAbundanceModifier);
            animalChanges[animal] = animalChange;
        }
        foreach (Animal animal in animalAmounts.Keys)
        {
            if (animal.auxCreate != null)
            {
                if (animalChanges.ContainsKey(animal.auxCreate))
                {
                    animalChanges[animal.auxCreate] += Mathf.FloorToInt(animal.createPerStep * animalAmounts[animal]);
                }
                else
                {
                    animalChanges[animal.auxCreate] = Mathf.FloorToInt(animal.createPerStep * animalAmounts[animal]);
                }

            }
        }

        foreach (Animal key in animalChanges.Keys)
        {
            animalAmounts[key] += animalChanges[key];
            if (animalAmounts[key] < 0)
            {
                animalAmounts[key] = 0;
            }
        }
        

    }

    public Animal GetHighestLifeEnergy()
    {
        Animal highestKey = null;
        float highestAmount = 0f;
        foreach (Animal animal in animalAmounts.Keys)
        {
            if (animal.lifeEnergy * animalAmounts[animal] > highestAmount)
            {
                highestAmount = animal.lifeEnergy * animalAmounts[animal];
                highestKey = animal;
            }
        }
        return highestKey;
    }

}
