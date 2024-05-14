using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField] private AbilityInfo _abilityInfo;
    public AbilityInfo AbilityInfo { get { return _abilityInfo; } }
}
