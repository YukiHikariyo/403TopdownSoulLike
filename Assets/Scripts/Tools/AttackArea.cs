using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public enum AttackerType
    {
        Player,
        Enemy,
        Trap,
    }

    [Tooltip("攻击者类别")] public AttackerType attackerType;
}
