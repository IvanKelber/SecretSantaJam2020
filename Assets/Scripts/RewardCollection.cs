using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class RewardCollection : MonoBehaviour
{
    public int numberOfRewards = 3;
    [SerializeField]
    EffectManifest effectManifest;

    [SerializeField]
    GameObject cardPrefab;

    [SerializeField]
    BoolGameEvent lockDoors;

    List<RewardConfig> rewards = new List<RewardConfig>();

    void Start()
    {
        rewards = effectManifest.GetRewards(numberOfRewards);
        for(int i = 0; i < rewards.Count; i++) {
            Vector3 position = transform.position + new Vector3(5 *(i - 1), 0, 0);
            GameObject rewardObj = Instantiate(cardPrefab, position, Quaternion.identity);
            rewardObj.GetComponent<Reward>().Init(rewards[i]);
            rewardObj.transform.parent = transform;
        }
    }


    public void OnRewardChosen(GameObject reward) {
        Reward r = reward.GetComponent<Reward>();
        r.ApplyEffect();
        lockDoors.Raise(false);
        Destroy(this.gameObject);
        
    }
}
