using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
	public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
	public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
	public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
	public float m_MaxDamage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
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
			this.transform.localScale = new Vector3 (3f, 3f, 2f);
		} else {
			this.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		}
	}

	private void Update()
	{
		m_TimeSinceShot = m_TimeSinceShot + Time.deltaTime;
	}


	private void OnTriggerEnter (Collider other)
	{
		if (m_TimeSinceShot > 0.01f) {
			
			// Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
			Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius, m_TankMask);

			// Go through all the colliders...
			for (int i = 0; i < colliders.Length; i++) {
				// ... and find their rigidbody.
				Rigidbody targetRigidbody = colliders [i].GetComponent<Rigidbody> ();

				// If they don't have a rigidbody, go on to the next collider.
				if (!targetRigidbody)
					continue;

				// Add an explosion force.
				targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);

				// Find the TankHealth script associated with the rigidbody.
				TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();

				// If there is no TankHealth script attached to the gameobject, go on to the next collider.
				if (!targetHealth)
					continue;

				// Calculate the amount of damage the target should take based on it's distance from the shell.
				float damage = CalculateDamage (targetRigidbody.position);

				// Deal this damage to the tank.
				targetHealth.TakeDamage (damage);
			}

			// Unparent the particles from the shell.
			if (m_ExplosionParticles != null) {
				m_ExplosionParticles.transform.parent = null;

				// Play the particle system.
				m_ExplosionParticles.Play ();

				// Play the explosion sound effect.
				m_ExplosionAudio.Play ();

				// Once the particles have finished, destroy the gameobject they are on.
				ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
				Destroy (m_ExplosionParticles.gameObject, mainModule.duration);
			}


			if (m_IsCluster) {
				Cluster ();
			}

			// Destroy the shell.
			Destroy (gameObject);

		}
	}

	private void Cluster() {
		for (int i = 0; i < m_Transforms.Length; i++)
		{
			// ... set their material color to the color specific to this tank.
			Rigidbody shellInstance =
				Instantiate (m_Shell, m_Transforms[i].position, m_Transforms[i].rotation) as Rigidbody;

			// Set the shell's velocity to the launch force in the fire position's forward direction.
			shellInstance.velocity = 30f * m_Transforms[i].forward; 

			ShellExplosion m_shell = shellInstance.GetComponent<ShellExplosion> ();
			m_shell.m_IsCluster = false;

			if (m_IsSuper) {
				m_shell.m_IsSuper = true;
			}
		}
	}

	private float CalculateDamage (Vector3 targetPosition)
	{
		// Create a vector from the shell to the target.
		Vector3 explosionToTarget = targetPosition - transform.position;

		// Calculate the distance from the shell to the target.
		float explosionDistance = explosionToTarget.magnitude;

		// Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
		float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

		// Calculate damage as this proportion of the maximum possible damage.
		float damage = relativeDistance * m_MaxDamage;

		if (m_IsSuper) {
			damage = damage * 3;
		}

		// Make sure that the minimum damage is always 0.
		damage = Mathf.Max (0f, damage);

		return damage;
	}
}