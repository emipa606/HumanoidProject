using System;
using AbilityUser;
using Verse;

namespace HumanUpgraded
{
	// Token: 0x02000011 RID: 17
	public class CompHumanUpgraded : CompAbilityUser
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00003438 File Offset: 0x00001638
		private bool IsHumanUpgraded
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
					flag = (((health != null) ? health.hediffSet : null) != null);
				}
				return flag && base.Pawn.health.hediffSet.HasHediff(HediffDef.Named("GreenStormHediff"), false);
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000348C File Offset: 0x0000168C
		public override bool TryTransformPawn()
		{
			return this.IsHumanUpgraded;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003494 File Offset: 0x00001694
		public override void CompTick()
		{
			Pawn pawn = base.Pawn;
			if (pawn == null || !pawn.Spawned)
			{
				return;
			}
			if (Find.TickManager.TicksGame > 350 && this.IsHumanUpgraded)
			{
				if (!this.firstTick)
				{
					this.PostInitializeTick();
				}
				base.CompTick();
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000034E8 File Offset: 0x000016E8
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
				base.AddPawnAbility(UpgradedAbilitiesDefOf.UpgradedStorm, true, -1f);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000355B File Offset: 0x0000175B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.gaveAbilities, "gaveAbilities", false, false);
		}

		// Token: 0x0400001D RID: 29
		private bool gaveAbilities;

		// Token: 0x0400001E RID: 30
		private bool firstTick;
	}
}
