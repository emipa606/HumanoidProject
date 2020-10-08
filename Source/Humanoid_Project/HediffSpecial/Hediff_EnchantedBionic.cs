using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace HediffSpecial
{
	// Token: 0x02000019 RID: 25
	public class Hediff_EnchantedBionic : HediffWithComps
	{
		// Token: 0x0600004A RID: 74 RVA: 0x000039E7 File Offset: 0x00001BE7
		public override void PostMake()
		{
			base.PostMake();
			this.SetNextHealTick();
			this.SetNextGrowTick();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000039FB File Offset: 0x00001BFB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksUntilNextHeal, "ticksUntilNextHeal", 0, false);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003A18 File Offset: 0x00001C18
		public override void Tick()
		{
			base.Tick();
			if (Current.Game.tickManager.TicksGame >= this.ticksUntilNextHeal)
			{
				this.TrySealWounds();
				this.SetNextHealTick();
			}
			if (Current.Game.tickManager.TicksGame >= this.ticksUntilNextGrow && this.def.TryGetModExtension<DefModExtension_BionicSpecial>().regrowParts)
			{
				this.TryRegrowBodyparts();
				this.SetNextGrowTick();
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003A84 File Offset: 0x00001C84
		public void TrySealWounds()
		{
			IEnumerable<Hediff> enumerable = from hd in this.pawn.health.hediffSet.hediffs
			where hd.TendableNow(false)
			select hd;
			if (enumerable != null)
			{
				foreach (Hediff hediff in enumerable)
				{
                    if (hediff is HediffWithComps hediffWithComps)
                    {
                        HediffComp_TendDuration hediffComp_TendDuration = hediffWithComps.TryGetComp<HediffComp_TendDuration>();
                        hediffComp_TendDuration.tendQuality = 2f;
                        hediffComp_TendDuration.tendTicksLeft = Find.TickManager.TicksGame;
                        this.pawn.health.Notify_HediffChanged(hediff);
                    }
                }
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003B44 File Offset: 0x00001D44
		public void TryRegrowBodyparts()
		{
			if (this.def.TryGetModExtension<DefModExtension_BionicSpecial>().protoBodyPart != null)
			{
				using (IEnumerator<BodyPartRecord> enumerator = this.pawn.GetFirstMatchingBodyparts(this.pawn.RaceProps.body.corePart, HediffDefOf.MissingBodyPart, this.def.TryGetModExtension<DefModExtension_BionicSpecial>().protoBodyPart, (Hediff hediff) => hediff is Hediff_AddedPart).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BodyPartRecord part = enumerator.Current;
						Hediff hediff3 = this.pawn.health.hediffSet.hediffs.First((Hediff hediff) => hediff.Part == part && hediff.def == HediffDefOf.MissingBodyPart);
						if (hediff3 != null)
						{
							this.pawn.health.RemoveHediff(hediff3);
							this.pawn.health.AddHediff(this.def.TryGetModExtension<DefModExtension_BionicSpecial>().protoBodyPart, part, null, null);
							this.pawn.health.hediffSet.DirtyCache();
						}
					}
					return;
				}
			}
			using (IEnumerator<BodyPartRecord> enumerator2 = this.pawn.GetFirstMatchingBodyparts(this.pawn.RaceProps.body.corePart, HediffDefOf.MissingBodyPart, HediffDefOf_CosmosInd.CosmosRegrowingTech, (Hediff hediff) => hediff is Hediff_AddedPart).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					BodyPartRecord part = enumerator2.Current;
					Hediff hediff2 = this.pawn.health.hediffSet.hediffs.First((Hediff hediff) => hediff.Part == part && hediff.def == HediffDefOf.MissingBodyPart);
					if (hediff2 != null)
					{
						this.pawn.health.RemoveHediff(hediff2);
						this.pawn.health.AddHediff(HediffDefOf_CosmosInd.CosmosRegrowingTech, part, null, null);
						this.pawn.health.hediffSet.DirtyCache();
					}
				}
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003D94 File Offset: 0x00001F94
		public void SetNextHealTick()
		{
			this.ticksUntilNextHeal = Current.Game.tickManager.TicksGame + this.def.TryGetModExtension<DefModExtension_BionicSpecial>().healTicks;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003DBC File Offset: 0x00001FBC
		public void SetNextGrowTick()
		{
			this.ticksUntilNextGrow = Current.Game.tickManager.TicksGame + this.def.TryGetModExtension<DefModExtension_BionicSpecial>().growthTicks;
		}

		// Token: 0x0400002F RID: 47
		public int ticksUntilNextHeal;

		// Token: 0x04000030 RID: 48
		public int ticksUntilNextGrow;
	}
}
