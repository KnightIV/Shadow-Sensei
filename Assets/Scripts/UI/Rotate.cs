using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
	
	void Update () {
	    Vector3 rotation = Vector3.forward * Time.deltaTime * -100;
        transform.Rotate(rotation);
	}
}
