using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackEffect
{
    
    void Apply(Character target);

}

public class DamageHealth : IAttackEffect
{
    private int minDamage;
    private int maxDamage;

    public DamageHealth(int value)
    {
        this.minDamage = value;
        this.maxDamage = value;
    }

    public DamageHealth(int minValue, int maxValue)
    {
        this.minDamage = minValue;
        this.maxDamage = maxValue;
    }

    public void Apply(Character target)
    {
        target.Health -= Random.Range(minDamage, maxDamage);
    }
}

//public class DamageStamina : IAttackEffect
//{

//}
