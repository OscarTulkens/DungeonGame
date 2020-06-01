using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "TileSpawns/Monster", order = 1)]
public class MonsterObject : ScriptableObject
{
    public string MonsterName;
    public GameObject MonsterPrefab;
    public int MonsterHealth;
    public int MonsterAttack;
    public int difficultyLevel;
    public List<TreasureObject> _treasureObjects;
}
