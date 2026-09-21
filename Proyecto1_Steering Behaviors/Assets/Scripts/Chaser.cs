using UnityEngine;

public class Chaser : MonoBehaviour
{
    public float wanderSpeed = 5f;     
    public float turnInterval = 4f;    
    public float minTurnAngle = 45f;
    public float maxTurnAngle = 100f;
    public float detectRadius = 5f;   
    public float loseRadius = 8f;    
    public float arriveRadius = 2f;  
    public float stopRadius = 1f;     

    Rigidbody2D rb;
    Transform player;
    Vector2 dir;
    float nextTurn;
    bool chasing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        dir = Random.insideUnitCircle.normalized;
    }

    void FixedUpdate()
    {
        Vector2 toPlayer = (Vector2)player.position - rb.position;
        float dist = toPlayer.magnitude;

        if (dist < detectRadius) chasing = true;
        else if (dist > loseRadius) chasing = false;

        if (chasing)   
            rb.linearVelocity = toPlayer.normalized * wanderSpeed * Mathf.Clamp01((dist - stopRadius) / (arriveRadius - stopRadius));
        else           
            Wander();
    }

    void Wander()
    {
        if (Time.time >= nextTurn)
        {
            float giro = Random.Range(minTurnAngle, maxTurnAngle) * (Random.value < 0.5f ? -1 : 1);
            dir = Quaternion.Euler(0, 0, giro) * dir;
            nextTurn = Time.time + turnInterval;
        }
        rb.linearVelocity = dir * wanderSpeed;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        dir = (rb.position - col.GetContact(0).point).normalized;   
        nextTurn = Time.time + turnInterval;
    }
}
