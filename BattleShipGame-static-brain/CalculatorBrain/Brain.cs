using System;

namespace CalculatorBrain
{
    public class Brain
    {
        public Double CurrentValue { get; private set; } = 0;

        public double Add(double value) 
        {
            CurrentValue += value;
            return CurrentValue;
        }

        public void Clear() => CurrentValue = 0;
        public void SetValue(double value) => CurrentValue = value;

        public double ApplyCustomFunction(Func<double, double, double> funcToApply, double value)
        {
            CurrentValue = funcToApply(CurrentValue, value);
            return CurrentValue;
        }

        public override string ToString() => CurrentValue.ToString();

    }
}