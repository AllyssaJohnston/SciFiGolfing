using UnityEngine;

public class GolfBall : MonoBehaviour
{
    Rigidbody rb;

    public bool collisionActive = false;
    public float collisionChangeTimeLength = 0.2f;
    private float collisionChangeTimer = .1f;
    private float aliveTimer = 0f;
    private float glowingLength = 3f;
    public Material glowingTexture;
    public Material plainTexture;

    public void SetUp()
    {
        gameObject.GetComponent<Renderer>().material = glowingTexture;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collisionChangeTimer = collisionChangeTimeLength;
        rb = GetComponent<Rigidbody>();
        ObjectManager.resetWorld.AddListener(Reset);
        resetCollisionTimer();
    }

    // Update is called once per frame
    void Update()
    {
        aliveTimer += Time.deltaTime;
        if (!collisionActive)
        {
            collisionChangeTimer -= Time.deltaTime;
        }
        if (collisionChangeTimer <= 0){
            collisionActive = true;
        }
        if (aliveTimer > glowingLength)
        {
            switchToPlain();
        }
    }

    private void switchToPlain()
    {
        gameObject.GetComponent<Renderer>().material = plainTexture;
    }

    public void resetCollisionTimer()
    {
        collisionActive = false;
        collisionChangeTimer = collisionChangeTimeLength;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null || collision.collider == null || collision.gameObject.transform.parent == null) { return; }
        if (collision.gameObject.layer == LayerMask.NameToLayer("hole"))
        {
            // hit a hole
            //Debug.Log("hit a hole");
            ScoreTracker.IncreaseScore();
            RemoveSelf();
        }
        if (collisionActive) {
            AnimationManager.StopAnimation();
            Debug.Log(collision.gameObject.transform.parent.gameObject.name);
            Vector3 dir = (transform.position - collision.gameObject.transform.position).normalized;
            rb.AddForce(dir * AnimationManager.getForce(), ForceMode.Impulse);
        }
    }

    public void Reset() { RemoveSelf(); }

    private void RemoveSelf()
    {
        ScoreTracker.decreaseBalls();
        Destroy(gameObject);
    }
}
