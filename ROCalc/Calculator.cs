
using System;


namespace ROCalc
{
    using Processor = Func<float, float, float>;

    public struct State
    {
        public float? Accumulator { get; private set; }
        public Processor? Processor { get; private set; }
        public string Text { get; private set; }
        public bool HasExecuted { get; private set; }

        public State(float? accumulator = 0, Processor? processor = null, String? text = "", bool? hasExecuted = false)
        {
            Accumulator = accumulator;
            Processor = processor;
            Text = text ?? "";
            HasExecuted = hasExecuted ?? false;
        }

        public State withAccumulator(float? accumulator)
        {
            State returnState = this; /* copy */
            returnState.Accumulator = accumulator;
            return returnState;
        }

        public State withProcessor(Processor? processor)
        {
            State returnState = this; /* copy */
            returnState.Processor = processor;
            return returnState;
        }

        public State withText(string text)
        {
            State returnState = this; /* copy */
            returnState.Text = text;
            return returnState;
        }

        public State withHasExecuted(bool hasExecuted)
        {
            State returnState = this; /* copy */
            returnState.HasExecuted = hasExecuted;
            return returnState;
        }
    }

    public interface IOperation
    {
        public abstract State? Apply(State state);
    }

    public class OperationAppendText : IOperation
    {
        public string Text { get; private set; }

        public OperationAppendText(String text)
        {
            this.Text = text;
        }

        public State? Apply(State state)
        {
            /* remove 0 if that is what is being displayed */
            string prependText = state.Text;
            if ( prependText == "0" )
            {
                prependText = "";
            }
            else if (prependText == "-0")
            {
                prependText = "-";
            }

            return state.withText(prependText + Text);
        }
    }

    public class OperationClear : IOperation
    {
        public State? Apply(State state)
        {
            bool _IsClear(State state)
            {
                bool isTextClear = state.Text == null || state.Text.Length == 0;
                return isTextClear && state.Accumulator == null;
            }
            if (_IsClear(state)) { return null; }
            return new State();
        }
    }

    public class OperationToggleNegative : IOperation
    {
        public State? Apply(State state)
        {
            return state.withText(_Toggle(state.Text));
        }

        public string _Toggle(string text)
        {
            if ( text == null ) { return "-"; }
            if ( text.Length == 0 ) { return "-"; }

            string returnText = text;

            char firstChar = text.ToCharArray()[0];
            if (firstChar == '-')
            {
                returnText = text.Substring(1, text.Length - 1);
            }
            else
            {
                returnText = "-" + text;
            }

            return returnText;
        }
    }

    public class OperationAddDotText : IOperation
    {
        public State? Apply(State state)
        {
            if (state.Text.Contains(".")) { return null; }
            return state.withText(state.Text + ".");
        }
    }

    public class OperationCalcEquals : IOperation
    {
        public State? Apply(State state)
        {
            if (state.Accumulator == null) { return null; }
            if (state.Processor == null) { return null; }

            if (state.Text.Length == 0)
            {
                return state
                    .withText($"{ state.Accumulator }");
            }
            else if ( !state.HasExecuted )
            {
                float userInputValue = float.Parse(state.Text);

                float oldAccumulator = state.Accumulator ?? 0;

                float newValue = state.Processor(oldAccumulator, userInputValue);

                return state
                    .withHasExecuted(true)
                    .withAccumulator(userInputValue)
                    .withText($"{ newValue }");
            }
            else if ( state.HasExecuted )
            {
                float userInputValue = float.Parse(state.Text);

                float oldAccumulator = state.Accumulator ?? 0;

                float newValue = state.Processor(userInputValue, oldAccumulator);

                return state
                    .withText($"{ newValue }");
            }
            else
            {
                return null;
            }
        }
    }

    public abstract class OperationCalc : IOperation
    {
        protected Processor Processor { get; set; } = (a,b) => 0;

        public State? Apply(State state)
        {
            if (state.Text.Length == 0) { return null; }
            State? newState = new OperationCalcEquals().Apply(state);

            if (newState == null)
            {
                newState = new State(
                    accumulator: float.Parse(state.Text),
                    processor: this.Processor,
                    text: "");
            }

            return newState;
        }
    }

    public class OperationCalcAdd : OperationCalc
    {
        public OperationCalcAdd()
        {
            Processor = (x, y) => { return x + y; };
        }
    }

    public class OperationCalcSubtract : OperationCalc
    {
        public OperationCalcSubtract()
        {
            Processor = (x, y) => { return x - y; };
        }
    }

    public class OperationCalcMultiply : OperationCalc
    {
        public OperationCalcMultiply()
        {
            Processor = (x, y) => { return x * y; };
        }
    }

    public class OperationCalcDivide : OperationCalc
    {
        public OperationCalcDivide()
        {
            Processor = (x, y) => { return x / y; };
        }
    }
}
