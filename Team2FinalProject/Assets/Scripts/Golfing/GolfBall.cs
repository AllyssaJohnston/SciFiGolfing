using UnityEngine;

public class GolfBall : MonoBehaviour
{
    Rigidbody rb;

    public bool template = false;
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

    private void Start()
    {
        if (template)
        {
            r = GetComponent<Renderer>();
            r.material = ballMaterial;
            SetGlowEnabled(true);
            SetGlowIntensity(1);
            SetScrollSpeed(1);
            SetPulseSpeed(2);
        }
    }

    public void SetUp()
    {
        collisionChangeTimer = collisionChangeTimeLength;
        r = GetComponent<Renderer>();
        r.material = ballMaterial;
        rb = GetComponent<Rigidbody>();
        ObjectManager.backToSetUp.AddListener(Reset);
        resetCollisionTimer();
        SetGlowEnabled(glowEnabled);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (template)
        {
            return;
        }
        aliveTimer += Time.deltaTime;
        if (!collisionActive)
        {
            collisionChangeTimer -= Time.deltaTime;
        }
        if (collisionChangeTimer <= 0)
        {
            collisionActive = true;
        }
        if (!glowing())
        {
            SetGlowEnabled(false);
            firstGlow = false;
        }
        if (Mathf.Abs(transform.position.x) > 1000f || Mathf.Abs(transform.position.z) > 1000f || transform.position.y < -20f)
        {
            // safety, did fall out of world?
            RemoveSelf();
        }
    }

    public void resetCollisionTimer()
    {
        collisionActive = false;
        collisionChangeTimer = collisionChangeTimeLength;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (template)
        {
            return;
        }
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

    public void Reset() 
    {
        GolfBall.firstGlow = true;
        if (template)
        {
            return;
        }
        RemoveSelf(); 
    }

    public bool glowing() 
    {
        if (template)
        {
            return true;
        }
        return (firstGlow && (aliveTimer < (glowingLength + 1.5f))) || (!firstGlow && (aliveTimer < glowingLength));
    }

    public void RemoveSelf()
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
