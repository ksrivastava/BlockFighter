using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject playerOne;
	public GameObject playerTwo;
	public GameObject levelBounds;
	public GameObject littleBox;
	public GameObject pointsBar;

	public float minOrthographicSize = 10;
	public float maxOrthographicSize = 20;
	public float thresholdDist = 10f;
	public float lerpDuration = 0.5f;

	public bool cameraLock = false;



	void FixedUpdate(){
		if(playerOne != null && playerTwo != null) {
			bool playerOneInCamera = (playerOne) ? checkObjectInCamera(playerOne.transform.position) : false;
			bool playerTwoInCamera = (playerTwo) ? checkObjectInCamera(playerTwo.transform.position) : false;
			bool bothInCamera = playerOneInCamera && playerTwoInCamera;

			var dist = Vector3.Distance (playerOne.transform.position, playerTwo.transform.position);

			if (!bothInCamera && !cameraLock){
				StartCoroutine (changeCameraSize (Camera.main.orthographicSize, maxOrthographicSize, lerpDuration, Time.time,
				                                  Camera.main.transform.position, levelBounds.transform.position));
			}else if (bothInCamera && dist <= thresholdDist && Camera.main.orthographicSize != minOrthographicSize && !cameraLock) {
				StartCoroutine (changeCameraSize (Camera.main.orthographicSize, minOrthographicSize, lerpDuration, Time.time, 
				                Camera.main.transform.position, 
				                CenterCamera(playerOne.transform.position,playerTwo.transform.position)));	
			}
		}
	}

	bool checkObjectInCamera(Vector3 position){
		var bottomLeft = Camera.main.ViewportToWorldPoint (Vector3.zero);
		var topRight = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 0));
		return (position.x > bottomLeft.x && position.x < topRight.x &&
		        position.y > bottomLeft.y && position.y < topRight.y);
	}

	bool checkPointInBounds(GameObject bounds, Vector3 position){
		var bottomLeft = bounds.camera.ViewportToWorldPoint (Vector3.zero);
		var topRight = bounds.camera.ViewportToWorldPoint (new Vector3 (1, 1, 0));
		return (position.x > bottomLeft.x && position.x < topRight.x &&
		        position.y > bottomLeft.y && position.y < topRight.y);
	}

	IEnumerator changeCameraSize(float oldSize, float newSize, float duration, float startTime, Vector3 oldPos = default(Vector3), Vector3 newPos = default(Vector3)){
		while (cameraLock == true) {
			yield return null;
		}

		cameraLock = true;
		while (Camera.main.orthographicSize != newSize) {
			float i = (Time.time - startTime) / duration; 
			Camera.main.orthographicSize = Mathf.Lerp(oldSize, newSize, i);
			if(oldPos != newPos){
				var pos = Vector3.Lerp(oldPos,newPos,i);
				pos.z = Camera.main.transform.position.z;
				Camera.main.transform.position = pos;
			}
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		cameraLock = false;
	}

	Vector3 CenterCamera(Vector3 one, Vector3 two){
		Vector3 mid = new Vector3();
		mid.x = (one.x + two.x )/2;
		mid.y = (one.y + two.y) / 2;

		// check maincamera bounds with levelBounds
		littleBox.transform.position = mid;

		var littleBottomLeft = littleBox.camera.ViewportToWorldPoint (new Vector2(0,0));
		var littleTopRight = littleBox.camera.ViewportToWorldPoint (new Vector2(1,1));

		var levelBottomLeft = levelBounds.camera.ViewportToWorldPoint(new Vector2(0,0));
		var levelTopRight = levelBounds.camera.ViewportToWorldPoint(new Vector2(1,1));

		if (!checkPointInBounds (levelBounds,littleBottomLeft)) {
			if(littleBottomLeft.x < levelBottomLeft.x){
				mid.x += Mathf.Abs(littleBottomLeft.x - levelBottomLeft.x);
			}

			if(littleBottomLeft.y < levelBottomLeft.y){
				mid.y += Mathf.Abs(littleBottomLeft.y - levelBottomLeft.y);
			}
		}

		if (!checkPointInBounds (levelBounds,littleTopRight)) {
			if(littleTopRight.x > levelTopRight.x){
				mid.x -= Mathf.Abs(littleTopRight.x - levelTopRight.x);
			}
			
			if(littleTopRight.y > levelTopRight.y){
				mid.y -= Mathf.Abs(littleTopRight.y - levelTopRight.y);
			}
		}

		littleBox.transform.position = mid;
		return mid;

	}

	public GameObject GetLevelBounds() {
		return levelBounds;
	}

}
