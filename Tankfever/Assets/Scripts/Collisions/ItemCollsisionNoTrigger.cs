using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollsisionNoTrigger : MonoBehaviour 
{
	public ParticleSystem m_ExplosionParticles; 
	private int m_ItemType;

	private void Start()
	{
		m_ItemType = 2;
	}

	void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.layer == 9) {
			
			// ... and find their rigidbody.
			Rigidbody targetRigidbody = other.collider.GetComponent<Rigidbody> ();

			// Find the TankHealth script associated with the rigidbody.
			TankShooting tankShooting = targetRigidbody.GetComponent<TankShooting> ();

			// Calculate the amount of damage the target should take based on it's distance from the shell.
			float damage = 0f;

			// Unparent the particles.
			m_ExplosionParticles.transform.parent = null;

			// Play the particle system.
			m_ExplosionParticles.Play();
			ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
			Destroy (m_ExplosionParticles.gameObject, mainModule.duration);

			// Deal this damage to the tank.
			tankShooting.AddItem (m_ItemType);
			Destroy (gameObject);
		}
	}
}