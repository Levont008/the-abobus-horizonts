using Content.Shared.Weapons.Melee.Events;
using Content.Shared.PowerCell;

namespace Content.Shared._OuterHorizons.Durability;

public abstract partial class DurabilitySystem : EntitySystem
{
    [Dependency] private readonly PowerCellSystem _powerCell = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<DurabilityComponent, MeleeHitEvent>(OnMeleeHit);
        SubscribeLocalEvent<DurabilityComponent, AttemptMeleeEvent>(OnMeleeAttempt);
    }

    private void OnMeleeHit(Entity<DurabilityComponent> ent, ref MeleeHitEvent args)
    {
        if (ent.Comp.UseEnergy)
            _powerCell.TryUseCharge(ent.Owner, ent.Comp.ChargePerUse);
        else
        {
            ent.Comp.Damage--;
            if (ent.Comp.Damage <= 0)
                PredictedQueueDel(ent.Owner); //потом добавить звук ломания
        }
    }

    private void OnMeleeAttempt(Entity<DurabilityComponent> ent, ref AttemptMeleeEvent args)
    {
        if (ent.Comp.UseEnergy && !_powerCell.HasCharge(ent.Owner, ent.Comp.ChargePerUse, user: args.User))
            args.Cancelled = true;
    }
}
