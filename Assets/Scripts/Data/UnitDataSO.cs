using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Unit/UnitDataSO",menuName="UnitData")]
public class UnitDataSO : ScriptableObject
{
    
    public int movementRange = 10;
    public int attackStrength = 1;
    public int health = 1;
}
