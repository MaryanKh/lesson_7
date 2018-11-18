using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    public float speed = 0.1f;
    public Transform leftB;
    public Transform rightB;
    public Transform bottomB;
    public Transform topB;
    public GameObject foodPrefab;
    public GameObject tailPrefab;
    public Text countText;
    public int score = 0;
    public UnityEvent OnEat;

    private Vector2 vec = Vector2.up;
    private Vector2 moveVec;
    private List<Transform> tail = new List<Transform>();
    private bool eat = false;
    private bool moveHorizontal = true;
    private bool moveVertical = false;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Movement", 0.3f, speed);
        Count();
        SpawnApple();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow) && moveHorizontal)
        {
            moveHorizontal = false;
            moveVertical = true;
            vec = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && moveVertical)
        {
            moveVertical = false;
            moveHorizontal = true;
            vec = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && moveVertical)
        {
            moveHorizontal = true;
            moveVertical = false;
            vec = -Vector2.up;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && moveHorizontal)
        {
            moveHorizontal = false;
            moveVertical = true;
            vec = -Vector2.right;
        }
        moveVec = vec / 3f;
    }

    void Movement()
    {
        Vector2 ta = transform.position;
        if (eat)
        {
            GameObject g = (GameObject)Instantiate(tailPrefab, ta, Quaternion.identity);
            tail.Insert(0, g.transform);
            eat = false;
        }
        else if (tail.Count > 0) {
            tail.Last().position = ta;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }

        transform.Translate(moveVec);
    }

    public void SpawnApple()
    {
        int x = (int)Random.Range(leftB.position.x, rightB.position.x);
        int y = (int)Random.Range(bottomB.position.y, topB.position.y);  

        foreach(var tails in tail)
        {
            if(tails.position.x == x || tails.position.y == y)
            {
                SpawnApple();
            }
        }

        Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity);
    }

    void Count()
    {
        countText.text = score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "food")
        {
            eat = true;
            Destroy(other.gameObject);
            score++;
            Count();
            SpawnApple();
        }
        else
        {
            SceneManager.LoadScene(0);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(OnEat != null)
        {
            OnEat.Invoke();
        }
    }
}
