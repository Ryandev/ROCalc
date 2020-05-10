using System;
using Xunit;

using ROCalc;


namespace UnitTest
{
    public class TestOperations
    {
        [Fact]
        public void TestAppendText()
        {
            State? state = new State(0, null, "");

            Assert.NotNull(state);
            Assert.Equal(0, state?.Text.Length);

            IOperation op = new OperationAppendText("-");

            state = op.Apply(state.Value);

            Assert.Equal(1, state?.Text.Length);
            Assert.Equal("-", state?.Text);
        }

        [Fact]
        public void TestAdd()
        {
            State? state = new State(null, null, "");
            Assert.NotNull(state);

            state = new OperationAppendText("9").Apply(state.Value);
            Assert.NotNull(state);
            Assert.Null(state?.Accumulator);
            
            state = new OperationCalcAdd().Apply(state.Value);
            Assert.NotNull(state);
            Assert.Equal(9, state?.Accumulator);

            state = new OperationAppendText("9").Apply(state.Value);
            Assert.NotNull(state);
            Assert.Equal(9, state?.Accumulator);

            state = new OperationCalcAdd().Apply(state.Value);
            Assert.NotNull(state);
            Assert.Equal("18", state?.Text);

            state = new OperationCalcAdd().Apply(state.Value);
            Assert.NotNull(state);
            Assert.Equal("27", state?.Text);
        }

        [Fact]
        public void TestEquals()
        {
            State? state = new State(null, null, "");
            Assert.NotNull(state);

            state = new OperationAppendText("9").Apply(state.Value);
            Assert.NotNull(state);
            Assert.Null(state?.Accumulator);

            state = new OperationCalcAdd().Apply(state.Value);
            Assert.NotNull(state);
            Assert.Equal(9, state?.Accumulator);

            state = new OperationAppendText("9").Apply(state.Value);
            Assert.NotNull(state);
            Assert.Equal("9", state?.Text);

            state = new OperationCalcEquals().Apply(state.Value);
            Assert.NotNull(state);
            Assert.Equal("18", state?.Text);

            state = new OperationCalcEquals().Apply(state.Value);
            Assert.NotNull(state);
            Assert.Equal("27", state?.Text);

            state = new OperationCalcEquals().Apply(state.Value);
            Assert.NotNull(state);
            Assert.Equal("36", state?.Text);

            state = new OperationCalcEquals().Apply(state.Value);
            Assert.NotNull(state);
            Assert.Equal("45", state?.Text);
        }
    }
}
