using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed = 0.1f;
    public Transform leftB;
    public Transform rightB;
    public Transform bottomB;
    public Transform topB;
    public GameObject food;
    public Text countText;
    public int score = 0;

    private Vector2 vec = Vector2.up;
    private Vector2 moveVec;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Movement", 0.3f, speed);
        Count();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            vec = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            vec = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            vec = -Vector2.up;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            vec = -Vector2.right;
        }
        moveVec = vec / 3f;
    }

    void Movement()
    {
        transform.Translate(moveVec);
    }

    void Count()
    {
        countText.text = score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "food")
        {
            Destroy(other.gameObject);
            score++;
            Count();
        }

        if(other.gameObject.tag == "border")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
