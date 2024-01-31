using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Move")]
public class Move : ScriptableObject
{
    new public string name;

    public int damageMin, damageMax;
    public int hpChange;
    public bool cutsDamage;
    public Animal animalScaler;

    [TextArea]
    public string description;
    [TextArea]
    public string sideEffectDescription;

    public ElementType element;

    public List<AnimalChangeEffect> animalChanges = new List<AnimalChangeEffect>();

}

[System.Serializable]
public class AnimalChangeEffect
{
    public List<Animal> animals = new List<Animal>();
    public int change;
    public float rate = 1f;
    public bool onReceiveDamage;
    public bool scalesWithDamage;

}
