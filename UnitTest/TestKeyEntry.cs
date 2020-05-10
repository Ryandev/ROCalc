using System;
using Xunit;

using ROCalc;


namespace UnitTest
{
    public class TestKeyEntry
    {
        [Fact]
        public void TestAdd()
        {
            ViewModel model = new ViewModel();

            model.KeyCodeEnterred(System.Windows.Input.Key.D3);
            Assert.Equal("3", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Add);
            Assert.Equal("", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.D2);
            Assert.Equal("2", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Return);
            Assert.Equal("5", model.Text);
        }

        [Fact]
        public void TestSubtract()
        {
            ViewModel model = new ViewModel();

            model.KeyCodeEnterred(System.Windows.Input.Key.D3);
            Assert.Equal("3", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Subtract);
            Assert.Equal("", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.D2);
            Assert.Equal("2", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Return);
            Assert.Equal("1", model.Text);
        }

        [Fact]
        public void TestClear()
        {
            ViewModel viewModel = new ViewModel();

            void performSubtractTestOnModel(ViewModel model)
            {
                model.KeyCodeEnterred(System.Windows.Input.Key.D2);
                Assert.Equal("2", model.Text);

                model.KeyCodeEnterred(System.Windows.Input.Key.D0);
                Assert.Equal("20", model.Text);

                model.KeyCodeEnterred(System.Windows.Input.Key.D2);
                Assert.Equal("202", model.Text);

                model.KeyCodeEnterred(System.Windows.Input.Key.D0);
                Assert.Equal("2020", model.Text);

                model.KeyCodeEnterred(System.Windows.Input.Key.Subtract);
                Assert.Equal("", model.Text);

                model.KeyCodeEnterred(System.Windows.Input.Key.D1);
                Assert.Equal("1", model.Text);

                model.KeyCodeEnterred(System.Windows.Input.Key.D9);
                Assert.Equal("19", model.Text);

                model.KeyCodeEnterred(System.Windows.Input.Key.D8);
                Assert.Equal("198", model.Text);

                model.KeyCodeEnterred(System.Windows.Input.Key.D8);
                Assert.Equal("1988", model.Text);

                model.KeyCodeEnterred(System.Windows.Input.Key.Return);
                Assert.Equal("32", model.Text);
            }

            performSubtractTestOnModel(viewModel);

            viewModel.AddOperation(new OperationClear());
            Assert.Equal("", viewModel.Text);
            Assert.Null(viewModel.CurrentState.Processor);
            Assert.Null(viewModel.CurrentState.Accumulator);

            performSubtractTestOnModel(viewModel);

            viewModel.AddOperation(new OperationClear());
            Assert.Equal("", viewModel.Text);
            Assert.Null(viewModel.CurrentState.Processor);
            Assert.Null(viewModel.CurrentState.Accumulator);
        }

        [Fact]
        public void TestMultiply()
        {
            ViewModel model = new ViewModel();

            model.KeyCodeEnterred(System.Windows.Input.Key.D3);
            Assert.Equal("3", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Multiply);
            Assert.Equal("", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.D2);
            Assert.Equal("2", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Return);
            Assert.Equal("6", model.Text);
        }

        [Fact]
        public void TestDivide()
        {
            ViewModel model = new ViewModel();

            model.KeyCodeEnterred(System.Windows.Input.Key.D3);
            Assert.Equal("3", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Divide);
            Assert.Equal("", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.D2);
            Assert.Equal("2", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Return);
            Assert.Equal("1.5", model.Text);
        }

        [Fact]
        public void TestEqualsRepeat()
        {
            ViewModel model = new ViewModel();

            model.KeyCodeEnterred(System.Windows.Input.Key.D9);
            Assert.Equal("9", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Subtract);
            Assert.Equal("", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.D3);
            Assert.Equal("3", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Return);
            Assert.Equal("6", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Return);
            Assert.Equal("3", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Return);
            Assert.Equal("0", model.Text);

            model.KeyCodeEnterred(System.Windows.Input.Key.Return);
            Assert.Equal("-3", model.Text);
        }
    }
}
