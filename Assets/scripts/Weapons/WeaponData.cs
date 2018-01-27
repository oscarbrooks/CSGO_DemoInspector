using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public string Name;
    public GameObject ModelPrefab;
}