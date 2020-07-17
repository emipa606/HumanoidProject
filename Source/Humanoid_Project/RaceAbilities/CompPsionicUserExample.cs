using System;
using AbilityUser;
using Verse;

namespace RaceAbilities
{
	// Token: 0x02000042 RID: 66
	public class CompPsionicUserExample : CompAbilityUser
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00006F8C File Offset: 0x0000518C
		private bool IsAvianRace
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
				return flag && base.Pawn.health.hediffSet.HasHediff(HediffDef.Named("AvianHeDiff"), false);
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006FE0 File Offset: 0x000051E0
		public override bool TryTransformPawn()
		{
			return this.IsAvianRace;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006FE8 File Offset: 0x000051E8
		public override void CompTick()
		{
			Pawn pawn = base.Pawn;
			if (pawn == null || !pawn.Spawned)
			{
				return;
			}
			if (Find.TickManager.TicksGame > 200 && this.IsAvianRace)
			{
				if (!this.firstTick)
				{
					this.PostInitializeTick();
				}
				base.CompTick();
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000703C File Offset: 0x0000523C
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
				base.AddPawnAbility(RaceAbilitiesDefOf.AvianPower, true, -1f);
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000070AF File Offset: 0x000052AF
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.gaveAbilities, "gaveAbilities", false, false);
		}

		// Token: 0x04000077 RID: 119
		private bool gaveAbilities;

		// Token: 0x04000078 RID: 120
		private bool firstTick;
	}
}
