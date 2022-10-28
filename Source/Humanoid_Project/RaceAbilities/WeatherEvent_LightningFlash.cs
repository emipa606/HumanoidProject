using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RaceAbilities;

public class WeatherEvent_LightningFlash : WeatherEvent
{
    private const int FlashFadeInTicks = 5;

    private const int MinFlashDuration = 15;

    private const int MaxFlashDuration = 30;

    private const float FlashShadowDistance = 25f;

    private static readonly SkyColorSet LightningFlashColors = new SkyColorSet(new Color(0.9f, 0.95f, 1f),
        new Color(0.784313738f, 0.8235294f, 0.847058833f), new Color(0.9f, 0.95f, 1f), 1.15f);

    private readonly int duration;

    private readonly Vector2 shadowVector;

    private int age;

    public WeatherEvent_LightningFlash(Map map) : base(map)
    {
        duration = Rand.Range(15, 60);
        shadowVector = new Vector2(Rand.Range(-5f, 5f), Rand.Range(-5f, 0f));
    }

    public override bool Expired => age > duration;

    public override SkyTarget SkyTarget => new SkyTarget(1f, LightningFlashColors, 1f, 1f);

    public override Vector2? OverrideShadowVector => shadowVector;

    public override float SkyTargetLerpFactor => LightningBrightness;

    protected float LightningBrightness
    {
        get
        {
            if (age <= 3)
            {
                return age / 3f;
            }

            return 1f - (age / (float)duration);
        }
    }

    public override void FireEvent()
    {
        SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(map);
    }

    public override void WeatherEventTick()
    {
        age++;
    }
}