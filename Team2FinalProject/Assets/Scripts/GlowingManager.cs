using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlowingManager : MonoBehaviour
{
    private static GlowingManager instance;
    public Toggle glowToggle;
    public Slider glowIntensitySlider;
    public Slider scrollSpeedSlider;
    public Slider pulseSpeedSlider;
    public TMP_Text intensityLabel;
    public TMP_Text scrollLabel;
    public TMP_Text pulseLabel;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    void Start()
    {
        SetupSliders();
        intensityLabel.text = "Intensity: " + glowIntensitySlider.value.ToString("F2");
        scrollLabel.text = "Scroll speed: " + scrollSpeedSlider.value.ToString("F2");
        pulseLabel.text = "Pulse speed: " + pulseSpeedSlider.value.ToString("F2");
        glowToggle.onValueChanged.AddListener(OnGlowToggleChanged);
        glowIntensitySlider.onValueChanged.AddListener(OnGlowIntensityChanged);
        scrollSpeedSlider.onValueChanged.AddListener(OnScrollSpeedChanged);
        pulseSpeedSlider.onValueChanged.AddListener(OnPulseSpeedChanged);
    }

    private void SetupSliders()
    {
        glowIntensitySlider.minValue = 0f;
        glowIntensitySlider.maxValue = 5f;

        scrollSpeedSlider.minValue = -5f;
        scrollSpeedSlider.maxValue = 5f;

        pulseSpeedSlider.minValue = 0f;
        pulseSpeedSlider.maxValue = 10f;

        glowToggle.isOn = false;                
        glowIntensitySlider.value = 1f;        
        scrollSpeedSlider.value = 0.5f;           
        pulseSpeedSlider.value = 2f;            
    }

    public static void setUpGlow(GolfBall ball)
    {
        // Apply all UI settings immediately to new balls
        ball.SetGlowEnabled(instance.glowToggle.isOn);
        ball.SetGlowIntensity(instance.glowIntensitySlider.value);
        ball.SetScrollSpeed(instance.scrollSpeedSlider.value);
        ball.SetPulseSpeed(instance.pulseSpeedSlider.value);
    }

    // UI callbacks
    void OnGlowToggleChanged(bool enabled)
    {
        foreach (GolfBall ball in GolfBallManager.getGolfBalls())
        {
            if (ball == null || ball.enabled == false) { continue; }
            ball.SetGlowEnabled(enabled);
        }
    }

    void OnGlowIntensityChanged(float value)
    {
        intensityLabel.text = "Intensity: " + value.ToString("F2");
        foreach (GolfBall ball in GolfBallManager.getGolfBalls())
        {
            if (ball == null || ball.enabled == false) { continue; }
            ball.SetGlowIntensity(value);
        }
    }

    void OnScrollSpeedChanged(float value)
    {
        scrollLabel.text = "Scroll speed: " + value.ToString("F2");
        foreach (GolfBall ball in GolfBallManager.getGolfBalls())
        {
            if (ball == null || ball.enabled == false) { continue; }
            ball.SetScrollSpeed(value);
        }
    }

    void OnPulseSpeedChanged(float value)
    {
        pulseLabel.text = "Pulse speed: " + value.ToString("F2");
        foreach (GolfBall ball in GolfBallManager.getGolfBalls())
        {
            if (ball == null || ball.enabled == false) { continue; }
            ball.SetPulseSpeed(value);
        }
    }
}
