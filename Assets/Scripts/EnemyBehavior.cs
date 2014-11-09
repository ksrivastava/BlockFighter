using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public float forceMultiplier = 30.0f;
	public float attackDelay = 3.0f;

	private float health = 20;
	public float Health {
		get {
			print (transform.name + ": " + health.ToString());
			return health;
		}
		
		set {
			health = value;
			
			if (health == 0) {
				Destroy(gameObject);
			}
		}
	}

	public void ReduceHealth(int n) {
		Health -= n;
	}

	void Start () {
		StartCoroutine (Shoot ());
	}

	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Hammer") {
			var hammerController = col.gameObject.transform.parent.GetComponent<HammerControl>();
			if (hammerController.isHitting && !hammerController.attackComplete) {
				ReduceHealth(5);
				col.gameObject.collider2D.enabled = false;
			}
		}
	}

	IEnumerator Shoot () {
		while (true) {
			GameObject obj = Instantiate (Resources.Load ("Events/EnemyAttack")) as GameObject;
			obj.transform.position = transform.position;
			EnemyAttack atk = obj.GetComponent<EnemyAttack>();

			GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
			int target = Random.Range (0, targets.Length);

			Vector3 dir = targets[target].transform.position - gameObject.transform.position;
			dir /= dir.magnitude;
			dir.y += 0.25f;

			atk.Launch(new Vector2(forceMultiplier * dir.x, forceMultiplier * dir.y));

			yield return new WaitForSeconds(attackDelay);
		}
	}
}
