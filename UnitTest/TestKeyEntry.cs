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

    }
}
