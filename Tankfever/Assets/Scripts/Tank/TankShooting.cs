using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;        
    public AudioSource m_ShootingAudio;    
    public AudioClip m_FireClip;         
    public float m_LaunchForce = 15f; 

    
    private string m_FireButton;    
	float m_TimeSinceLastShot;


    private void OnEnable()
    {
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;
		m_TimeSinceLastShot = 0f;
    }
    

    private void Update()
    {
		m_TimeSinceLastShot = m_TimeSinceLastShot + Time.deltaTime;

        // Track the current state of the fire button and make decisions based on the current launch force.
		if (Input.GetButtonUp (m_FireButton))
		{
			// ... launch the shell.
			if (m_TimeSinceLastShot >= 0.3f) {
				Fire ();
			}
		}
    }


    private void Fire()
    {
		// Change the clip to the firing clip and play it.
		m_ShootingAudio.clip = m_FireClip;
		m_ShootingAudio.Play ();

        // Instantiate and launch the shell.
		m_TimeSinceLastShot = 0f;

		// Create an instance of the shell and store a reference to it's rigidbody.
		Rigidbody shellInstance =
			Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

		// Set the shell's velocity to the launch force in the fire position's forward direction.
		shellInstance.velocity = m_LaunchForce * m_FireTransform.forward; ;


    }
}