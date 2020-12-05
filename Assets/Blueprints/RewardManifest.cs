using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Manifest/Rewards")]
public class RewardManifest : Manifest<RewardConfig>
{
    public List<RewardConfig> Get(int numberOfRewards) {
        List<RewardConfig> rewards = new List<RewardConfig>();
        for (int i = 0; i < items.Count; i++) {
            RewardConfig temp = items[i];
            int randomIndex = Random.Range(i, items.Count);
            items[i] = items[randomIndex];
            items[randomIndex] = temp;
        }
        for(int i = 0; i < Mathf.Min(numberOfRewards, items.Count); i++) {
            rewards.Add(items[i]);
        }
        return rewards;
    }
}
