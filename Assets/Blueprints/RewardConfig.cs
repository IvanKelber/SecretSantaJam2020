using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Configs/Reward")]
public class RewardConfig : ScriptableObject
{
    public RewardType type;

    public string name;
    public string description;
    public GameObject rewardPrefab;

    public Sprite rewardSprite;

    public List<RewardEffect> effects;

    public void ApplyEffect(PlayerValues values) {
        foreach(RewardEffect effect in effects) {
            effect.Apply(values);
        }
    }

}
