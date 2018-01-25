using UnityEngine;
using UnityEngine.SceneManagement;

//TODO Fix lighting bug

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelWin;

    Rigidbody rigidBody; // Naming of Rigidbody
    AudioSource audioSource; // Naming of Audiosource

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {
        // Somewhere stop sound on death
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; } // Ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //Do Nothing
                break;
            case "Finish":
                StartLevelWinSequence();
                break;
            default:
                StartDeathSequence();
                break;



        }
    }

    private void StartLevelWinSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelWin);
        Invoke("LoadNextLevel", 1f); // Parameterise time
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        Invoke("LoadFirstLevel", 3f); // Parameterise time
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // Allow for more 2 levels
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        float addThrust = mainThrust * Time.deltaTime;


        if (Input.GetKey(KeyCode.W)) // Can thrust while rotating
        {
            ApplyThrust(addThrust);
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust(float addThrust)
    {
        rigidBody.AddRelativeForce(Vector3.up * addThrust);
        if (!audioSource.isPlaying) // So audio doesn't play every frame
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation

    }

 

}
