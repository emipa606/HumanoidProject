using System;
using AbilityUser;
using Verse;

namespace HumanAbilities
{
	// Token: 0x02000009 RID: 9
	public class CompPsionicUserExample : CompAbilityUser
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000029A0 File Offset: 0x00000BA0
		private bool IsHumanRace
		{
			get
			{
				Pawn pawn = base.Pawn;
				bool flag;
				if (pawn == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn_HealthTracker health = pawn.health;
					flag = ((health?.hediffSet) != null);
				}
				return flag && base.Pawn.health.hediffSet.HasHediff(HediffDef.Named("RedLightningImpact"), false);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000029F4 File Offset: 0x00000BF4
		public override bool TryTransformPawn()
		{
			return this.IsHumanRace;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000029FC File Offset: 0x00000BFC
		public override void CompTick()
		{
			Pawn pawn = base.Pawn;
			if (pawn == null || !pawn.Spawned)
			{
				return;
			}
			if (Find.TickManager.TicksGame > 200 && this.IsHumanRace)
			{
				if (!this.firstTick)
				{
					this.PostInitializeTick();
				}
				base.CompTick();
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002A50 File Offset: 0x00000C50
		private void PostInitializeTick()
		{
			Pawn pawn = base.Pawn;
			if (pawn == null || !pawn.Spawned)
			{
				return;
			}
			if (base.Pawn.story != null && base.Pawn.story.DisabledWorkTagsBackstoryAndTraits.OverlapsWithOnAnyWorkType(WorkTags.Violent))
			{
				return;
			}
			this.firstTick = true;
			this.Initialize();
			if (!this.gaveAbilities)
			{
				this.gaveAbilities = true;
				base.AddPawnAbility(HumanAbilitiesDefOf.HumanPower, true, -1f);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002AC3 File Offset: 0x00000CC3
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.gaveAbilities, "gaveAbilities", false, false);
		}

		// Token: 0x0400000E RID: 14
		private bool gaveAbilities;

		// Token: 0x0400000F RID: 15
		private bool firstTick;
	}
}
