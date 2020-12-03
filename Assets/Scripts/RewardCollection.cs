using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class RewardCollection : MonoBehaviour
{
    public int numberOfRewards = 3;
    [SerializeField]
    RewardManifest rewardManifest;

    [SerializeField]
    BoolGameEvent lockDoors;

    List<RewardConfig> rewards = new List<RewardConfig>();

    void Start()
    {
        rewards = rewardManifest.Get(numberOfRewards);
        for(int i = 0; i < rewards.Count; i++) {
            Vector3 position = transform.position + new Vector3(5 *(i - 1), 0, 0);
            GameObject rewardObj = Instantiate(rewards[i].rewardPrefab, position, Quaternion.identity);
            rewardObj.GetComponent<Reward>().Init(rewards[i]);
            rewardObj.transform.parent = transform;
        }
    }


    public void OnRewardChosen(GameObject reward) {
        string n = reward.GetComponent<Reward>().config.name;
        lockDoors.Raise(false);
        Debug.Log(n + " HAS BEEN CHOSEN");
        Destroy(this.gameObject);
        
    }
}
