namespace iBingo.Tests.Presentations.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using iBingo.Presentations.Models;
    using iBingo.Presentations.Utility;
    using Xunit;
    using Xunit.Abstractions;

    public class ShuffleValuesTests : TestBase
    {
        public ShuffleValuesTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        #region Ctor

        private static ShuffleValues Factory()
        {
            return new ShuffleValues(1, 75, OnShuffleValuesAction);
        }

        private static void OnShuffleValuesAction(int value)
        {

        }

        private static IEnumerable<object[]> GetTest_成功_コンストラクタData()
        {

            yield return new object[]
                         {
                             $"(正常系) 1 ～ 75 までの値を設定した場合、{nameof(ShuffleValues.ShufflingValuesLength)} は 75 となること。",
                             1,
                             75,
                             new Action<int>(OnShuffleValuesAction),
                             Factory()
                         };
            yield return new object[]
                         {
                             $"(正常系) 1 ～ 100 までの値を設定した場合、{nameof(ShuffleValues.ShufflingValues)} に 1 ～ 100 までの値が設定されること。",
                             1,
                             100,
                             new Action<int>(OnShuffleValuesAction),
                             Factory()
                         };

            //yield return new object[]
            //             {
            //                 $"(正常系) "
            //                 , 1
            //                 , 75
            //                 , new Action<int>(OnShuffleValuesAction)
            //                 , Factory()
            //             };
        }

        [Theory]
        [MemberData(nameof(GetTest_成功_コンストラクタData))]
        public void Test_成功_コンストラクタ(string caseName, int minimum, int maximum, Action<int> shufflingAction, ShuffleValues expected)
        {
            // arrange
            ShuffleValues shuffleValues = null;

            // act
            var ex = Record.Exception(() => shuffleValues = new ShuffleValues(minimum, maximum, shufflingAction));

            // assert
            Assert.Null(ex);
            Assert.NotNull(shuffleValues);
            Assert.Equal(expected.MinimumValue, shuffleValues.MinimumValue);
            Assert.Equal(expected.MaximumValue, shuffleValues.MaximumValue);
            Assert.Equal(expected.ShufflingValuesLength, shuffleValues.ShufflingValuesLength);
            Assert.Equal(expected.ShufflingValues, shuffleValues.ShufflingValues);

            Output($"{caseName}{Environment.NewLine}"
                   + $" {nameof(shuffleValues.MinimumValue)}:{shuffleValues.MinimumValue}{Environment.NewLine}"
                   + $" {nameof(shuffleValues.MaximumValue)}:{shuffleValues.MaximumValue}{Environment.NewLine}"
                   + $" {nameof(shuffleValues.ShufflingValuesLength)}:{shuffleValues.ShufflingValuesLength}{Environment.NewLine}"
                   + $" {nameof(shuffleValues.ShufflingValues)}:{string.Join(", ", shuffleValues.ShufflingValues.Select(x => x))}{Environment.NewLine}");
        }

        [Theory]
        [MemberData(nameof(GetTest_失敗_コンストラクタData))]
        public void Test_失敗_コンストラクタ(string caseName, int minimum, int maximum, Action<int> shufflingAction, Type throwException)
        {
            // arrange
            ShuffleValues shuffleValues = null;

            // act
            var ex = Record.Exception(() => shuffleValues = new ShuffleValues(minimum, maximum, shufflingAction));

            // assert
            Assert.IsType(throwException, ex);

            OutputFailed(caseName, throwException);
        }

        private static IEnumerable<object[]> GetTest_失敗_コンストラクタData()
        {
            yield return new object[]
                         {
                             $"(異常系) {nameof(ShuffleValues.MinimumValue)} に 0 未満の値を設定すると{typeof(ArgumentOutOfRangeException)} をスローすること。"
                             , -1
                             , 75
                             , new Action<int>(OnShuffleValuesAction)
                             , typeof(ArgumentOutOfRangeException)
                         };

            yield return new object[]
                         {
                             $"(異常系) {nameof(ShuffleValues.MaximumValue)} に {nameof(ShuffleValues.MinimumValue)} 以下の値を設定すると{typeof(ArgumentOutOfRangeException)} をスローすること。"
                             , 10
                             , 10
                             , new Action<int>(OnShuffleValuesAction)
                             , typeof(ArgumentOutOfRangeException)
                         };

            yield return new object[]
                         {
                             $"(異常系) shufflingAction にnull を設定すると{typeof(ArgumentNullException)} をスローすること。"
                             , 1
                             , 75
                             , null
                             , typeof(ArgumentNullException)
                         };

            //yield return new object[]
            //             {
            //                 $"(異常系) "
            //                 , 1
            //                 , 75
            //                 , new Action<int>(OnShuffleValuesAction)
            //                 , Factory()
            //             };
        }

        #endregion

        #region Shuffle メソッド

        [Fact]
        public void Test_正常_Shuffle_ランダムに指定した値がすべて設定されること()
        {
            // arrange
            var hitNumbers = new List<NumberData>();
            var shuffleValues = new ShuffleValues(1, 75, OnShuffleValuesAction);

            // act
            var ex = Record.ExceptionAsync(async () =>
                                           {
                                               for (int i = 0; i < 75; i++)
                                               {
                                                   var v = shuffleValues.Shuffle(hitNumbers);
                                                   await Task.Delay(TimeSpan.FromMilliseconds(100));
                                                   shuffleValues.Stop();
                                                   hitNumbers.Add(v.Result);
                                               }
                                           });

            // assert
            Assert.Null(ex.Result);
            var results = hitNumbers.OrderBy(x => x.Number);
            var resultValues = results.Select(x => x.Number).ToArray();
            Output($"シャッフルしたすべての値の列挙{Environment.NewLine}"
                   + $"{string.Join(", ", resultValues)}");
            Assert.Equal(75, hitNumbers.Count);
            Assert.True(results.All(x => x.Hit));
            Assert.Equal(Enumerable.Range(1, 75).ToArray(), resultValues);
        }

        [Fact]
        public void Test_正常_ランダムに指定した値がすべて設定されることx50()
        {
            for (int a = 0; a < 50; a++)
            {
                var sw = Stopwatch.StartNew();

                // arrange
                var hitNumbers = new List<NumberData>();
                var shuffleValues = new ShuffleValues(1, 75, OnShuffleValuesAction);

                // act
                var ex = Record.ExceptionAsync(async () =>
                                               {
                                                   for (int i = 0; i < 75; i++)
                                                   {
                                                       var v = shuffleValues.Shuffle(hitNumbers);
                                                       await Task.Delay(TimeSpan.FromMilliseconds(51));
                                                       shuffleValues.Stop();
                                                       hitNumbers.Add(v.Result);
                                                   }
                                               });

                // assert
                Assert.Null(ex.Result);
                var results = hitNumbers.OrderBy(x => x.Number);
                var resultValues = results.Select(x => x.Number).ToArray();
                Assert.Equal(75, hitNumbers.Count);
                Assert.True(results.All(x => x.Hit));
                Assert.Equal(Enumerable.Range(1, 75).ToArray(), resultValues);

                sw.Stop();
                Output($"{a + 1}回目 (経過時間 {sw.Elapsed})");
            }
        }

        #endregion
    }
}