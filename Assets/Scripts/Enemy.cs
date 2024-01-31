using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    new public string name;
    public int maxHP;
    public int hp;
    public Sprite sprite;

    public Move[] moveList;

    public ElementType weakness;


}
