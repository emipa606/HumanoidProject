using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RaceAbilities
{
	// Token: 0x02000040 RID: 64
	public static class BlueFireUtility
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x00006A5D File Offset: 0x00004C5D
		public static bool CanEverAttachFire(this Thing t)
		{
			return !t.Destroyed && t.FlammableNow && t.def.category == ThingCategory.Pawn && t.TryGetComp<CompAttachBase>() != null;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006A88 File Offset: 0x00004C88
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

		// Token: 0x060000D4 RID: 212 RVA: 0x00006BB6 File Offset: 0x00004DB6
		public static bool TryStartFireIn(IntVec3 c, Map map, float fireSize)
		{
			if (BlueFireUtility.ChanceToStartFireIn(c, map) <= 0f)
			{
				return false;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.BlueFire, null);
			fire.fireSize = fireSize;
			GenSpawn.Spawn(fire, c, map, Rot4.North, WipeMode.Vanish, false);
			return true;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006BF0 File Offset: 0x00004DF0
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
			if (t.HasAttachment(ThingDefOf.BlueFire))
			{
				return;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.BlueFire, null);
			fire.fireSize = fireSize;
			fire.AttachTo(t);
			GenSpawn.Spawn(fire, t.Position, t.Map, Rot4.North, WipeMode.Vanish, false);
            if (t is Pawn pawn)
            {
                pawn.jobs.StopAll(false);
                pawn.records.Increment(RecordDefOf.TimesOnFire);
            }
        }

		// Token: 0x060000D6 RID: 214 RVA: 0x00006C83 File Offset: 0x00004E83
		public static bool IsBurning(this TargetInfo t)
		{
			if (t.HasThing)
			{
				return t.Thing.IsBurning();
			}
			return t.Cell.ContainsStaticFire(t.Map);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006CB0 File Offset: 0x00004EB0
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
				return t.HasAttachment(ThingDefOf.BlueFire);
			}
			return t.Position.ContainsStaticFire(t.Map);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006D44 File Offset: 0x00004F44
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

		// Token: 0x060000D9 RID: 217 RVA: 0x00006D8C File Offset: 0x00004F8C
		public static bool ContainsTrap(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice is Building_Trap;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006DAF File Offset: 0x00004FAF
		public static bool Flammable(this TerrainDef terrain)
		{
			return terrain.GetStatValueAbstract(StatDefOf.Flammability, null) > 0.01f;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006DC4 File Offset: 0x00004FC4
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
