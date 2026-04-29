using UnityEngine;

public class BoardController : MonoBehaviour
{
    public InputReader inputReader;

    private Vector2 input;

    public float tiltSpeed = 50f;
    public float maxTilt = 15f;

    float tiltX, tiltZ;

    private void OnEnable()
    {
        inputReader.MoveEvent += OnMove;
    }

    private void OnDisable()
    {
        inputReader.MoveEvent -= OnMove;
    }

    private void OnMove(Vector2 value)
    {
        input = value;
        
        //Debug.Log("Board received: " + value);
    }

		/*
    void Update()
    {
        tiltX += input.y * tiltSpeed * Time.deltaTime;
        tiltZ -= input.x * tiltSpeed * Time.deltaTime;

        tiltX = Mathf.Clamp(tiltX, -maxTilt, maxTilt);
        tiltZ = Mathf.Clamp(tiltZ, -maxTilt, maxTilt);

        transform.rotation = Quaternion.Euler(tiltX, 0, tiltZ);   
        
        //Debug.Log(transform.rotation);
    }
		*/
	
    void FixedUpdate()
		{
		    tiltX += input.y * tiltSpeed * Time.fixedDeltaTime;
		    tiltZ -= input.x * tiltSpeed * Time.fixedDeltaTime;
		
		    tiltX = Mathf.Clamp(tiltX, -maxTilt, maxTilt);
		    tiltZ = Mathf.Clamp(tiltZ, -maxTilt, maxTilt);
		
		    transform.rotation = Quaternion.Euler(tiltX, 0, tiltZ);
		    
		    //Debug.Log(transform.rotation);
		}

}
