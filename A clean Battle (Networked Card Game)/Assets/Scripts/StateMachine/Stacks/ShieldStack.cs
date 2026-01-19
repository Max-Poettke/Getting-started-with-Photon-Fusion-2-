using UnityEngine;

public class ShieldStack : ScriptableStat
{
    public override void Initialize(){
        description = "Shield\nReduces incoming damage by its amount";
    }

    public override void OnBeforeDamage(DamageContext ctx)
    {
        if (ctx.IgnoreShield || ctx.Amount <= 0)
            return;

        int absorbed = Mathf.Min(amount, ctx.Amount);
        amount -= absorbed;
        ctx.Amount -= absorbed;

        if (amount <= 0)
        {
            ctx.Target.RemoveStatAndUpdateUI(this);
        }
    }
}
