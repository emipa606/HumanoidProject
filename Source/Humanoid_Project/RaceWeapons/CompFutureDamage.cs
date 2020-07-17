using System;
using Verse;

namespace RaceWeapons
{
	// Token: 0x02000034 RID: 52
	public class CompFutureDamage : ThingComp
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00005640 File Offset: 0x00003840
		public override void Initialize(CompProperties vprops)
		{
			base.Initialize(vprops);
			this.compProp = (vprops as CompProperties_RaceDamage);
			if (this.compProp != null)
			{
				this.damageDef = this.compProp.damageDef;
				this.damageAmount = this.compProp.damageAmount;
				this.chanceToProc = this.compProp.chanceToProc;
				return;
			}
			Log.Message("Error", false);
			this.count = 9876;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000056B2 File Offset: 0x000038B2
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x04000049 RID: 73
		public CompProperties_RaceDamage compProp;

		// Token: 0x0400004A RID: 74
		public string damageDef;

		// Token: 0x0400004B RID: 75
		public int damageAmount;

		// Token: 0x0400004C RID: 76
		public float chanceToProc;

		// Token: 0x0400004D RID: 77
		public int count;
	}
}
