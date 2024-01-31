using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimalMenu : MonoBehaviour
{
    public WorldManager worldManager;
    public GameObject AnimalTrackerObject;
    List<AnimalTracker> animalTrackers = new List<AnimalTracker>();
    // Start is called before the first frame update
    IEnumerator Start()
    {
        foreach (Animal animal in worldManager.animals)
        {
            GameObject tracker = Instantiate(AnimalTrackerObject, this.transform);
            tracker.GetComponent<AnimalTracker>().myAnimal = animal;
            //animalTrackers.Add(tracker.GetComponent<AnimalTracker>());
        }
        yield return null;
        foreach (Transform child in transform)
        {
            Animal childAnimal = child.GetComponent<AnimalTracker>().myAnimal;
            child.GetComponent<AnimalTracker>().displayAmount = worldManager.animalAmounts[childAnimal];
            child.GetComponent<AnimalTracker>().Amount = worldManager.animalAmounts[childAnimal];
            //child.GetComponentInChildren<TextMeshProUGUI>().text = childAnimal.name + ": " + worldManager.animalAmounts[childAnimal];
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform)
        {
            Animal childAnimal = child.GetComponent<AnimalTracker>().myAnimal;
            child.GetComponent<AnimalTracker>().Amount = worldManager.animalAmounts[childAnimal];
            //child.GetComponentInChildren<TextMeshProUGUI>().text = childAnimal.name + ": " + worldManager.animalAmounts[childAnimal];
        }
    }
}
