using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manifest<T> : ScriptableObject 
                 where T : ScriptableObject
{
    public List<T> items = new List<T>();
    private Dictionary<string, T> itemMap = new Dictionary<string, T>();

    void OnEnable() {
        foreach(T item in items) {
            itemMap.Add(item.name, item);
        }
    }

    void Refresh() {
        foreach(T item in items) {
            if(!itemMap.ContainsKey(item.name)) {
                itemMap.Add(item.name, item);
            }
        }
    }

    public T Get(string name) {
        return itemMap[name];
    }

    public List<T> GetList() {
        return items;
    }
}
