
using System;


namespace ROCalc
{
    using Processor = Func<float, float, float>;

    public struct State
    {
        public float? Accumulator { get; private set; }
        public Processor? Processor { get; private set; }
        public string Text { get; private set; }

        public State(float? accumulator, Processor? processor, String? text)
        {
            Accumulator = accumulator;
            Processor = processor;
            Text = text ?? "";
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
            if (state.Text.Length <= 0) { return null; }
            return state
                .withAccumulator(0)
                .withProcessor(null)
                .withText("0");
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
            if (text.Length == 0) { return ""; }

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
            else
            {
                float userInputValue = float.Parse(state.Text);

                float oldAccumulator = state.Accumulator ?? 0;

                float newValue = state.Processor(oldAccumulator, userInputValue);

                return state
                    .withAccumulator(oldAccumulator)
                    .withText($"{ newValue }");
            }
        }
    }

    public class OperationCalcAdd : IOperation
    {
        public State? Apply(State state)
        {
            if (state.Text.Length == 0) { return null; }
            State? newState = new OperationCalcEquals().Apply(state);

            if ( newState == null )
            {
                newState = new State(
                    accumulator: float.Parse(state.Text),
                    processor: ((x, y) => { return x + y; }),
                    text: "");
            }

            return newState;
        }
    }

    public class OperationCalcSubtract : IOperation
    {
        public State? Apply(State state)
        {
            if (state.Text.Length == 0) { return null; }
            State newState = (new OperationCalcEquals().Apply(state) ?? state);
            return new State(
                accumulator: newState.Accumulator ?? float.Parse(newState.Text),
                processor: ((x, y) => { return x - y; }),
                text: "");
        }
    }

    public class OperationCalcMultiply : IOperation
    {
        public State? Apply(State state)
        {
            if (state.Text.Length == 0) { return null; }
            State newState = (new OperationCalcEquals().Apply(state) ?? state);
            return new State(
                accumulator: newState.Accumulator ?? float.Parse(newState.Text),
                processor: ((x, y) => { return x * y; }),
                text: "");
        }
    }

    public class OperationCalcDivide : IOperation
    {
        public State? Apply(State state)
        {
            if (state.Text.Length == 0) { return null; }
            State newState = (new OperationCalcEquals().Apply(state) ?? state);
            return new State(
                accumulator: newState.Accumulator ?? float.Parse(newState.Text),
                processor: ((x, y) => { return x / y; }),
                text: "");
        }
    }
}