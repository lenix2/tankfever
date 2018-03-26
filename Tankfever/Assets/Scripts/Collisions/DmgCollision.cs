using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgCollision : MonoBehaviour {
	public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".


	private void Start ()
	{
	}


	private void OnTriggerEnter (Collider other)
	{
			// ... and find their rigidbody.
			Rigidbody targetRigidbody = other.GetComponent<Rigidbody> ();

			// Find the TankHealth script associated with the rigidbody.
			TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();

			// Calculate the amount of damage the target should take based on it's distance from the shell.
			float damage = 1000f;

			// Deal this damage to the tank.
			targetHealth.TakeDamage (damage);
	}
}
