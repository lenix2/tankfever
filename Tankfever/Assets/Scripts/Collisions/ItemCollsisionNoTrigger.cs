using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemCollsisionNoTrigger : MonoBehaviour 
{
	public ParticleSystem m_ExplosionParticles; 
	private int m_ItemType;
	public Sprite m_Item_1;
	public Sprite m_Item_2;
	public Sprite m_Item_3;
	public Sprite m_Item_4;
	public Sprite m_Item_5;
	public Sprite m_Item_6;
	public Image m_Image;

	private void Start()
	{
		float rdm = Random.Range (0f, 100f);
		float val = 100 / 6;

		if (rdm < val) {
			m_ItemType = 1;
			m_Image.sprite = m_Item_1;
		} else if (rdm < val * 2) {
			m_ItemType = 2;
			m_Image.sprite = m_Item_2;
		} else if (rdm < val * 3) {
			m_ItemType = 3;
			m_Image.sprite = m_Item_3;
		} else if (rdm < val * 4) {
			m_ItemType = 4;
			m_Image.sprite = m_Item_4;
		} else if (rdm < val * 5) {
			m_ItemType = 5;
			m_Image.sprite = m_Item_5;
		} else if (rdm < val * 6) {
			m_ItemType = 6;
			m_Image.sprite = m_Item_6;
		} else {
			m_ItemType = 1;
			m_Image.sprite = m_Item_1;
		}
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