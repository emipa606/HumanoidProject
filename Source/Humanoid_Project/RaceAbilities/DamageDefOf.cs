using System;
using RimWorld;
using Verse;

namespace RaceAbilities
{
	// Token: 0x0200003F RID: 63
	[DefOf]
	public static class DamageDefOf
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x00006A4C File Offset: 0x00004C4C
		static DamageDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DamageDefOf));
		}

		// Token: 0x04000076 RID: 118
		public static DamageDef BlueFire;
	}
}
