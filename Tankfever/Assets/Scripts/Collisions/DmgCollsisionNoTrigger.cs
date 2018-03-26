using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgCollsisionNoTrigger : MonoBehaviour 
{
	void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.layer == 9) {
			
			// ... and find their rigidbody.
			Rigidbody targetRigidbody = other.collider.GetComponent<Rigidbody> ();

			// Find the TankHealth script associated with the rigidbody.
			TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();

			// Calculate the amount of damage the target should take based on it's distance from the shell.
			float damage = 1000f;

			// Deal this damage to the tank.
			targetHealth.TakeDamage (damage);
		}
	}
}