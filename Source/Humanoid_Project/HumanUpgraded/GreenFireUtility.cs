using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace HumanUpgraded
{
	// Token: 0x0200000C RID: 12
	public static class GreenFireUtility
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002C79 File Offset: 0x00000E79
		public static bool CanEverAttachFire(this Thing t)
		{
			return !t.Destroyed && t.FlammableNow && t.def.category == ThingCategory.Pawn && t.TryGetComp<CompAttachBase>() != null;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002CA4 File Offset: 0x00000EA4
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

		// Token: 0x06000025 RID: 37 RVA: 0x00002DD2 File Offset: 0x00000FD2
		public static bool TryStartFireIn(IntVec3 c, Map map, float fireSize)
		{
			if (GreenFireUtility.ChanceToStartFireIn(c, map) <= 0f)
			{
				return false;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.GreenFire, null);
			fire.fireSize = fireSize;
			GenSpawn.Spawn(fire, c, map, Rot4.North, WipeMode.Vanish, false);
			return true;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002E0C File Offset: 0x0000100C
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
			if (t.HasAttachment(ThingDefOf.GreenFire))
			{
				return;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.GreenFire, null);
			fire.fireSize = fireSize;
			fire.AttachTo(t);
			GenSpawn.Spawn(fire, t.Position, t.Map, Rot4.North, WipeMode.Vanish, false);
            if (t is Pawn pawn)
            {
                pawn.jobs.StopAll(false);
                pawn.records.Increment(RecordDefOf.TimesOnFire);
            }
        }

		// Token: 0x06000027 RID: 39 RVA: 0x00002E9F File Offset: 0x0000109F
		public static bool IsBurning(this TargetInfo t)
		{
			if (t.HasThing)
			{
				return t.Thing.IsBurning();
			}
			return t.Cell.ContainsStaticFire(t.Map);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002ECC File Offset: 0x000010CC
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
				return t.HasAttachment(ThingDefOf.GreenFire);
			}
			return t.Position.ContainsStaticFire(t.Map);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002F60 File Offset: 0x00001160
		public static bool ContainsStaticFire(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
                if (list[i] is Fire fire && fire.parent == null)
                {
                    return true;
                }
            }
			return false;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002FA8 File Offset: 0x000011A8
		public static bool ContainsTrap(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice is Building_Trap;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002FCB File Offset: 0x000011CB
		public static bool Flammable(this TerrainDef terrain)
		{
			return terrain.GetStatValueAbstract(StatDefOf.Flammability, null) > 0.01f;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002FE0 File Offset: 0x000011E0
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
