﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardReward : Reward
{
    [SerializeField]
    Image image;

    [SerializeField]
    TMP_Text cardTitle;

    [SerializeField]
    TMP_Text cardDescription;

    public override void Init(RewardConfig config) {
        base.Init(config);
        // image.sprite = config.rewardSprite;
        cardTitle.text = config.GetTitle();
        cardDescription.text = config.GetDescription();
    }

    public override void OnEnterTrigger(Collider2D collider) {
        transform.localScale *= 2;
    }

    public override void OnExitTrigger(Collider2D collider) {
        transform.localScale /= 2;
    }
}
