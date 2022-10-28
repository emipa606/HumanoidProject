using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HediffSpecial;

public static class BodypartUtility
{
    public static IEnumerable<BodyPartRecord> GetFirstMatchingBodyparts(this Pawn pawn, BodyPartRecord startingPart,
        HediffDef hediffDef)
    {
        var hediffs = pawn.health.hediffSet.hediffs;
        var currentSet = new List<BodyPartRecord>();
        var nextSet = new List<BodyPartRecord>
        {
            startingPart
        };
        do
        {
            currentSet.AddRange(nextSet);
            nextSet.Clear();
            foreach (var part in currentSet)
            {
                var matchingPart = false;
                int num;
                for (var i = hediffs.Count - 1; i >= 0; i = num - 1)
                {
                    var hediff = hediffs[i];
                    if (hediff.Part == part && hediff.def == hediffDef)
                    {
                        matchingPart = true;
                        yield return part;
                    }

                    num = i;
                }

                if (matchingPart)
                {
                    continue;
                }

                for (var j = 0; j < part.parts.Count; j = num + 1)
                {
                    nextSet.Add(part.parts[j]);
                    num = j;
                }
            }

            currentSet.Clear();
        } while (nextSet.Count > 0);
    }

    public static IEnumerable<BodyPartRecord> GetFirstMatchingBodyparts(this Pawn pawn, BodyPartRecord startingPart,
        HediffDef hediffDef, HediffDef hediffExceptionDef)
    {
        var hediffs = pawn.health.hediffSet.hediffs;
        var currentSet = new List<BodyPartRecord>();
        var nextSet = new List<BodyPartRecord>
        {
            startingPart
        };
        do
        {
            currentSet.AddRange(nextSet);
            nextSet.Clear();
            foreach (var part in currentSet)
            {
                var matchingPart = false;
                int num;
                for (var i = hediffs.Count - 1; i >= 0; i = num - 1)
                {
                    var hediff = hediffs[i];
                    if (hediff.Part == part)
                    {
                        if (hediff.def == hediffExceptionDef)
                        {
                            matchingPart = true;
                            break;
                        }

                        if (hediff.def == hediffDef)
                        {
                            matchingPart = true;
                            yield return part;
                            break;
                        }
                    }

                    num = i;
                }

                if (matchingPart)
                {
                    continue;
                }

                for (var j = 0; j < part.parts.Count; j = num + 1)
                {
                    nextSet.Add(part.parts[j]);
                    num = j;
                }
            }

            currentSet.Clear();
        } while (nextSet.Count > 0);
    }

    public static IEnumerable<BodyPartRecord> GetFirstMatchingBodyparts(this Pawn pawn, BodyPartRecord startingPart,
        HediffDef hediffDef, HediffDef hediffExceptionDef, Predicate<Hediff> extraExceptionPredicate)
    {
        var hediffs = pawn.health.hediffSet.hediffs;
        var currentSet = new List<BodyPartRecord>();
        var nextSet = new List<BodyPartRecord>
        {
            startingPart
        };
        do
        {
            currentSet.AddRange(nextSet);
            nextSet.Clear();
            foreach (var part in currentSet)
            {
                var matchingPart = false;
                int num;
                for (var i = hediffs.Count - 1; i >= 0; i = num - 1)
                {
                    var hediff = hediffs[i];
                    if (hediff.Part == part)
                    {
                        if (hediff.def == hediffExceptionDef || extraExceptionPredicate(hediff))
                        {
                            matchingPart = true;
                            break;
                        }

                        if (hediff.def == hediffDef)
                        {
                            matchingPart = true;
                            yield return part;
                            break;
                        }
                    }

                    num = i;
                }

                if (matchingPart)
                {
                    continue;
                }

                for (var j = 0; j < part.parts.Count; j = num + 1)
                {
                    nextSet.Add(part.parts[j]);
                    num = j;
                }
            }

            currentSet.Clear();
        } while (nextSet.Count > 0);
    }

    public static IEnumerable<BodyPartRecord> GetFirstMatchingBodyparts(this Pawn pawn, BodyPartRecord startingPart,
        HediffDef hediffDef, HediffDef[] hediffExceptionDefs)
    {
        var hediffs = pawn.health.hediffSet.hediffs;
        var currentSet = new List<BodyPartRecord>();
        var nextSet = new List<BodyPartRecord>
        {
            startingPart
        };
        do
        {
            currentSet.AddRange(nextSet);
            nextSet.Clear();
            foreach (var part in currentSet)
            {
                var matchingPart = false;
                int num;
                for (var i = hediffs.Count - 1; i >= 0; i = num - 1)
                {
                    var hediff = hediffs[i];
                    if (hediff.Part == part)
                    {
                        if (hediffExceptionDefs.Contains(hediff.def))
                        {
                            matchingPart = true;
                            break;
                        }

                        if (hediff.def == hediffDef)
                        {
                            matchingPart = true;
                            yield return part;
                            break;
                        }
                    }

                    num = i;
                }

                if (matchingPart)
                {
                    continue;
                }

                for (var j = 0; j < part.parts.Count; j = num + 1)
                {
                    nextSet.Add(part.parts[j]);
                    num = j;
                }
            }

            currentSet.Clear();
        } while (nextSet.Count > 0);
    }

    public static IEnumerable<BodyPartRecord> GetFirstMatchingBodyparts(this Pawn pawn, BodyPartRecord startingPart,
        HediffDef[] hediffDefs)
    {
        var hediffs = pawn.health.hediffSet.hediffs;
        var currentSet = new List<BodyPartRecord>();
        var nextSet = new List<BodyPartRecord>
        {
            startingPart
        };
        do
        {
            currentSet.AddRange(nextSet);
            nextSet.Clear();
            foreach (var part in currentSet)
            {
                var matchingPart = false;
                int num;
                for (var i = hediffs.Count - 1; i >= 0; i = num - 1)
                {
                    var hediff = hediffs[i];
                    if (hediff.Part == part && hediffDefs.Contains(hediff.def))
                    {
                        matchingPart = true;
                        yield return part;
                        break;
                    }

                    num = i;
                }

                if (matchingPart)
                {
                    continue;
                }

                for (var j = 0; j < part.parts.Count; j = num + 1)
                {
                    nextSet.Add(part.parts[j]);
                    num = j;
                }
            }

            currentSet.Clear();
        } while (nextSet.Count > 0);
    }

    public static void ReplaceHediffFromBodypart(this Pawn pawn, BodyPartRecord startingPart, HediffDef hediffDef,
        HediffDef replaceWithDef)
    {
        var hediffs = pawn.health.hediffSet.hediffs;
        var list = new List<BodyPartRecord>();
        var list2 = new List<BodyPartRecord>
        {
            startingPart
        };
        do
        {
            list.AddRange(list2);
            list2.Clear();
            foreach (var bodyPartRecord in list)
            {
                for (var i = hediffs.Count - 1; i >= 0; i--)
                {
                    var hediff = hediffs[i];
                    if (hediff.Part != bodyPartRecord || hediff.def != hediffDef)
                    {
                        continue;
                    }

                    var hediff2 = hediffs[i];
                    hediffs.RemoveAt(i);
                    hediff2.PostRemoved();
                    var item = HediffMaker.MakeHediff(replaceWithDef, pawn, bodyPartRecord);
                    hediffs.Insert(i, item);
                }

                foreach (var partRecord in bodyPartRecord.parts)
                {
                    list2.Add(partRecord);
                }
            }

            list.Clear();
        } while (list2.Count > 0);
    }
}