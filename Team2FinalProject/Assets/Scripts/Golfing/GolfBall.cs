using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GolfBall : MonoBehaviour
{
    Rigidbody rb;

    public bool collisionActive = false;
    public float collisionChangeTimeLength = 0.2f;
    private float collisionChangeTimer = .1f;
    private float aliveTimer = 0f;
    private float glowingLength = 10f;
    public Texture glowingTexture;
    public Texture plainTexture;
    public Material ballMaterial;
    public float glowIntensity = 1.0f;
    public float scrollSpeed = 1f;
    public float pulseSpeed = 2f;
    public bool glowEnabled = false;


    public void SetUp()
    {
        EnableGlow(glowEnabled);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collisionChangeTimer = collisionChangeTimeLength;
        rb = GetComponent<Rigidbody>();
        ObjectManager.resetWorld.AddListener(Reset);
        resetCollisionTimer();

        EnableGlow(glowEnabled);
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
        // if (globalGlowEnabled && aliveTimer > glowingLength)
        // {
        //     EnableGlow(false);
        // }
    }

    public void EnableGlow(bool enabled)
    {

        ballMaterial.SetFloat("_GlowEnabled", enabled ? 1f : 0f);

        if(enabled)
        {
            ApplyGlowTexture();
        }
        else
        {
            ApplyPlainTexture();
        }
    }

    private void ApplyGlowTexture()
    {
        ballMaterial.SetTexture("_MainTex", glowingTexture);
        ballMaterial.SetTexture("GlowTex", glowingTexture);

        ballMaterial.SetFloat("_GlowIntensity", glowIntensity);
        ballMaterial.SetFloat("_ScrollSpeed", scrollSpeed);
        ballMaterial.SetFloat("_PulseSpeed", pulseSpeed);
        GetComponent<Renderer>().material = ballMaterial;
    }
    private void ApplyPlainTexture()
    {
        ballMaterial.SetTexture("_MainTex", glowingTexture);
        ballMaterial.SetFloat("GlowIntensity", 0f);
        gameObject.GetComponent<Renderer>().material = ballMaterial;
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
        var r = GetComponent<Renderer>();

        r.material.SetFloat("_GlowEnabled", enable ? 1f : 0f);
    }

    public void SetGlowIntensity(float value)
    {
        glowIntensity = value;
        var r = GetComponent<Renderer>();

        r.material.SetFloat("_GlowIntensity", value);
    }

    public void SetScrollSpeed(float value)
    {
        scrollSpeed = value;
        var r = GetComponent<Renderer>();

        r.material.SetFloat("_ScrollSpeed", value);
    }

    public void SetPulseSpeed(float value)
    {
        pulseSpeed = value;
        var r = GetComponent<Renderer>();

        r.material.SetFloat("_PulseSpeed", value);
    }
}
