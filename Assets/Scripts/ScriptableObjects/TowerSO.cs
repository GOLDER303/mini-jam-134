using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TowerSO : ScriptableObject
{
    public TowerTypeEnum towerType;
    public List<Sprite> statesSprites;
}
