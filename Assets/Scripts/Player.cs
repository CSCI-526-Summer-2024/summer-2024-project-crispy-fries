using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public GameObject player;
    public GameObject _tileMap;
    public ShadowCaster2DCreator shadowCaster;
    public Tilemap tileMap;
    public SpotLightManager spotLightManager;
    // Hardcoded size of Tilemap grid
    Vector3Int[] positions;
    public TileBase[] tileArray;
    private bool isOpened;

    // Start is called before the first frame update
    void Start()
    {
        positions = new Vector3Int[3];
        tileArray = new TileBase[positions.Length];
        positions[0] = new Vector3Int(-1,-2,0);
        positions[1] = new Vector3Int(-1,-3,0);
        positions[2] = new Vector3Int(-1,-4,0);

        // For prefabs getting Tilemap reference
        _tileMap = GameObject.Find("Wall");
        tileMap = _tileMap.GetComponent<Tilemap>();
        shadowCaster = _tileMap.GetComponent<ShadowCaster2DCreator>();

        isOpened = false;
    }

    
    void FixedUpdate()
    {
        Move();
        OpenDoor();
        shadowCaster.Create();
    }
    // Update is called once per frame
    void Update() {
        SetLights();
        SpawnClone();
    }

    void Move()
    {
        float moveSpeed = 10;
        //Define the speed at which the object moves.

        float horizontalInput = Input.GetAxis ("Horizontal"); 
        //Get the value of the Horizontal input axis.

        float verticalInput = Input.GetAxis("Vertical");
        //Get the value of the Vertical input axis.

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime);
        //Move the object to XYZ coordinates defined as horizontalInput, 0, and verticalInput respectively.
    }

    void SetLights(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            spotLightManager.TurnOffLight(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            spotLightManager.TurnOffLight(1);
        }
    }

    void SpawnClone(){
        if (Input.GetMouseButtonDown(0)){
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(player, new Vector3(cursorPos.x, cursorPos.y, 0), Quaternion.identity);
        }
    }

    void OpenDoor(){
        Vector3 playerPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        if(tileMap.WorldToCell(playerPos) == new Vector3(-6,2,0) && !isOpened){
            Debug.Log("Stepped on button at: " + tileMap.WorldToCell(playerPos));
            tileMap.SetTiles(positions, tileArray);
            isOpened = true;
        }
    }
}

