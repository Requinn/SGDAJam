using MichaelWolfGames.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DragonHandler : MonoBehaviour {
    [SerializeField]
    HealthManagerBase[] _damageableNode;

    // Use this for initialization
    void Start() {
        //subscribe to all the damageablenodes
        for (int i = 0; i < _damageableNode.Length; i++) {
            _damageableNode[i].OnTakeDamage += ReactToDamage;
            _damageableNode[i].OnDeath += ReactToKill;
        }
	}

    /// <summary>
    /// To handle the critical node dying
    /// </summary>
    private void ReactToKill() {
        Debug.Log("Node died");
    }

    /// <summary>
    /// How will we handle the damage being dealt to us?
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ReactToDamage(object sender, Damage.DamageEventArgs args) {
        Debug.Log("Damaged with severity: " + args.DamageValue);
        //when struck, speed up the animation of the dragon by X amount for some seconds
    }


}
