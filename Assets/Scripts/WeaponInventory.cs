using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{

    public RectTransform inventoryDisplay;
    List<Image> hudGunImages = new List<Image>();

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
        GetSlotImages();
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
        int newIndex = gunIndex;
        if(HasRoom()) {
            gunConfigs.Add(config);
            newIndex = Count - 1;
        } else {
            GunConfig droppedConfig = gunConfigs[gunIndex];
            gunConfigs[gunIndex] = config;
        }
        hudGunImages[newIndex].sprite = config.gunSprite;
        hudGunImages[newIndex].color = Color.white;
        Equip(newIndex);
    }

    public void Equip(int index) {

        hudGunImages[gunIndex].transform.parent.GetComponent<Image>().color = Color.white * new Color(1,1,1,.3f);
        hudGunImages[index].transform.parent.GetComponent<Image>().color = Color.yellow;
        gun.SetConfig(gunConfigs[index]);
        gunIndex = index;
    }

    public void Reset() {
        gunConfigs.Clear();
        ClearImages();
        Pickup(defaultConfig);
    }

    void GetSlotImages() {
        foreach(Transform slot in inventoryDisplay) {
            hudGunImages.Add(slot.GetChild(0).GetComponent<Image>());
        }
    }

    void ClearImages() {
        foreach(Image image in hudGunImages) {
            image.sprite = null;
            image.color = Color.clear;
        }
    }

}
