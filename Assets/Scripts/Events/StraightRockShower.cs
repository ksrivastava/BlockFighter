using UnityEngine;
using System.Collections;

public class StraightRockShower : MonoBehaviour, IEvent {

	public int numRocks = 5;
	public string path = "ThrowableObjects/StraightRock";
	// Use this for initialization
	public IEnumerator Start () {
		var pos = new Vector3 (-0.33f, 14.17f, 0.019f);
		this.transform.position = pos;
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
		EventController.EventEnd (EventType.StraightRockShower, EventType.Blimp, 3);
	}

	public void OnDestroy(){
		End ();
	}
}
