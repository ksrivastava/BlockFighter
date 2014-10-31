using UnityEngine;
using System.Collections;

public class MessageLife : MonoBehaviour {

	bool killCalled = false;

	public void Kill (float seconds) {
		if (killCalled)
						return;
		Invoke ("KillHelper", seconds);
	}

	public void KillHelper(){
		Destroy (this.gameObject);
	}
}
