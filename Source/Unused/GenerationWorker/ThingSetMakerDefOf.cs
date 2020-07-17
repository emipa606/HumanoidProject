using System;
using RimWorld;

namespace GenerationWorker
{
	// Token: 0x02000027 RID: 39
	[DefOf]
	public static class ThingSetMakerDefOf
	{
		// Token: 0x06000081 RID: 129 RVA: 0x000050B4 File Offset: 0x000032B4
		static ThingSetMakerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThingSetMakerDefOf));
		}

		// Token: 0x0400003E RID: 62
		public static ThingSetMakerDef Gen_OldOutpost;

		// Token: 0x0400003F RID: 63
		public static ThingSetMakerDef RewardOptions;
	}
}
