using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TowerSO : ScriptableObject
{
    public int numberOfStates;
    public int currentState = 0;
    public List<Sprite> statesSprites;
}
