using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TileTrigger : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap component
    public TileBase triggerTile; // Reference to the specific tile that triggers behavior
    public string nextSceneName; // Name of the next scene to load

    void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.collider.CompareTag("Player"))
        {
            // Get the position of the player
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3Int cellPosition = tilemap.WorldToCell(contactPoint);

            // Get the tile at the player's position
            TileBase collidedTile = tilemap.GetTile(cellPosition);

            // Check if the collided tile is a trigger tile
            if (collidedTile != null && collidedTile==triggerTile)
            {
                // Load the next scene
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

}