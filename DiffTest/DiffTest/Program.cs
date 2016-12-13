using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string text1 = "ABC123";
            string text2 = "?ABO123";

            var result = Diff(
                text1, 0, text1.Length,
                text2, 0, text2.Length).ToArray();
        }

        public enum DiffSectionType
        {
            Copy,
            Insert,
            Delete
        }

        public struct DiffSection
        {
            private readonly DiffSectionType _Type;
            private readonly int _Length;

            public DiffSection(DiffSectionType type, int length)
            {
                _Type = type;
                _Length = length;
            }

            public DiffSectionType Type
            {
                get
                {
                    return _Type;
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
                return string.Format("{0} {1}", Type, Length);
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
                yield return new DiffSection(
                    DiffSectionType.Copy,
                    lcs.Length);

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
                    DiffSectionType.Delete,
                    firstEnd - firstStart);
            }
            if (secondStart < secondEnd)
            {
                // we got content from second collection --> inserted
                yield return new DiffSection(
                    DiffSectionType.Insert,
                    secondEnd - secondStart);
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
                int positionInFirstCollection,
                int positionInSecondCollection,
                int length)
            {
                _Success = true;
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
                        Length);
                else
                    return "LCS (-)";
            }
        }
    }
}
