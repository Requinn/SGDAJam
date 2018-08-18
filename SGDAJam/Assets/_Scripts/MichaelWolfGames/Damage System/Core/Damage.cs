using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MichaelWolfGames.DamageSystem
{
    /// <summary>
    /// Static class used for data handling throughout the DamageSystem.
    /// Contains Declarations for:
    /// - DamageEventHandler Delegate.
    /// - DamageEventArgs Struct.
    /// 
    /// Michael K. Wolf
    /// January, 2018
    /// </summary>
    public static partial class Damage
    {
        /// <summary>
        /// Core delegate used for DamageEvents.
        /// </summary>
        /// <param name="sender">Object that is sending the DamageEvent.</param>
        /// <param name="args">Damage Event Arguments</param>
        public delegate void DamageEventHandler(object sender, DamageEventArgs args);

        /// <summary>
        /// Core delegate used for DamageEvent Mutators.
        /// These delegates will MODFIY the event arguments before they're interpreted by the Damagable.
        /// Example: Mutator that adds resistances or weaknesses to damage types.
        /// </summary>
        /// <param name="sender">Object that is sending the DamageEvent.</param>
        /// <param name="args">Mutated Damage Event Arguments</param>
        public delegate void DamageEventMutator(object sender, ref DamageEventArgs args);

        /// <summary>
        /// Arguments passed during DamageEvents.
        /// </summary>
        [System.Serializable]
        public struct DamageEventArgs
        {
            public float DamageValue;
            public DamageType DamageType;
            public Vector3 HitPoint;
            public Vector3 HitNormal;
            public Faction SourceFaction;
            public DamageEventType EventType;
            public DamageEventArgs(float damageValue, Vector3 hitPoint, DamageType type = DamageType.Default, Faction faction = Faction.Generic, DamageEventType eType = DamageEventType.HIT)
            {
                DamageValue = damageValue;
                DamageType = type;
                HitPoint = hitPoint;
                HitNormal = Vector3.up;
                SourceFaction = faction;
                EventType = eType;
            }
            public DamageEventArgs(float damageValue, Vector3 hitPoint, Vector3 hitNormal, DamageType type = DamageType.Default, Faction faction = Faction.Generic, DamageEventType eType = DamageEventType.HIT)
            {
                DamageValue = damageValue;
                DamageType = type;
                HitPoint = hitPoint;
                HitNormal = hitNormal;
                SourceFaction = faction;
                EventType = eType;
            }
        }

    }
}