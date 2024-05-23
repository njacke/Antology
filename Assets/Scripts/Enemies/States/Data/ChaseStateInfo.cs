using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New ChaseStateInfo")]

public class ChaseStateInfo : ScriptableObject
{
    public float MovementSpeed = 3f;
    public float RotationSpeed = 1.5f;
    public float MaxChaseRange = 5f;
    public float MinChaseRange = 1f;
}
