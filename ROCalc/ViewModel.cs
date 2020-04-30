
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ROCalc
{
    public class ViewModel : INotifyPropertyChanged
    {
        ObservableCollection<State> States { get; set; }
        public String Text => CurrentState.Text ?? "";

        static Dictionary<System.Windows.Input.Key, Func<IOperation>> _KeyMapping;

        static ViewModel()
        {
            _KeyMapping = new Dictionary<System.Windows.Input.Key, Func<IOperation>>()
            {
                { System.Windows.Input.Key.Add, () => new OperationCalcAdd() },
                { System.Windows.Input.Key.Subtract, () => new OperationCalcSubtract() },
                { System.Windows.Input.Key.Divide, () => new OperationCalcDivide() },
                { System.Windows.Input.Key.Multiply, () => new OperationCalcMultiply() },
                { System.Windows.Input.Key.Decimal, () => new OperationAddDotText() },
                { System.Windows.Input.Key.Back, () => new OperationClear() },
                { System.Windows.Input.Key.Return, () => new OperationCalcEquals() },

                { System.Windows.Input.Key.D0, () => new OperationAppendText("0") },
                { System.Windows.Input.Key.D1, () => new OperationAppendText("1") },
                { System.Windows.Input.Key.D2, () => new OperationAppendText("2") },
                { System.Windows.Input.Key.D3, () => new OperationAppendText("3") },
                { System.Windows.Input.Key.D4, () => new OperationAppendText("4") },
                { System.Windows.Input.Key.D5, () => new OperationAppendText("5") },
                { System.Windows.Input.Key.D6, () => new OperationAppendText("6") },
                { System.Windows.Input.Key.D7, () => new OperationAppendText("7") },
                { System.Windows.Input.Key.D8, () => new OperationAppendText("8") },
                { System.Windows.Input.Key.D9, () => new OperationAppendText("9") },

                { System.Windows.Input.Key.NumPad0, () => new OperationAppendText("0") },
                { System.Windows.Input.Key.NumPad1, () => new OperationAppendText("1") },
                { System.Windows.Input.Key.NumPad2, () => new OperationAppendText("2") },
                { System.Windows.Input.Key.NumPad3, () => new OperationAppendText("3") },
                { System.Windows.Input.Key.NumPad4, () => new OperationAppendText("4") },
                { System.Windows.Input.Key.NumPad5, () => new OperationAppendText("5") },
                { System.Windows.Input.Key.NumPad6, () => new OperationAppendText("6") },
                { System.Windows.Input.Key.NumPad7, () => new OperationAppendText("7") },
                { System.Windows.Input.Key.NumPad8, () => new OperationAppendText("8") },
                { System.Windows.Input.Key.NumPad9, () => new OperationAppendText("9") },
            };
        }

        public ViewModel()
        {
            States = new ObservableCollection<State>();

            States.Add(new State(0, null, "0"));

            States.CollectionChanged += (s, e) => OnPropertyChanged("Text");
        }

        public void KeyCodeEnterred(System.Windows.Input.Key key)
        {
            IOperation? newOperation = _KeyMapping.ContainsKey(key) ? _KeyMapping[key]() : null;

            if (newOperation != null)
            {
                AddOperation(newOperation);
            }
        }

        public void AddOperation(IOperation op)
        {
            State? newState = op.Apply(CurrentState);
            if (newState == null) { return; }

            States.Add(newState.Value);
        }

        public void RemoveLastOperation()
        {
            if (States.Count <= 1) { return; }

            States.RemoveAt(States.Count - 1);
        }

        public State CurrentState => States.LastOrDefault();

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if ( propertyName == null ) { return; }
            if ( PropertyChanged == null ) { return; }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
