using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Manifest/Rewards")]
public class RewardManifest : Manifest<RewardConfig>
{
    public List<RewardConfig> Get(int numberOfRewards) {
        return items;
    }
}
