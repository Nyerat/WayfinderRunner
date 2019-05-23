using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float hitPoints;
    
    public float moveSpeed;
    public float speedMultiplier;

    public float speedIncreaseMilestone;
    private float speedMilestoneCount;

    public float jumpForce;

    public float jumpTime;
    private float jumpTimeCounter;

    private bool invuln = false;

    private Rigidbody2D rb2d;

    private bool knockback;

    public bool grounded;
    public LayerMask ground;

    private Collider2D myCollider;

    public float knockbackCount;
    public float counter;

    private float scorenumber = 0;
    public Text score;
    public Text health;
    

    // Start is called before the first frame update
    void Start()
    {
        knockback = false;
        

        rb2d = GetComponent<Rigidbody2D>();

        myCollider = GetComponent<Collider2D>();

        jumpTimeCounter = jumpTime;

        speedMilestoneCount = speedIncreaseMilestone;
    }

    // Update is called once per frame
    void Update()
    {
        health.text = hitPoints.ToString();
        score.text = scorenumber.ToString();

        grounded = Physics2D.IsTouchingLayers(myCollider, ground);

        if (transform.position.x > speedMilestoneCount)
        {
            speedMilestoneCount += speedIncreaseMilestone;


            speedIncreaseMilestone += speedIncreaseMilestone * speedMultiplier;
            moveSpeed = moveSpeed * speedMultiplier;
        }

        if (knockback)
        {
            if (counter <= 0)
            {
                invuln = true;
                counter = knockbackCount;
                rb2d.AddForce(new Vector3(-600f, 300f, transform.position.z));
                moveSpeed -= 1;
            }
            else
            {
                counter -= Time.deltaTime;
                if (counter <= 0)
                {
                    invuln = false;
                    knockback = false;
                }
            }
        }
        else
        {
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
            if (hit.collider != null)
            {
                if (grounded)
                {
                    //Touch touch = Input.GetTouch(0);
                    //Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);




                    if (hit.collider.name == "JumpTouchZone")
                    {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                    }

                }




                if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
                {
                    if (jumpTimeCounter > 0)
                    {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                        jumpTimeCounter -= Time.deltaTime;
                    }
                }

            }
            if(Input.GetKeyUp (KeyCode.Space) || Input.GetMouseButtonUp(0))
            {
                jumpTimeCounter = 0;
            }

            if(grounded)
            {
                jumpTimeCounter = jumpTime;
            }
        }








    }

    //Player Touches Enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Enemy") && invuln == false)
        {
            var enemyCollided = collision.GetComponent<GameObject>();
            var enemyCollidedstat = collision.GetComponent<EnemyStats>();
            var damage = enemyCollidedstat.enemyDamageDealt;

            hitPoints = hitPoints - damage;

            if(hitPoints <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            enemyCollidedstat.enemyHitPoints -= 5;
            if (enemyCollidedstat.enemyHitPoints <= 0)
            {
                enemyCollidedstat.DestroyMe();
                
            }


            knockback = true;
            





            Debug.Log(damage);

            Debug.Log(hitPoints);
        }
    }

    public void scoreAddFromEnemyDying()
    {
        scorenumber++;
    }
}
