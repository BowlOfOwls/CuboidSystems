using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MaterialSO : ScriptableObject
{
    public InteractableSO input;
    public InteractableSO output;
    public int spawnRatio;
}
