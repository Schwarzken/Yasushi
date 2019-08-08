using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KniveDrop : MonoBehaviour {
    public float attackInterval = 3.0f;
    float nextAttackTime;

    public GameObject knife1;
    public GameObject knife2;
    public GameObject knife3;
    public GameObject knife4;
	public GameObject kniv;
	[SerializeField] float knifeRespawnDelay;

	public Transform knifeHolder1;
	public Transform knifeHolder2;
	public Transform knifeHolder3;
	public Transform knifeHolder4;

    bool knife1drop = false;
    bool knife2drop = false;
    bool knife3drop = false;
    bool knife4drop = false;

    Transform playerTransform;
    Sounds sound;

    [SerializeField] Transform Zone1; 
    [SerializeField] Transform Zone2; 
    [SerializeField] Transform Zone3; 
    [SerializeField] Transform Zone4;

	int sceneNumber;
    // Use this for initialization
    void Start () {
		sceneNumber = SceneManager.GetActiveScene ().buildIndex;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        nextAttackTime = Time.time + attackInterval;
        sound = FindObjectOfType<Sounds>();
    }
	
	// Update is called once per frame
	void Update () {
		if (Time.time - nextAttackTime > 0) {
			Vector3 playerPos = playerTransform.position;
			if (sceneNumber != 4) {
				if (playerPos.x < Zone1.position.x) {
					if (knife1 != null) {
						knife1.GetComponent<Knife> ().KnifeShake (8f, 0.1f);
						StartCoroutine (KnifeDelay (knife1));
						if (knife1drop == false) {
							sound.PlayKnife ();
							knife1drop = true;
						}
					} else
						return;
				} else if (playerPos.x < Zone2.position.x) {
					if (knife2 != null) {
						knife2.GetComponent<Knife> ().KnifeShake (8f, 0.1f);
						StartCoroutine (KnifeDelay (knife2));
						if (knife2drop == false) {
							sound.PlayKnife ();
							knife2drop = true;
						}
					} else
						return;
				} else if (playerPos.x < Zone3.position.x) {
					if (knife3 != null) {
						knife3.GetComponent<Knife> ().KnifeShake (8f, 0.1f);
						StartCoroutine (KnifeDelay (knife3));
						if (knife3drop == false) {
							sound.PlayKnife ();
							knife3drop = true;
						}
					} else
						return;
				} else if (playerPos.x < Zone4.position.x) {
					if (knife4 != null) {
						knife4.GetComponent<Knife> ().KnifeShake (8f, 0.1f);
						StartCoroutine (KnifeDelay (knife4));
						if (knife4drop == false) {
							sound.PlayKnife ();
							knife4drop = true;
						}
					} else
						return;
				}
			} else if (sceneNumber == 4) {
				if (playerPos.x < Zone1.position.x) {
					if (knife1 != null) {
						knife1.GetComponent<Knife> ().KnifeShake (8f, 0.1f);
						StartCoroutine (KnifeDelay (knife1));
						if (knife1drop == false) {
							sound.PlayKnife ();
							knife1drop = true;
						}
					} else
						return;
				} else if (playerPos.x < Zone2.position.x) {
					if (knife2 != null) {
						knife2.GetComponent<Knife> ().KnifeShake (8f, 0.1f);
						StartCoroutine (KnifeDelay (knife2));
						if (knife2drop == false) {
							sound.PlayKnife ();
							knife2drop = true;
						}
					} else
						return;
				}
			}
			StartCoroutine (KnifeRespawn (kniv));
		}
	}

    IEnumerator KnifeDelay(GameObject knife)
    {
            yield return new WaitForSeconds(1f);
		if (knife != null) {
			knife.GetComponent<Knife> ().KnifeDrop ();
			yield return new WaitForSeconds (1f);
			Destroy (knife);
		}
    }

	IEnumerator KnifeRespawn(GameObject knife)
	{
		yield return new WaitForSeconds(knifeRespawnDelay);
		if (knife1 == null) {
			knife1 = Instantiate (knife, knifeHolder1.position, knifeHolder1.rotation);
		}
		if (knife2 == null) {
			knife2 = Instantiate (knife, knifeHolder2.position, knifeHolder2.rotation);
		}
		if (sceneNumber != 4) {
			if (knife3 == null) {
				knife3 = Instantiate (knife, knifeHolder3.position, knifeHolder3.rotation);
			}
			if (knife4 == null) {
				knife4 = Instantiate (knife, knifeHolder4.position, knifeHolder4.rotation);
			}
		}
	}
}
