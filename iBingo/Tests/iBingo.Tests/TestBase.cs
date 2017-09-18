namespace iBingo.Tests
{
    using System;
    using Xunit.Abstractions;

    public abstract class TestBase
    {
        private readonly ITestOutputHelper _outputHelper;

        protected TestBase(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        protected void Output(string message) => _outputHelper.WriteLine(message);

        protected void OutputFailed(string message, object actual, object expected)
        {
            Output($"{message}{Environment.NewLine}"
                   + $" [実際の値] <{actual}>{Environment.NewLine}"
                   + $" [期待値]  <{expected}>");
        }

        protected void OutputFailed(string message, Type throwException)
        {
            Output($"{message}{Environment.NewLine}"
                   + $" [スローされた例外型] <{throwException}>");
        }
    }
}