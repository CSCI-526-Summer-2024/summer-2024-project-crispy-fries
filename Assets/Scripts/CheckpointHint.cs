using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class CheckpointHint : MonoBehaviour
{
    private CheckpointHintManager checkpointHintManager;
    [SerializeField] private string checkpointName;
    public Vector3 offset;
    private GameObject player;
    public string hintValue;
    public bool hasPassed;
    [SerializeField] private float delay = 3.0f;
    private Coroutine hintCoroutine;
    private TextUIManager textUIManager;
    
    void Start()
    {
        hasPassed = false;
        checkpointHintManager = FindObjectOfType<CheckpointHintManager>();
        checkpointHintManager.RegisterCheckpoint(this.gameObject);
        textUIManager = FindObjectOfType<TextUIManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {   
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPassed)
        {
            hasPassed = true;
            checkpointHintManager.PassCheckpoint(this.gameObject);
            hintCoroutine = StartCoroutine(ShowHintWithDelay());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintCoroutine != null)
            {
                StopCoroutine(hintCoroutine);
                hintCoroutine = null;
            }
            textUIManager.DeTriggerHint();
        }
    }

    private IEnumerator ShowHintWithDelay()
    {
        yield return new WaitForSeconds(delay);
        textUIManager.TriggerHint(this);
    }
}
