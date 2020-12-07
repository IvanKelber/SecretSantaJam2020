using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Manifest/Effects")]
public class EffectManifest : Manifest<RewardEffect>
{
    public float rareEffectChance = .1f;

    public float epicEffectChance = .02f;

    public List<RewardConfig> GetRewards(int numberOfRewards) {
        Dictionary<string, RewardConfig> rewards = new Dictionary<string, RewardConfig>();
        for(int i = 0; i < numberOfRewards; i++) {
            float quality = Random.Range(0f,1f);
            RewardConfig.Rarity rarity;
            if(quality < epicEffectChance) {
                rarity = RewardConfig.Rarity.Epic;
            } else if(quality < rareEffectChance) {
                rarity = RewardConfig.Rarity.Rare;
            } else {
                rarity = RewardConfig.Rarity.Common;
            }
            RewardConfig reward = GenerateRandomReward(rarity);
            while(rewards.ContainsKey(reward.GetTitle())) {
                reward = GenerateRandomReward(rarity);
            }
            rewards.Add(reward.GetTitle(), reward);
        }
        List<RewardConfig> r = new List<RewardConfig>();
        foreach(string key in rewards.Keys) {
            r.Add(rewards[key]);
        }
        return r;

    }

    public RewardConfig GenerateRandomReward(RewardConfig.Rarity rarity) {
        // Shuffle RewardEffects
        for (int i = 0; i < items.Count; i++) {
            RewardEffect temp = items[i];
            int randomIndex = Random.Range(i, items.Count);
            items[i] = items[randomIndex];
            items[randomIndex] = temp;
        }
        if(items.Count < 2) {
            return null;
        }
        return new RewardConfig(rarity, items[0], items[1]);
    }
}
