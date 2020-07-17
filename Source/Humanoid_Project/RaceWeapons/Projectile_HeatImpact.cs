using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RaceWeapons
{
	// Token: 0x02000036 RID: 54
	public class Projectile_HeatImpact : Projectile
	{
		// Token: 0x0600009F RID: 159 RVA: 0x000056EF File Offset: 0x000038EF
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.drawingTexture = this.def.DrawMatSingle;
			this.compED = base.GetComp<CompFutureDamage>();
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00005716 File Offset: 0x00003916
		public ThingDef_RaceWeapons Props
		{
			get
			{
				return this.def as ThingDef_RaceWeapons;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005723 File Offset: 0x00003923
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005740 File Offset: 0x00003940
		public override void Tick()
		{
			if (this.tickCounter == 0)
			{
				this.PerformPreFiringTreatment();
			}
			if (this.tickCounter < this.Props.TickOffset)
			{
				this.GetPreFiringDrawingParameters();
			}
			else
			{
				if (this.tickCounter == this.Props.TickOffset)
				{
					this.Fire();
				}
				this.GetPostFiringDrawingParameters();
			}
			if (this.tickCounter == this.Props.TickOffset + this.Props.TickOffsetSecond)
			{
				base.Tick();
				this.Destroy(DestroyMode.Vanish);
			}
			Pawn pawn = this.launcher as Pawn;
			if (pawn != null)
			{
				Pawn_StanceTracker stances = pawn.stances;
				if (stances != null && (!(stances.curStance is Stance_Busy) || pawn.Dead))
				{
					this.Destroy(DestroyMode.Vanish);
				}
			}
			this.tickCounter++;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005804 File Offset: 0x00003A04
		public virtual void PerformPreFiringTreatment()
		{
			this.DetermineImpactExactPosition();
			Vector3 vector = (this.destination - this.origin).normalized * 0.9f;
			this.drawingScale = new Vector3(1f, 1f, (this.destination - this.origin).magnitude - vector.magnitude);
			this.drawingPosition = this.origin + vector / 2f + (this.destination - this.origin) / 2f + Vector3.up * this.def.Altitude;
			this.drawingMatrix.SetTRS(this.drawingPosition, this.ExactRotation, this.drawingScale);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000058E4 File Offset: 0x00003AE4
		public virtual void GetPreFiringDrawingParameters()
		{
			if (this.Props.TickOffset != 0)
			{
				this.drawingIntensity = this.Props.DrawingOffset + (this.Props.DrawingOffsetSecond - this.Props.DrawingOffset) * (float)this.tickCounter / (float)this.Props.TickOffset;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000593C File Offset: 0x00003B3C
		public virtual void GetPostFiringDrawingParameters()
		{
			if (this.Props.TickOffsetSecond != 0)
			{
				this.drawingIntensity = this.Props.DrawingOffsetThird + (this.Props.DrawingOffsetFourth - this.Props.DrawingOffsetThird) * (((float)this.tickCounter - (float)this.Props.TickOffset) / (float)this.Props.TickOffsetSecond);
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000059A4 File Offset: 0x00003BA4
		protected void DetermineImpactExactPosition()
		{
			Vector3 vector = this.destination - this.origin;
			int num = (int)vector.magnitude;
			Vector3 vector2 = vector / vector.magnitude;
			Vector3 destination = this.origin;
			Vector3 vector3 = this.origin;
			IntVec3 intVec = IntVec3Utility.ToIntVec3(vector3);
			for (int i = 1; i <= num; i++)
			{
				vector3 += vector2;
				intVec = IntVec3Utility.ToIntVec3(vector3);
				if (!GenGrid.InBounds(vector3, base.Map))
				{
					this.destination = destination;
					return;
				}
				if (!this.def.projectile.flyOverhead && this.def.projectile.alwaysFreeIntercept && i >= 5)
				{
					List<Thing> list = base.Map.thingGrid.ThingsListAt(base.Position);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (thing.def.Fillage == FillCategory.Full)
						{
							this.destination = intVec.ToVector3Shifted() + new Vector3(Rand.Range(-0.3f, 0.3f), 0f, Rand.Range(-0.3f, 0.3f));
							this.hitThing = thing;
							break;
						}
						if (thing.def.category == ThingCategory.Pawn)
						{
							Pawn pawn = thing as Pawn;
							float num2 = 0.45f;
							if (pawn.Downed)
							{
								num2 *= 0.1f;
							}
							float num3 = GenGeo.MagnitudeHorizontal(this.ExactPosition - this.origin);
							if (num3 < 4f)
							{
								num2 *= 0f;
							}
							else if (num3 < 7f)
							{
								num2 *= 0.5f;
							}
							else if (num3 < 10f)
							{
								num2 *= 0.75f;
							}
							num2 *= pawn.RaceProps.baseBodySize;
							if (Rand.Value < num2)
							{
								this.destination = intVec.ToVector3Shifted() + new Vector3(Rand.Range(-0.3f, 0.3f), 0f, Rand.Range(-0.3f, 0.3f));
								this.hitThing = pawn;
								break;
							}
						}
					}
				}
				destination = vector3;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005BE4 File Offset: 0x00003DE4
		public virtual void Fire()
		{
			this.ApplyDamage(this.hitThing);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005BF2 File Offset: 0x00003DF2
		protected void ApplyDamage(Thing hitThing)
		{
			if (hitThing != null)
			{
				this.Impact(hitThing);
				return;
			}
			this.ImpactSomething();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005C08 File Offset: 0x00003E08
		private void ImpactSomething()
		{
			if (this.def.projectile.flyOverhead)
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
				if (roofDef != null)
				{
					if (roofDef.isThickRoof)
					{
						this.def.projectile.soundHitThickRoof.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
						this.Destroy(DestroyMode.Vanish);
						return;
					}
					if (base.Position.GetEdifice(base.Map) == null || base.Position.GetEdifice(base.Map).def.Fillage != FillCategory.Full)
					{
						RoofCollapserImmediate.DropRoofInCells(base.Position, base.Map, null);
					}
				}
			}
			if (!this.usedTarget.HasThing || !base.CanHit(this.usedTarget.Thing))
			{
				List<Thing> list = new List<Thing>();
				list.Clear();
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && base.CanHit(thing))
					{
						list.Add(thing);
					}
				}
				list.Shuffle<Thing>();
				for (int j = 0; j < list.Count; j++)
				{
					Thing thing2 = list[j];
					Pawn pawn = thing2 as Pawn;
					float num;
					if (pawn != null)
					{
						num = 0.5f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						if (pawn.GetPosture() != PawnPosture.Standing && GenGeo.MagnitudeHorizontalSquared(this.origin - this.destination) >= 20.25f)
						{
							num *= 0.2f;
						}
						if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
						{
							num *= VerbUtility.InterceptChanceFactorFromDistance(this.origin, base.Position);
						}
					}
					else
					{
						num = 1.5f * thing2.def.fillPercent;
					}
					if (Rand.Chance(num))
					{
						this.Impact(list.RandomElement<Thing>());
						return;
					}
				}
				this.Impact(null);
				return;
			}
			Pawn pawn2 = this.usedTarget.Thing as Pawn;
			if (pawn2 != null && pawn2.GetPosture() != PawnPosture.Standing && GenGeo.MagnitudeHorizontalSquared(this.origin - this.destination) >= 20.25f && !Rand.Chance(0.2f))
			{
				this.Impact(null);
				return;
			}
			this.Impact(this.usedTarget.Thing);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005EDC File Offset: 0x000040DC
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			if (hitThing != null)
			{
				int damageAmount = this.def.projectile.GetDamageAmount((float)base.DamageAmount, null);
				ThingDef equipmentDef = this.equipmentDef;
				float armorPenetration = this.def.projectile.GetArmorPenetration(base.ArmorPenetration, null);
				DamageInfo dinfo = new DamageInfo(this.def.projectile.damageDef, (float)damageAmount, armorPenetration, this.ExactRotation.eulerAngles.y, this.launcher, null, equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, null);
				hitThing.TakeDamage(dinfo);
				Pawn pawn = hitThing as Pawn;
				if (pawn != null && !pawn.Downed && Rand.Value < this.compED.chanceToProc)
				{
					MoteMaker.ThrowMicroSparks(this.destination, base.Map);
					hitThing.TakeDamage(new DamageInfo(DefDatabase<DamageDef>.GetNamed(this.compED.damageDef, true), (float)this.compED.damageAmount, armorPenetration, this.ExactRotation.eulerAngles.y, this.launcher, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					return;
				}
			}
			else
			{
				SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(base.Position, map, false));
				MoteMaker.MakeStaticMote(this.ExactPosition, map, ThingDefOf.LaserMoteWorker, 0.5f);
				Projectile_HeatImpact.ThrowMicroSparksRed(this.ExactPosition, base.Map);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006044 File Offset: 0x00004244
		public override void Draw()
		{
			base.Comps_PostDraw();
			Graphics.DrawMesh(MeshPool.plane10, this.drawingMatrix, FadedMaterialPool.FadedVersionOf(this.drawingTexture, this.drawingIntensity), 0);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00006070 File Offset: 0x00004270
		public static void ThrowMicroSparksRed(Vector3 loc, Map map)
		{
			if (!GenView.ShouldSpawnMotesAt(loc, map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			Rand.PushState();
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDef.Named("LaserImpactThing"), null);
			moteThrown.Scale = Rand.Range(1f, 1.2f);
			moteThrown.rotationRate = Rand.Range(-12f, 12f);
			moteThrown.exactPosition = loc;
			moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
			moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
			moteThrown.SetVelocity((float)Rand.Range(35, 45), 1.2f);
			GenSpawn.Spawn(moteThrown, IntVec3Utility.ToIntVec3(loc), map, WipeMode.Vanish);
			Rand.PopState();
		}

		// Token: 0x04000051 RID: 81
		public int tickCounter;

		// Token: 0x04000052 RID: 82
		public Thing hitThing;

		// Token: 0x04000053 RID: 83
		public CompFutureDamage compED;

		// Token: 0x04000054 RID: 84
		public Material preFiringTexture;

		// Token: 0x04000055 RID: 85
		public Material postFiringTexture;

		// Token: 0x04000056 RID: 86
		public Matrix4x4 drawingMatrix;

		// Token: 0x04000057 RID: 87
		public Vector3 drawingScale;

		// Token: 0x04000058 RID: 88
		public Vector3 drawingPosition;

		// Token: 0x04000059 RID: 89
		public float drawingIntensity;

		// Token: 0x0400005A RID: 90
		public Material drawingTexture;
	}
}
