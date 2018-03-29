using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushCollisionNoTrigger : MonoBehaviour 
{
	public float m_ExplosionForce = 100f;
	public float m_ExplosionRadius =20f;

	void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.layer == 9) {
			
			// ... and find their rigidbody.
			Rigidbody targetRigidbody = other.collider.GetComponent<Rigidbody> ();

			// Find the TankHealth script associated with the rigidbody.
			TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();

			// Add an explosion force.
			targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);

			// Calculate the amount of damage the target should take based on it's distance from the shell.
			float damage = 20f;

			// Deal this damage to the tank.
			targetHealth.TakeDamage (damage);
		}
	}
}