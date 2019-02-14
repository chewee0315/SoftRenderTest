using UnityEngine;
using System.Collections;

public class Haikei : MonoBehaviour {

	protected const float	DEPTH = 100.0f;

	public Texture	texture;
	public Camera	main_camera;

	// ================================================================ //

	void 	Start()
	{
		do {

			GameObject	camera_go = GameObject.FindGameObjectWithTag("MainCamera");	

			if(camera_go == null) {
				Debug.Log("Can't find \"MainCamera\".");
				break;
			}

			this.main_camera = camera_go.GetComponent<Camera>();

			if(this.main_camera == null) {
				Debug.Log("Can't find Camera Component.");
				break;
			}

			MeshRenderer	renderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
			
			if(renderer == null) {
				Debug.Log("Can't find MeshRenderer Component.");
				break;				
			}

			if(this.texture == null) {
				Debug.Log("texture not setted.\n");
				break;
			}

			// ---------------------------------------------------------------- //			

			renderer.material.mainTexture = this.texture;

			this.transform.parent = this.main_camera.transform;

			Vector3		position = this.transform.position;
	
			position.x = 0.0f;
			position.y = 0.0f;
			position.z = DEPTH;
	
			this.transform.localPosition = position;
			this.transform.localRotation = Quaternion.identity;

			float	fov = this.main_camera.fieldOfView;

			float	y_size = Mathf.Ceil(2.0f*DEPTH*Mathf.Tan(fov*Mathf.Deg2Rad/2.0f));
			float	x_size = y_size*this.main_camera.aspect;

			float	texture_aspect = (float)this.texture.width/(float)this.texture.height;

			float	x_scale = x_size/texture_aspect;
			float	y_scale = y_size;
			
			float	scale = Mathf.Max(x_scale, y_scale);

			this.transform.localScale = new Vector3(scale*texture_aspect, scale, 1.0f);

		} while(false);
	}
}
