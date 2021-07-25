using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HediffSpecial
{
    // Token: 0x0200001C RID: 28
    public static class BodypartUtility
    {
        // Token: 0x06000059 RID: 89 RVA: 0x00003F92 File Offset: 0x00002192
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

        // Token: 0x0600005A RID: 90 RVA: 0x00003FB0 File Offset: 0x000021B0
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

        // Token: 0x0600005B RID: 91 RVA: 0x00003FD5 File Offset: 0x000021D5
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

        // Token: 0x0600005C RID: 92 RVA: 0x00004002 File Offset: 0x00002202
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

        // Token: 0x0600005D RID: 93 RVA: 0x00004027 File Offset: 0x00002227
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

        // Token: 0x0600005E RID: 94 RVA: 0x00004048 File Offset: 0x00002248
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
}