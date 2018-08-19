using UnityEngine;
using System.Collections;
using MichaelWolfGames.DamageSystem;

public class DeathEvent :  HealthManagerEventListenerBase {
	protected override void DoOnDeath() {
		Debug.Log(this.name  + " has died!");
	}

	protected override void DoOnRevive() {
		//throw new System.NotImplementedException();
	}

	protected override void DoOnTakeDamage(object sender, Damage.DamageEventArgs damageEventArgs) {
		//throw new System.NotImplementedException();
	}
}
