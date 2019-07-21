using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileCamera : MonoBehaviour
{
	public float ScrollSpeed = 3f;
    public float ScreenBorderMargin = 10f;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		Vector3 pos = Input.mousePosition;

        #region DebugCatching
        //if (Input.GetKey(KeyCode.E))
        //{
        //    int sas = 100;
        //}
        #endregion

        float m = ScreenBorderMargin;
        float dirx = pos.x < m ? 1 : pos.x > Screen.width - m ? -1 : 0;
		float diry = pos.y < m ? -1 : pos.y > Screen.height - m ? 1 : 0;
		Vector3 dir = new Vector3(dirx, diry, 0f);
		//Debug.Log(dir * ScrollSpeed);
        transform.Translate(dir * Time.deltaTime * ScrollSpeed, Space.World);
    }
}
