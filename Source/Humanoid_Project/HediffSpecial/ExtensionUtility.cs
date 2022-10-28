using Verse;

namespace HediffSpecial;

public static class ExtensionUtility
{
    public static T TryGetModExtension<T>(this Def def) where T : DefModExtension
    {
        var result = def.HasModExtension<T>() ? def.GetModExtension<T>() : default;

        return result;
    }
}