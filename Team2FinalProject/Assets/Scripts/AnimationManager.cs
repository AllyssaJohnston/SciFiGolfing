using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static AnimationManager instance;
    public Transform rightShoulder;
    public Transform leftShoulder;
    public Transform leftElbow;
    public Transform golfClub;
    public float range = 40f;
    public float force = 10f;
    public float speed = 5f;

    private static bool playing = false;
    private static bool forward = false;
    private static float maxAngle = 0f;
    private static float curAngle = 0f;

    private static float startingRange = 40f;
    private static float startingForce = 10f;
    private static float startingSpeed = 5f;



    void Awake()
    {
        if (instance != null && instance != this)
        {
            //can't have multiples of this class
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        startingRange = range;
        startingForce = force;
        startingSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if (forward)
            {
                curAngle += speed * Time.deltaTime;
                rightShoulder.localRotation *= Quaternion.AngleAxis(-1.0f * speed * Time.deltaTime, Vector3.forward);
                leftShoulder.localRotation *= Quaternion.AngleAxis(-2.0f * speed * Time.deltaTime, Vector3.forward);
                leftElbow.localRotation *= Quaternion.AngleAxis(-1.75f * speed * Time.deltaTime, Vector3.forward);
                if (curAngle > maxAngle) 
                { 
                    forward = false;  
                }
            }
            else
            {
                curAngle -= speed * Time.deltaTime;
                rightShoulder.localRotation *= Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.forward);
                leftShoulder.localRotation *= Quaternion.AngleAxis(2.0f * speed * Time.deltaTime, Vector3.forward);
                leftElbow.localRotation *= Quaternion.AngleAxis(1.75f * speed * Time.deltaTime, Vector3.forward);
                if (curAngle < 0)
                {
                    playing = false;
                }
            }  
        }
    }

    public static void PlayAnimation()
    {
        Debug.Log(instance.range);
        float radius = instance.golfClub.localScale.y * 2;
        maxAngle = Mathf.PI * 2 * instance.range / radius;
        Debug.Log(maxAngle);
        playing = true;
        forward = true;

    }

    public static void Reset()
    {
        instance.range = startingRange;
        instance.force = startingForce;
        instance.speed = startingSpeed;
        SliderManager.ResetAttributeSliders();
    }

    public static float getRange() { return instance.range; }

    public static void setRange(float givenRange) { instance.range = givenRange; }

    public static float getForce() { return instance.force; }

    public static void setForce(float givenForce) { instance.force = givenForce; }

    public static float getSpeed() { return instance.speed; }

    public static void setSpeed(float givenSpeed) { instance.speed = givenSpeed; }
}
