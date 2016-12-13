using System.Collections.Generic;
using System.Diagnostics;

namespace DiffTest
{
    static class StringExt
    {
        public static IEnumerable<string> Align(this string text1, string text2, bool untilMax = true)
        {
            var result = Diff(text1, 0, text1.Length,
                              text2, 0, text2.Length);

            int offset = 0;
            int start1 = 0;
            int start2 = 0;
            int maxCopyLength = 0;
            foreach (var d in result)
            {
                switch (d.Type)
                {
                    case DiffSectionType.Copy:
                    {
                        start1 += d.Length;
                        start2 += d.Length;

                        bool go = !untilMax;

                        if (untilMax)
                        {
                            if (d.Length > maxCopyLength)
                            {
                                maxCopyLength = d.Length;
                                go = true;
                            }
                        }

                        if(go)
                        {
                            offset = start1 - start2;

                            Debug.WriteLine("-> LCS: " + d.ToString(text1));

                            if (offset > 0)
                            {
                                yield return text2.PadLeft(text2.Length + offset);
                            }
                            else
                            {
                                yield return text2.Remove(0, -offset);
                            }
                        }
                    }
                    break;

                    case DiffSectionType.Insert:
                    start2 += d.Length;
                    break;

                    case DiffSectionType.Delete:
                    start1 += d.Length;
                    break;
                }
            }
        }

        public enum DiffSectionType
        {
            Copy,
            Insert,
            Delete
        }

        public struct DiffSection
        {
            public readonly DiffSectionType Type;
            public readonly LongestCommonSubstringResult LCS;

            public DiffSection(DiffSectionType type, LongestCommonSubstringResult lcs)
            {
                Type = type;
                LCS = lcs;
            }

            public int Length
            {
                get
                {
                    return LCS.Length;
                }
            }

            public override string ToString()
            {
                return string.Format("{0} {1}", Type, Length);
            }

            public string ToString(string firstStr)
            {
                var s = LCS.Success ? firstStr.Substring(LCS.PositionInFirstCollection, LCS.Length) : "_";
                return string.Format("{0} {1}, [{2}]", Type, Length, s);
            }
        }

        private static IEnumerable<DiffSection> Diff(
            string firstCollection, int firstStart, int firstEnd,
            string secondCollection, int secondStart, int secondEnd
            )
        {
            var lcs = FindLongestCommonSubstring(
                firstCollection, firstStart, firstEnd,
                secondCollection, secondStart, secondEnd,
                EqualityComparer<char>.Default);

            if (lcs.Success)
            {
                // deal with the section before
                var sectionsBefore = Diff(
                    firstCollection, firstStart, lcs.PositionInFirstCollection,
                    secondCollection, secondStart, lcs.PositionInSecondCollection
                    );
                foreach (var section in sectionsBefore)
                    yield return section;

                // output the copy operation
                yield return new DiffSection(DiffSectionType.Copy, lcs);

                // deal with the section after
                var sectionsAfter = Diff(
                    firstCollection, lcs.PositionInFirstCollection + lcs.Length, firstEnd,
                    secondCollection, lcs.PositionInSecondCollection + lcs.Length, secondEnd
                    );
                foreach (var section in sectionsAfter)
                    yield return section;

                yield break;
            }

            // if we get here, no LCS
            if (firstStart < firstEnd)
            {
                // we got content from first collection --> deleted
                yield return new DiffSection(
                    DiffSectionType.Delete, new LongestCommonSubstringResult(false, firstStart, firstEnd, firstEnd - firstStart));
            }
            if (secondStart < secondEnd)
            {
                // we got content from second collection --> inserted
                yield return new DiffSection(
                    DiffSectionType.Insert, new LongestCommonSubstringResult(false, secondStart, secondEnd, secondEnd - secondStart));
            }
        }

        ///////////////////////////////////////////////////////////////
        // rest is from part 2, modified for recursive sections

        public static LongestCommonSubstringResult FindLongestCommonSubstring(
            string firstCollection, int firstStart, int firstEnd,
            string secondCollection, int secondStart, int secondEnd,
            IEqualityComparer<char> equalityComparer)
        {
            // default result, if we can't find anything
            var result = new LongestCommonSubstringResult();

            for (int index1 = firstStart; index1 < firstEnd; index1++)
            {
                for (int index2 = secondStart; index2 < secondEnd; index2++)
                {
                    if (equalityComparer.Equals(
                        firstCollection[index1],
                        secondCollection[index2]))
                    {
                        int length = CountEqual(
                            firstCollection, index1, firstEnd,
                            secondCollection, index2, secondEnd,
                            equalityComparer);

                        // Is longer than what we already have --> record new LCS
                        if (length > result.Length)
                        {
                            result = new LongestCommonSubstringResult(
                                true,
                                index1,
                                index2,
                                length);
                        }
                    }
                }
            }

            return result;
        }

        public static int CountEqual(
            string firstCollection, int firstPosition, int firstEnd,
            string secondCollection, int secondPosition, int secondEnd,
            IEqualityComparer<char> equalityComparer)
        {
            int length = 0;
            while (firstPosition < firstEnd
                && secondPosition < secondEnd)
            {
                if (!equalityComparer.Equals(
                    firstCollection[firstPosition],
                    secondCollection[secondPosition]))
                {
                    break;
                }

                firstPosition++;
                secondPosition++;
                length++;
            }
            return length;
        }

        ///////////////////////////////////////////////////////////////
        // rest is from part 2, unchanged

        public struct LongestCommonSubstringResult
        {
            private readonly bool _Success;
            private readonly int _PositionInFirstCollection;
            private readonly int _PositionInSecondCollection;
            private readonly int _Length;

            public LongestCommonSubstringResult(
                bool success,
                int positionInFirstCollection,
                int positionInSecondCollection,
                int length)
            {
                _Success = success;
                _PositionInFirstCollection = positionInFirstCollection;
                _PositionInSecondCollection = positionInSecondCollection;
                _Length = length;
            }

            public bool Success
            {
                get
                {
                    return _Success;
                }
            }

            public int PositionInFirstCollection
            {
                get
                {
                    return _PositionInFirstCollection;
                }
            }

            public int PositionInSecondCollection
            {
                get
                {
                    return _PositionInSecondCollection;
                }
            }

            public int Length
            {
                get
                {
                    return _Length;
                }
            }

            public override string ToString()
            {
                if (Success)
                    return string.Format(
                        "LCS ({0}, {1} x{2})",
                        PositionInFirstCollection,
                        PositionInSecondCollection,
                        Length
                        );
                else
                    return "LCS (-)";
            }
        }
    }
}
