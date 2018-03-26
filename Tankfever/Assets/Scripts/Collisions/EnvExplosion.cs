using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvExplosion : MonoBehaviour {
	public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
	public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.

	private void Start ()
	{
	}


	private void OnTriggerEnter (Collider other)
	{
		// Unparent the particles.
		m_ExplosionParticles.transform.parent = null;

		// Play the particle system.
		m_ExplosionParticles.Play();

		// Once the particles have finished, destroy the gameobject they are on.
		ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
		Destroy (m_ExplosionParticles.gameObject, mainModule.duration);

		// Destroy object.
		Destroy (gameObject);
	}
}