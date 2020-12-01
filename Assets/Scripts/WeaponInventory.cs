using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{

    public List<Image> hudGunImages = new List<Image>();

    [SerializeField]
    Gun gun;

    [SerializeField]
    List<GunConfig> gunConfigs = new List<GunConfig>();

    public GunConfig defaultConfig;

    public int Count {
        get {return gunConfigs.Count;}
    }

    public Gun Gun {
        get {return gun;}
    }

    int capacity = 2;
    int gunIndex = 0;

    void Start() {
        Reset();
    }

    void Update()
    {
        if(StaticUserControls.paused) {
            return;
        }
        GetEquipGun();
    }

    void GetEquipGun() {
        for(int i = 1; i <= gunConfigs.Count; i++) {
            if(Input.GetKeyDown("" + i)) {
                Equip(i - 1);
                return;
            }
        }
    }

    public bool HasRoom() {
        return Count < capacity;
    }

    public void Pickup(GunConfig config) {
        if(gunConfigs.Contains(config)) {
            return;
        }
        if(HasRoom()) {
            gunConfigs.Add(config);
            gunIndex = Count - 1;
        } else {
            GunConfig droppedConfig = gunConfigs[gunIndex];
            gunConfigs[gunIndex] = config;
        }
        Equip(gunIndex);
    }

    public void Equip(int index) {
        gun.SetConfig(gunConfigs[index]);
        gunIndex = index;
    }

    public void Reset() {
        gunConfigs.Clear();
        Pickup(defaultConfig);
    }

}
