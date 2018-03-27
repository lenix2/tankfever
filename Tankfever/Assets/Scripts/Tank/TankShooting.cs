using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;        
	public Transform m_FireTransform2; 
	public Transform m_FireTransform3; 
    public AudioSource m_ShootingAudio;    
    public AudioClip m_FireClip;         
    public float m_LaunchForce = 30f; 
	public Slider m_Slider;                            
	public Image m_FillImage; 

    
    private string m_FireButton;
	private int m_ammo;
	private List<int> m_itemList;
	float m_TimeSinceLastShot;


    private void OnEnable()
    {
		m_ammo = 5;
		m_Slider.value = m_ammo;
		m_itemList = new List<int>();
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;
		m_TimeSinceLastShot = 0f;
		m_itemList = new List<int>();
		m_ammo = 5;
		m_Slider.value = m_ammo;
    }

    private void Update()
    {
		m_TimeSinceLastShot = m_TimeSinceLastShot + Time.deltaTime;

        // Track the current state of the fire button and make decisions based on the current launch force.
		if (Input.GetButtonUp (m_FireButton))
		{
			// ... launch the shell.
			if (m_TimeSinceLastShot >= 0.5f && m_ammo > 0) {
				Fire ();
				// Set the slider's value appropriately.
				m_Slider.value = m_ammo;
			}
		}
    }

	public void AddItem(int i) {
		if (m_itemList.IndexOf (i) == -1) {
			m_itemList.Add (i);
		}

		AddAmmo (1);
	}

	public void AddAmmo(int i){
		m_ammo = m_ammo + i;

		if (m_ammo > 9) {
			m_ammo = 9;
		}

		m_Slider.value = m_ammo;
	}

    private void Fire()
    {
		// Change the clip to the firing clip and play it.
		m_ShootingAudio.clip = m_FireClip;
		m_ShootingAudio.Play ();

		m_ammo = m_ammo - 1;

        // Instantiate and launch the shell.
		m_TimeSinceLastShot = 0f;

		// Create an instance of the shell and store a reference to it's rigidbody.
		Rigidbody shellInstance =
			Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

		// Set the shell's velocity to the launch force in the fire position's forward direction.
		shellInstance.velocity = m_LaunchForce * m_FireTransform.forward; 

		if (m_itemList.IndexOf (2) > -1) {
			// Create an instance of the shell and store a reference to it's rigidbody.
			Rigidbody shellInstance2 =
				Instantiate (m_Shell, m_FireTransform2.position, m_FireTransform2.rotation) as Rigidbody;

			// Set the shell's velocity to the launch force in the fire position's forward direction.
			shellInstance2.velocity = m_LaunchForce * m_FireTransform2.forward;

			// Create an instance of the shell and store a reference to it's rigidbody.
			Rigidbody shellInstance3 =
				Instantiate (m_Shell, m_FireTransform3.position, m_FireTransform3.rotation) as Rigidbody;

			// Set the shell's velocity to the launch force in the fire position's forward direction.
			shellInstance3.velocity = m_LaunchForce * m_FireTransform3.forward;

			if (m_itemList.IndexOf (3) > -1) {
				ShellExplosion m_shell2 = shellInstance2.GetComponent<ShellExplosion> ();
				m_shell2.m_IsSuper = true;
				ShellExplosion m_shell3 = shellInstance3.GetComponent<ShellExplosion> ();
				m_shell3.m_IsSuper = true;
			}
			if (m_itemList.IndexOf (4) > -1) {
				ShellExplosion m_shell2 = shellInstance2.GetComponent<ShellExplosion> ();
				m_shell2.m_IsCluster = true;
				ShellExplosion m_shell3 = shellInstance3.GetComponent<ShellExplosion> ();
				m_shell3.m_IsCluster = true;
			}
		}

		if (m_itemList.IndexOf (3) > -1) {
			ShellExplosion m_shell = shellInstance.GetComponent<ShellExplosion> ();
			m_shell.m_IsSuper = true;
		}
		if (m_itemList.IndexOf (4) > -1) {
			ShellExplosion m_shell = shellInstance.GetComponent<ShellExplosion> ();
			m_shell.m_IsCluster = true;
		}
    }
}