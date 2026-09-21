using UnityEngine;

public class Wanderer : MonoBehaviour
{
    public float wanderSpeed = 5f;
    public float turnInterval = 5f;    
    public float minTurnAngle = 45f;  
    public float maxTurnAngle = 100f;  

    Rigidbody2D rb;
    Vector2 dir;                       
    float nextTurn;                    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = Random.insideUnitCircle.normalized;   
    }

    void FixedUpdate()
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
