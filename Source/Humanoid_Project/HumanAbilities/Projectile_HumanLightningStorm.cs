using AbilityUser;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace HumanAbilities;

public class Projectile_HumanLightningStorm : Projectile_Ability
{
    private int age;

    private Mesh boltMesh;

    private int duration;

    private IntVec3 strikeLoc = IntVec3.Invalid;

    private bool thrown;

    protected float LightningBrightness
    {
        get
        {
            if (age <= 3)
            {
                return age / 3f;
            }

            return 1.3f - (age / (float)duration);
        }
    }

    public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
    {
        if (age < 600)
        {
            return;
        }

        base.Destroy(mode);
    }

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

    public override void PostImpactEffects(Thing hitThing)
    {
        if (hitThing is not Pawn pawn)
        {
            return;
        }

        duration = Rand.Range(30, 60);
        ThrowBolt(pawn.Position, pawn);
    }

    public override void Tick()
    {
        base.Tick();
        age++;
    }

    public override void Draw()
    {
        if (boltMesh != null)
        {
            Graphics.DrawMesh(boltMesh, strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Skyfaller),
                Quaternion.identity,
                FadedMaterialPool.FadedVersionOf(
                    (Material)AccessTools.Field(typeof(WeatherEvent_LightningStrike), "LightningMat")
                        .GetValue(null), LightningBrightness), 0);
        }
    }
}