using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Animal")]
public class Animal : ScriptableObject
{
    new public string name;

    public float lifeEnergy;
    public float defencePower;
    public float growthRate;
    public float decayRate;
    public Animal auxCreate;
    public float createPerStep;

    public int startAmount;
    

    //public List<AnimalTagType> animalTags;

    //public List<AnimalTagType> dietTags;

    //public List<AnimalTagType> excludeDietTags;

    public List<Animal> dietAnimals;



    //public bool WillEat(Animal animal)
    //{
    //    if (name == animal.name) return false;
    //    foreach (AnimalTagType tag in animal.animalTags)
    //    {
    //        if (excludeDietTags.Contains(tag))
    //        {
    //            return false;
    //        }
    //    }
    //    foreach (AnimalTagType tag in dietTags)
    //    {
    //        if (!animal.animalTags.Contains(tag))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

}

