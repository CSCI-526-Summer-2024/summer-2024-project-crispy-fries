using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpotLightController : MonoBehaviour
{
    public Light2D baseLight;
    public float radius;
    public float angle;
    public string lightColorHex = "#FFFFFF";

    private bool isLightOn = true;
    [SerializeField]
    private float disabledTime;

    [SerializeField] private float maxTimerInScene = 5;

    public bool isToggleable;

    [SerializeField]
    private GameObject leftFlapPivot;
    [SerializeField]
    private GameObject rightFlapPivot;
    [SerializeField]
    private GameObject silhouette;

    [SerializeField]
    private SpriteRenderer bulbRenderer;

    [SerializeField]
    private GameObject timerProgress;
    private Coroutine countdownCoroutine;


    
    public string LightColorHex
    {
        get { return lightColorHex; }
        set
        {
            lightColorHex = value;
            UpdateSpotLightProperties();
        }
    }
    public Color GetLightColor()
    {
        return baseLight.color;
    }

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
        if(!isToggleable) return;
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }
    public void toggleLightOn()
    {
        if(!isToggleable) return;
        IsLightOn = true;
        timerProgress.transform.Find("TimerBarMaskPivot").localEulerAngles = new Vector3(0, 0, 0);;
        timerProgress.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        silhouette.SetActive(false);
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
    }
    public void toggleLight()
    {
        if(!isToggleable) return;
        if(IsLightOn)
        {
            toggleLightOff();

        }
        else
        {
            toggleLightOn();
        }
    }

    public bool DoesIlluminate(Vector2 point)
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
        RaycastHit2D[] hits = Physics2D.RaycastAll(spotlightPosition, direction, radius);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.distance < direction.magnitude)
            {
                // Check if the hit object has the "BlocksLight" tag
                if (hit.collider.CompareTag("BlocksLight"))
                {
                    Debug.Log("Obstructed by " + hit.collider.name);
                    return false; // Point is obstructed, not illuminated
                }
            }
        }
        return true; 

    }


        public bool IfInTheShadow(Vector2 point)
    {
        // If light is off, return false
        Vector2 spotlightPosition = transform.position;
        Vector2 direction = point - spotlightPosition;

        // Check if the point is within the spotlight's radius
        if (isLightOn) {
            return false;
        }
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
        RaycastHit2D[] hits = Physics2D.RaycastAll(spotlightPosition, direction, radius);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.distance < direction.magnitude)
            {
                // Check if the hit object has the "BlocksLight" tag
                if (hit.collider.CompareTag("BlocksLight"))
                {
                    Debug.Log("Obstructed by " + hit.collider.name);
                    return false; // Point is obstructed, not illuminated
                }
            }
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

        Color color;
        if (ColorUtility.TryParseHtmlString(lightColorHex, out color))
        {
            baseLight.color = color;
        }
        else
        {
            baseLight.color = Color.white;
        }

        if (!isToggleable)
        {
            timerProgress.SetActive(false);
            SetFlapColors(new Color(0.55f, 0.27f, 0.07f));
        }
        else
        {
            timerProgress.SetActive(true);
            float initialMaskRotation = (1-disabledTime/maxTimerInScene)*180;
            Transform progressMaskPivot = timerProgress.transform.Find("TimerBarMaskPivot");
            Transform sizeMaskPivot = timerProgress.transform.Find("TimerBar/SizeMaskPivot");
            progressMaskPivot.transform.localEulerAngles = new Vector3(0, 0, initialMaskRotation);
            sizeMaskPivot.transform.localEulerAngles = new Vector3(0, 0, initialMaskRotation);

        }


        PositionFlaps();
        UpdateBulbColor();
        buildSilhouette();

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
    private void SetFlapColors(Color color)
    {
        if (leftFlapPivot != null && rightFlapPivot != null)
        {
            leftFlapPivot.transform.Find("LeftFlap").GetComponent<SpriteRenderer>().color = color;
            rightFlapPivot.transform.Find("RightFlap").GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void UpdateBulbColor()
    {
        bulbRenderer.color = isLightOn ? new Color(baseLight.color.r,baseLight.color.g,baseLight.color.b,0.5f) : Color.grey;
    }

    private void buildSilhouette()
    {
        LineRenderer lr = silhouette.GetComponent<LineRenderer>();

        lr.positionCount=16;
        var points = new Vector3[lr.positionCount];
        //set first and last point to center of light
        points[0] = transform.position;
        points[lr.positionCount-1] = transform.position;

        for(int i=1;i<lr.positionCount-1;i++)
        {

            float deltaAngle = (-transform.eulerAngles.z+Mathf.Lerp(-angle/2,angle/2, (float)(i-1)/(lr.positionCount-3))) * Mathf.Deg2Rad;
            Vector3 delta = new Vector3(Mathf.Sin(deltaAngle), Mathf.Cos(deltaAngle),0);
            points[i] = transform.position + radius*delta;
        }
        lr.SetPositions(points);
    }

    private IEnumerator CountdownCoroutine()
    {
        IsLightOn = false;
        float timer = disabledTime; // Set the timer to the full duration
        Transform spriteMaskPivot = timerProgress.transform.Find("TimerBarMaskPivot");
        float startRotation = (1-disabledTime/maxTimerInScene)*180;
        float endRotation = 180f;
        
        SpriteRenderer timerBarSprite = timerProgress.transform.Find("TimerBar/BaseCircle").GetComponent<SpriteRenderer>();
        timerBarSprite.color = Color.yellow;
        silhouette.SetActive(true);

        while (timer > 0)
        {

            // Calculate and set the current rotation of the mask
            float t = (disabledTime - timer) / disabledTime;
            float currentRotation = Mathf.Lerp(startRotation, endRotation, t);
            spriteMaskPivot.transform.localEulerAngles = new Vector3(0, 0, currentRotation);
            silhouette.GetComponent<LineRenderer>().material.SetTextureOffset("_MainTex", Vector2.right * timer * 0.5f);

            // Wait for the next frame
            yield return null;

            // Decrease timer by the time elapsed since the last frame
            timer -= Time.deltaTime;
        }

        // Ensure the mask is fully rotated at the end
        spriteMaskPivot.transform.localEulerAngles = new Vector3(0, 0, (1-disabledTime/maxTimerInScene)*180);
        
        timerBarSprite.color = Color.green;
        IsLightOn = true;
        silhouette.SetActive(false);
        countdownCoroutine = null;
    }

    void Start()
    {
        baseLight.pointLightInnerRadius = radius;
        baseLight.pointLightOuterRadius = radius;
        UpdateBulbColor();
        silhouette.SetActive(false);
        silhouette.GetComponent<LineRenderer>().material.SetColor("_Color", bulbRenderer.color); // set color of dashed line
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
