using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;

public class GlowingManager : MonoBehaviour
{
    public Toggle glowToggle;
    public Slider glowIntensitySlider;
    public Slider scrollSpeedSlider;
    public Slider pulseSpeedSlider;
    public TMP_Text intensityLabel;
    public TMP_Text scrollLabel;
    public TMP_Text pulseLabel;

    private List<GolfBall> activeBalls = new List<GolfBall>();

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

        // Register all existing balls in the scene
        FindExistingBalls();
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

    public void RegisterBall(GolfBall ball)
    {
        activeBalls.Add(ball);

        // Apply all UI settings immediately to new balls
        ball.SetGlowEnabled(glowToggle.isOn);
        ball.SetGlowIntensity(glowIntensitySlider.value);
        ball.SetScrollSpeed(scrollSpeedSlider.value);
        ball.SetPulseSpeed(pulseSpeedSlider.value);
    }

    private void FindExistingBalls()
    {
        activeBalls.Clear();
        activeBalls.AddRange(FindObjectsByType<GolfBall>(FindObjectsSortMode.None));

        foreach (var ball in activeBalls)
        {
            ball.SetGlowEnabled(glowToggle.isOn);
            ball.SetGlowIntensity(glowIntensitySlider.value);
            ball.SetScrollSpeed(scrollSpeedSlider.value);
            ball.SetPulseSpeed(pulseSpeedSlider.value);
        }
    }

    // UI callbacks
    void OnGlowToggleChanged(bool enabled)
    {
        CleanupDeadBalls();
        foreach (var b in activeBalls)
            b.SetGlowEnabled(enabled);
    }

    void OnGlowIntensityChanged(float value)
    {
        CleanupDeadBalls();
        intensityLabel.text = "Intensity: " + value.ToString("F2");
        foreach (var b in activeBalls)
            b.SetGlowIntensity(value);
    }

    void OnScrollSpeedChanged(float value)
    {
        CleanupDeadBalls();
        scrollLabel.text = "Scroll speed: " + value.ToString("F2");
        foreach (var b in activeBalls)
            b.SetScrollSpeed(value);
    }

    void OnPulseSpeedChanged(float value)
    {
        CleanupDeadBalls();
        pulseLabel.text = "Pulse speed: " + value.ToString("F2");
        foreach (var b in activeBalls)
            b.SetPulseSpeed(value);
    }

    private void CleanupDeadBalls()
    {
        for (int i = activeBalls.Count - 1; i >= 0; i--)
        {
            if (activeBalls[i] == null)
                activeBalls.RemoveAt(i);
        }
    }
}
