using UnityEngine;
using System.Collections;

    public class FlyCamera : MonoBehaviour
    {

        /*
        Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
        Converted to C# 27-02-13 - no credit wanted.
        Simple flycam I made, since I couldn't find any others made public.  
        Made simple to use (drag and drop, done) for regular keyboard layout  
        wasd : basic movement
        shift : Makes camera accelerate
        space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/


        public float mainSpeed = 100.0f; //regular speed
        public float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
        public float maxShift = 1000.0f; //Maximum speed when holdin gshift
        public float camSens = 0.25f; //How sensitive it with mouse
        private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
        private float totalRun = 1.0f;
        private bool firstTime = true;
        private bool lockTranslation = false;    // camera controls locked (doesn't move)
        private bool lockAngles = false;

        private bool DebugDisplayOn = true;

        void Start()
        {
		Debug.Log("DevCamera Controls: Mouse+Rt-Mouse=Rotation, WASD=Move"); //, Q=Lock Rotation, E=Lock Translation, Space(hold)=Allow Vertical Move");
        }
        void Update()
        {
			if (Input.GetMouseButton(1))
			{
				if (!lockAngles)
				{
					if (firstTime)
					{
						firstTime = false;
						lastMouse = Vector3.zero;
					}
					else
					{
						lastMouse = Input.mousePosition - lastMouse;
					}
					lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
					lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
					transform.eulerAngles = lastMouse;
					lastMouse = Input.mousePosition;
					//Mouse  camera angle done.  
				}
			}
			else
			{
				firstTime = true;
			}

			if (!lockTranslation)
            {
                //Keyboard commands
                Vector3 p = GetBaseInput();
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    totalRun += Time.deltaTime;
                    p = p * totalRun * shiftAdd;
                    p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                    p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                    p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
                }
                else
                {
                    totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                    p = p * mainSpeed;
                }

                p = p * Time.deltaTime;
                Vector3 newPosition = transform.position;
                if (!Input.GetKey(KeyCode.Space))
                { //If player wants to move on X and Z axis only
                    transform.Translate(p);
                    newPosition.x = transform.position.x;
                    newPosition.z = transform.position.z;
                    transform.position = newPosition;
                }
                else
                {
                    transform.Translate(p);
                }
            }

#if false
		if (Input.GetKeyDown(KeyCode.Q))
            {
                lockAngles = !lockAngles;
                if (lockAngles)
                    firstTime = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                lockTranslation = !lockTranslation;
            }
#endif
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                DebugDisplayOn = !DebugDisplayOn;
            }
        }

        private Vector3 GetBaseInput()
        { //returns the basic values, if it's 0 than it's not active.
            Vector3 p_Velocity = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                p_Velocity += new Vector3(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                p_Velocity += new Vector3(0, 0, -1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                p_Velocity += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                p_Velocity += new Vector3(1, 0, 0);
            }
            return p_Velocity;
        }

        void OnGUI()
        {
#if UNITY_EDITOR
            if (DebugDisplayOn)
            {
                int dyText = 15;
                int y = 10;
                GUI.Label(new Rect(10, y, 350, 20), "Tilde (~) key toggles this diplay:"); y += dyText;
                GUI.Label(new Rect(10, y, 350, 20), "Dev Fly Camera"); y += dyText;
                GUI.Label(new Rect(10, y, 350, 20), "   WASD = Move & Mouse-Look"); y += dyText;
				GUI.Label(new Rect(10, y, 350, 20), "   RightMouse+MouseMove = Rotation"); y += dyText;
				//GUI.Label(new Rect(10, y, 350, 20), "   Q  Lock rotation"); y += dyText;
				//GUI.Label(new Rect(10, y, 350, 20), "   E  Lock movement"); y += dyText;
				GUI.Label(new Rect(10, y, 350, 20), "   hold SPACE = enable Vertical motion"); y += dyText;

            }
#endif
        }


    }
