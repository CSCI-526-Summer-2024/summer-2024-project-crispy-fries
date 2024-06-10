using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpotLightController : MonoBehaviour
{
    public Light2D baseLight;
    public float radius;
    public float angle;

    private bool isLightOn = true;
    [SerializeField]
    private int disabledTime;

    [SerializeField]
    private GameObject leftFlapPivot;
    [SerializeField]
    private GameObject rightFlapPivot;

    [SerializeField]
    private SpriteRenderer bulbRenderer;

    [SerializeField]
    private TextMeshPro countdownText;
    private Coroutine countdownCoroutine;
    


    public float Radius
    {
        get { return radius; }
        set
        {
            radius = value;
            UpdateSpotLightProperties();
        }
    }

    // Property for angle
    public float Angle
    {
        get { return angle; }
        set
        {
            angle = Mathf.Max(0, value);
            UpdateSpotLightProperties();
        }
    }

    public bool IsLightOn
    {
        get { return isLightOn; }
        set
        {
            isLightOn = value;
            baseLight.enabled = isLightOn;
            UpdateBulbColor();
        }
    }

    // Only to be used by player script as this manages timer related co-routines. Other places use IsLightOn
    public void toggleLightOff()
    {

        IsLightOn = false;
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }
    public void toggleLightOn()
    {

        IsLightOn = true;
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownText.text="";
    }
    public void toggleLight()
    {
        if(IsLightOn)
        {
            toggleLightOff();

        }
        else
        {
            toggleLightOn();
        }
    }

    public bool DoesIlluminate(Vector2 point, LayerMask obstacleLayer)
    {
        // If light is off, return false
        if(!IsLightOn)
        { 
            return false;
        }
        Vector2 spotlightPosition = transform.position;
        Vector2 direction = point - spotlightPosition;

        // Check if the point is within the spotlight's radius
        if (direction.magnitude > radius)
        {
            return false;
        }


        // Calculate the angle between the spotlight's forward direction and the vector to the point
        float angleToPoint = Vector2.Angle(transform.up, direction);

        // Check if the angle to the point is within the spotlight's angle
        if (angleToPoint > angle / 2f)
        {
            return false;
        }
        // Cast a ray from the spotlight towards the point with the spotlight's radius as the maximum distance
        RaycastHit2D hit = Physics2D.Raycast(spotlightPosition, direction, radius, obstacleLayer);
        if (hit.collider != null && hit.distance < direction.magnitude)
        {
            Debug.Log("Obstructed");
            return false; // Point is obstructed, not illuminated
        }
        return true; 

    }

    void Awake()
    {
        UpdateSpotLightProperties();
    }

    void OnValidate()
    {
        angle = Mathf.Max(0, angle);
        UpdateSpotLightProperties();
    }

    void UpdateSpotLightProperties()
    {

        baseLight.pointLightOuterRadius = radius;
        baseLight.pointLightInnerRadius = radius;
        baseLight.pointLightOuterAngle = angle;
        baseLight.pointLightInnerAngle = angle;

        PositionFlaps();

        // Force the editor to update
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(baseLight);
        UnityEditor.SceneView.RepaintAll();
        #endif
    }

    void PositionFlaps()
    {
        if (leftFlapPivot != null && rightFlapPivot != null)
        {
            float halfAngle = angle / 2;

            leftFlapPivot.transform.localRotation = Quaternion.Euler(0, 0, -halfAngle);
            rightFlapPivot.transform.localRotation = Quaternion.Euler(0, 0, halfAngle);
        }
    }

    private void UpdateBulbColor()
    {
        bulbRenderer.color = isLightOn ? Color.white : Color.grey;
    }

    private IEnumerator CountdownCoroutine()
    {
        int timer = Mathf.RoundToInt(disabledTime); // Round the duration to the nearest integer

        while (timer > 0)
        {
            // Update countdown text
            countdownText.text = timer.ToString();

            // Wait for a second
            yield return new WaitForSeconds(1f);

            // Decrease timer
            timer--;
        }

        IsLightOn = true;
        countdownText.text = "";
        countdownCoroutine = null;
    }

    void Start()
    {
        baseLight.pointLightInnerRadius = radius;
        baseLight.pointLightOuterRadius = radius;
        UpdateBulbColor();
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #if UNITY_EDITOR
    // void OnDrawGizmos()
    // {
    //     // Ensure properties are applied during scene view interactions
    //     UpdateSpotLightProperties();
    // }
    #endif
}
