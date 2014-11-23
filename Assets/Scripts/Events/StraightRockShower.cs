using UnityEngine;
using System.Collections;

public class StraightRockShower : MonoBehaviour, IEvent {

	public int numRocks = 5;
	public string path = "ThrowableObjects/StraightRock";
	// Use this for initialization
	public IEnumerator Start () {

		EventController.DisplayMessage ("Rock Shower! Throw rocks at each other!");
		int x = Random.Range (0, 2);
		if (x == 0) {
			this.GetComponent<TopLeftEntryMovement>().enabled = true;
		} else {
			this.GetComponent<TopRightEntryMovement>().enabled = true;
		}



		for(int i=0; i< numRocks; i++){
			yield return new WaitForSeconds(0.5f);
			CreateRock();
		}
		Destroy (this.gameObject);

	}

	void CreateRock(){
		var rock = Object.Instantiate (Resources.Load (path)) as GameObject;
		rock.transform.position = this.transform.position;
		Vector2 force = new Vector2(Random.Range(-800,800),Random.Range(-800,800));
		rock.rigidbody2D.AddForce(force);
	}

	public void Begin(){

	}

	public virtual void End(){
		EventController.EventEnd (RunnableEventType.StraightRockShower, RunnableEventType.Idle, 3);
	}

	public void OnDestroy(){
		End ();
	}
}
