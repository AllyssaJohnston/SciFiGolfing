using UnityEngine;

public class GolfBall : MonoBehaviour
{
    Rigidbody rb;

    public bool collisionActive = false;
    public float collisionChangeTimeLength = 0.2f;
    private float collisionChangeTimer = .1f;
    private float aliveTimer = 0f;
    private float glowingLength = 4.5f;
    public Material ballMaterial;
    private Renderer r;
    public float glowIntensity = 1.0f;
    public float scrollSpeed = 1f;
    public float pulseSpeed = 2f;
    public bool glowEnabled = false;
    private static bool firstGlow = true;

    public void SetUp()
    {
        collisionChangeTimer = collisionChangeTimeLength;
        rb = GetComponent<Rigidbody>();
        r = GetComponent<Renderer>();
        r.material = ballMaterial;
        ObjectManager.resetWorld.AddListener(Reset);
        resetCollisionTimer();
        SetGlowEnabled(glowEnabled);

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
        if ((firstGlow && (aliveTimer > (glowingLength + 3f))) || (!firstGlow && (aliveTimer > glowingLength)))
        {
            SetGlowEnabled(false);
            firstGlow = false;
        }

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

    public bool glowing() {  return aliveTimer < glowingLength; }

    private void RemoveSelf()
    {
        ScoreTracker.decreaseBalls();
        Destroy(gameObject);
    }

    public void SetGlowEnabled(bool enable)
    {
        glowEnabled = enable;
        r.material.SetFloat("_GlowEnabled", enable ? 1f : 0f);
        if (enable)
        {
            ballMaterial.SetFloat("_GlowIntensity", glowIntensity);
            ballMaterial.SetFloat("_ScrollSpeed", scrollSpeed);
            ballMaterial.SetFloat("_PulseSpeed", pulseSpeed);
        }
        else
        {
            ballMaterial.SetFloat("GlowIntensity", 0f);
        }
    }

    public void SetGlowIntensity(float value)
    {
        glowIntensity = value;
        r.material.SetFloat("_GlowIntensity", value);
    }

    public void SetScrollSpeed(float value)
    {
        scrollSpeed = value;
        r.material.SetFloat("_ScrollSpeed", value);
    }

    public void SetPulseSpeed(float value)
    {
        pulseSpeed = value;
        r.material.SetFloat("_PulseSpeed", value);
    }
}
