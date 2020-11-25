using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName="Tileset")]
public class Tileset : ScriptableObject
{
    public List<SerializableTile> tiles = new List<SerializableTile>();
    Dictionary<string, TileBase> tileDictionary = new Dictionary<string, TileBase>();

    private void OnEnable() {
        foreach(SerializableTile tile in tiles) {
            tileDictionary.Add(tile.name, tile.tile);
        }
    }

    public void Refresh() {
        foreach(SerializableTile tile in tiles) {
            if(!tileDictionary.ContainsKey(tile.name)) {
                tileDictionary.Add(tile.name, tile.tile);
            }
        }
    }

    public TileBase Get(string name) {
        return tileDictionary[name];
    }
    
}
