using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
[CreateAssetMenu(menuName="Configs/Reward")]
public class RewardConfig
{

    public enum Rarity {
        Common,
        Rare,
        Epic
    }

    public RewardType type;
    public RewardConfig.Rarity rarity;

    public string name;
    public string description;

    public RewardEffect negativeEffect;
    public RewardEffect positiveEffect;

    private float positiveValue;
    private float negativeValue;

    public RewardConfig(RewardConfig.Rarity rarity, RewardEffect positiveEffect, RewardEffect negativeEffect) {
        this.rarity = rarity;
        this.positiveEffect = positiveEffect;
        this.negativeEffect = negativeEffect;
        GenerateValues();
    }

    public void GenerateValues() {
        positiveValue = PositiveValue();
        negativeValue = NegativeValue();
    }

    public string GetDescription() {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(positiveEffect.effectText, positiveValue);
        if(rarity != Rarity.Common) {
            sb.Append("\n");
            string negativeText = negativeEffect.effectText;
            negativeText = negativeText.Replace("Increase", "Reduce");
            sb.AppendFormat(negativeText, negativeValue);
        }
        return sb.ToString();
    }

    public string GetTitle() {
        StringBuilder sb = new StringBuilder();
        if(rarity == Rarity.Epic) {
            sb.Append("Incredibly ");
        }
        if(rarity != Rarity.Common) {
            sb.Append(negativeEffect.adjective);
            sb.Append(" ");
        }
        sb.AppendFormat(positiveEffect.descriptor);
        return sb.ToString();
    }

    public int PositiveValue() {
        return ((int) rarity + 1) * (int)positiveEffect.value;
    }

    public int NegativeValue() {
        return ((int) rarity) * (int)negativeEffect.value;
    }

    public void ApplyEffect(PlayerValues values) {
        negativeEffect.Apply(-((int)rarity), values);
        positiveEffect.Apply(((int)rarity + 1), values);
             
    }

}
