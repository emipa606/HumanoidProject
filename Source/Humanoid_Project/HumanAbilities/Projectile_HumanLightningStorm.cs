﻿using AbilityUser;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace HumanAbilities
{
    // Token: 0x02000007 RID: 7
    public class Projectile_HumanLightningStorm : Projectile_Ability
    {
        // Token: 0x0400000A RID: 10
        private int age;

        // Token: 0x04000008 RID: 8
        private Mesh boltMesh;

        // Token: 0x0400000B RID: 11
        private int duration;

        // Token: 0x04000009 RID: 9
        private IntVec3 strikeLoc = IntVec3.Invalid;

        // Token: 0x0400000C RID: 12
        private bool thrown;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000016 RID: 22 RVA: 0x000028F8 File Offset: 0x00000AF8
        protected float LightningBrightness
        {
            get
            {
                if (age <= 3)
                {
                    return age / 3f;
                }

                return 1.3f - (age / (float) duration);
            }
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000027B0 File Offset: 0x000009B0
        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (age < 600)
            {
                return;
            }

            base.Destroy(mode);
        }

        // Token: 0x06000013 RID: 19 RVA: 0x000027C8 File Offset: 0x000009C8
        public void ThrowBolt(IntVec3 strikeZone, Pawn victim)
        {
            if (thrown)
            {
                return;
            }

            thrown = true;
            strikeLoc = strikeZone;
            SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera();
            boltMesh = LightningBoltMeshPool.RandomBoltMesh;
            GenExplosion.DoExplosion(strikeLoc, victim.Map, 2f, DamageDefOf.RedFire, null);
            var loc = strikeLoc.ToVector3Shifted();
            for (var i = 0; i < 4; i++)
            {
                MoteMaker.ThrowHumanSmoke(loc, victim.Map, 1.5f);
                MoteMaker.ThrowHumanMicroSparks(loc, victim.Map);
                MoteMaker.ThrowHumanLightningGlow(loc, victim.Map, 15.5f);
            }

            var info = SoundInfo.InMap(new TargetInfo(strikeLoc, victim.Map));
            SoundDefOf.Thunder_OnMap.PlayOneShot(info);
        }

        // Token: 0x06000014 RID: 20 RVA: 0x000028AC File Offset: 0x00000AAC
        public override void PostImpactEffects(Thing hitThing)
        {
            if (hitThing == null)
            {
                return;
            }

            if (hitThing is not Pawn pawn)
            {
                return;
            }

            duration = Rand.Range(30, 60);
            ThrowBolt(pawn.Position, pawn);
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000028E2 File Offset: 0x00000AE2
        public override void Tick()
        {
            base.Tick();
            age++;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002928 File Offset: 0x00000B28
        public override void Draw()
        {
            if (boltMesh != null)
            {
                Graphics.DrawMesh(boltMesh, strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Skyfaller),
                    Quaternion.identity,
                    FadedMaterialPool.FadedVersionOf(
                        (Material) AccessTools.Field(typeof(WeatherEvent_LightningStrike), "LightningMat")
                            .GetValue(null), LightningBrightness), 0);
            }
        }
    }
}