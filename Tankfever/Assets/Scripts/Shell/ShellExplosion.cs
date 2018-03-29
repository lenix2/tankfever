using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
	public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
	public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
	public float m_MaxDamage = 30f;                    // The amount of damage done if the explosion is centred on a tank.
	public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
	public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
	public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
	private float m_TimeSinceShot;  


	public bool m_IsCluster = false;
	public bool m_IsSuper = false;
	public Transform[] m_Transforms;
	public Rigidbody m_Shell; 

	private void Start ()
	{
		// If it isn't destroyed by then, destroy the shell after it's lifetime.
		Destroy (gameObject, m_MaxLifeTime);
		m_TimeSinceShot = 0f;

		if (m_IsSuper) {
			MeshRenderer[] renderers = this.gameObject.GetComponentsInChildren<MeshRenderer> ();

			// Go through all the renderers...
			for (int i = 0; i < renderers.Length; i++)
			{
				// ... set their material color to the color specific to this tank.
				renderers[i].material.color = new Color(1f, 1f, 0f, 1f);
			}
		}

		if (m_IsCluster) {
			this.transform.localScale = new Vector3 (5f, 5f, 4f);
		} else {
			this.transform.localScale = new Vector3 (3f, 3f, 3f);
		}
	}

	private void Update()
	{
		m_TimeSinceShot = m_TimeSinceShot + Time.deltaTime;
	}


	private void OnTriggerEnter (Collider other)
	{
		if (m_TimeSinceShot > 0.01f) {

			if (other.gameObject.layer == 9) {
				Rigidbody targetRigidbody = other.GetComponent<Rigidbody> ();

				if (m_ExplosionForce > 0) {
					// Add an explosion force.
					targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);
				}

				// Find the TankHealth script associated with the rigidbody.
				TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();

				// Calculate the amount of damage the target should take based on it's distance from the shell.
				float damage = CalculateDamage ();

				// Deal this damage to the tank.
				targetHealth.TakeDamage (damage);
			}

			// Unparent the particles from the shell.
			if (m_ExplosionParticles != null) {
				m_ExplosionParticles.transform.parent = null;

				// Play the particle system.
				m_ExplosionParticles.Play ();

				// Once the particles have finished, destroy the gameobject they are on.
				ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
				Destroy (m_ExplosionParticles.gameObject, mainModule.duration);
			}


			if (m_IsCluster) {
				Cluster ();
			}

			if (other.gameObject.layer == 10) {
				Destroy (other.gameObject);
			}

			// Destroy the shell.
			Destroy (gameObject);

		}
	}

	private void Cluster() {
		for (int i = 0; i < m_Transforms.Length; i++)
		{
			if (m_Shell != null) {
				// ... set their material color to the color specific to this tank.
				Rigidbody shellInstance =
					Instantiate (m_Shell, m_Transforms [i].position, m_Transforms [i].rotation) as Rigidbody;

				// Set the shell's velocity to the launch force in the fire position's forward direction.
				shellInstance.velocity = 30f * m_Transforms [i].forward; 

				ShellExplosion m_shell = shellInstance.GetComponent<ShellExplosion> ();
				m_shell.m_IsCluster = false;

				if (m_IsSuper) {
					m_shell.m_IsSuper = true;
				}

			}
		}
	}

	private float CalculateDamage ()
	{
		float damage = m_MaxDamage;

		if (m_IsSuper) {
			damage = damage * 3;
		}

		return damage;
	}
}