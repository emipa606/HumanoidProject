using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace HumanAbilities
{
	// Token: 0x02000004 RID: 4
	public static class RedFireUtility
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000021E1 File Offset: 0x000003E1
		public static bool CanEverAttachFire(this Thing t)
		{
			return !t.Destroyed && t.FlammableNow && t.def.category == ThingCategory.Pawn && t.TryGetComp<CompAttachBase>() != null;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000220C File Offset: 0x0000040C
		public static float ChanceToStartFireIn(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			float num = (!c.TerrainFlammableNow(map)) ? 0f : c.GetTerrain(map).GetStatValueAbstract(StatDefOf.Flammability, null);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing is Fire)
				{
					return 0f;
				}
				if (thing.def.category != ThingCategory.Pawn && thingList[i].FlammableNow)
				{
					num = Mathf.Max(num, thing.GetStatValue(StatDefOf.Flammability, true));
				}
			}
			if (num > 0f)
			{
				Building edifice = c.GetEdifice(map);
				if (edifice != null && edifice.def.passability == Traversability.Impassable && edifice.OccupiedRect().ContractedBy(1).Contains(c))
				{
					return 0f;
				}
				List<Thing> thingList2 = c.GetThingList(map);
				for (int j = 0; j < thingList2.Count; j++)
				{
					if (thingList2[j].def.category == ThingCategory.Filth && !thingList2[j].def.filth.allowsFire)
					{
						return 0f;
					}
				}
			}
			return num;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000233A File Offset: 0x0000053A
		public static bool TryStartFireIn(IntVec3 c, Map map, float fireSize)
		{
			if (RedFireUtility.ChanceToStartFireIn(c, map) <= 0f)
			{
				return false;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.RedFire, null);
			fire.fireSize = fireSize;
			GenSpawn.Spawn(fire, c, map, Rot4.North, WipeMode.Vanish, false);
			return true;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002374 File Offset: 0x00000574
		public static void TryAttachFire(this Thing t, float fireSize)
		{
			if (t.Map == null)
			{
				return;
			}
			if (!t.Spawned)
			{
				return;
			}
			if (!t.CanEverAttachFire())
			{
				return;
			}
			if (t.HasAttachment(ThingDefOf.RedFire))
			{
				return;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.RedFire, null);
			fire.fireSize = fireSize;
			fire.AttachTo(t);
			GenSpawn.Spawn(fire, t.Position, t.Map, Rot4.North, WipeMode.Vanish, false);
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				pawn.jobs.StopAll(false);
				pawn.records.Increment(RecordDefOf.TimesOnFire);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002407 File Offset: 0x00000607
		public static bool IsBurning(this TargetInfo t)
		{
			if (t.HasThing)
			{
				return t.Thing.IsBurning();
			}
			return t.Cell.ContainsStaticFire(t.Map);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002434 File Offset: 0x00000634
		public static bool IsBurning(this Thing t)
		{
			if (t.Destroyed || !t.Spawned)
			{
				return false;
			}
			if (!(t.def.size == IntVec2.One))
			{
				CellRect.CellRectIterator iterator = t.OccupiedRect().GetIterator();
				while (!iterator.Done())
				{
					if (iterator.Current.ContainsStaticFire(t.Map))
					{
						return true;
					}
					iterator.MoveNext();
				}
				return false;
			}
			if (t is Pawn)
			{
				return t.HasAttachment(ThingDefOf.RedFire);
			}
			return t.Position.ContainsStaticFire(t.Map);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000024C8 File Offset: 0x000006C8
		public static bool ContainsStaticFire(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Fire fire = list[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002510 File Offset: 0x00000710
		public static bool ContainsTrap(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice is Building_Trap;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002533 File Offset: 0x00000733
		public static bool Flammable(this TerrainDef terrain)
		{
			return terrain.GetStatValueAbstract(StatDefOf.Flammability, null) > 0.01f;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002548 File Offset: 0x00000748
		public static bool TerrainFlammableNow(this IntVec3 c, Map map)
		{
			if (!c.GetTerrain(map).Flammable())
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].FireBulwark)
				{
					return false;
				}
			}
			return true;
		}
	}
}
