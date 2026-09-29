using Content.Shared.Weapons.Melee.Events;
using Content.Shared.PowerCell;
using Content.Shared.Tools.Components;

namespace Content.Shared._OuterHorizons.Durability;

public abstract partial class SharedDurabilitySystem : EntitySystem
{
    [Dependency] private readonly PowerCellSystem _powerCell = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<DurabilityComponent, MeleeHitEvent>(OnMeleeHit);
        SubscribeLocalEvent<DurabilityComponent, AttemptMeleeEvent>(OnMeleeAttempt);
        SubscribeLocalEvent<DurabilityComponent, ToolUseAttemptEvent>(OnToolAttempt);
    }
    private void OnMeleeHit(Entity<DurabilityComponent> ent, ref MeleeHitEvent args)
    {
        if (args.HitEntities.Count == 0)
            return;

        DurabilityUse(ent.Owner, ent.Comp);
    }

    private void OnMeleeAttempt(Entity<DurabilityComponent> ent, ref AttemptMeleeEvent args)
    {
        if (ent.Comp.UseEnergy && !_powerCell.HasCharge(ent.Owner, ent.Comp.ChargePerUse, user: args.User))
            args.Cancelled = true;
    }

    private void OnToolAttempt(Entity<DurabilityComponent> ent, ref ToolUseAttemptEvent args)
    {
        if (ent.Comp.UseEnergy && !_powerCell.HasCharge(ent.Owner, ent.Comp.ChargePerUse, user: args.User))
            args.Cancel();
    }

    public void DurabilityUse(EntityUid uid, DurabilityComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (component.UseEnergy)
            _powerCell.TryUseCharge(uid, component.ChargePerUse);
        else
        {
            component.Damage++;
            if (component.Damage >= component.MaxDurability)
                Break(uid); //потом добавить звук ломания
        }
    }

    protected virtual void Break(EntityUid uid)
    {
        //На серверной части
    }
}
