using System;

namespace RPNConverter
{
    // Simple container for a piece of the RPN expression
    // Could be a number (operand) or a math symbol (operator)
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public double NumericValue { get; }  // Only used for numbers
        public char Operator { get; }        // Only used for operators
        
        // Constructor for numbers
        public Token(string value, double numericValue)
        {
            Type = TokenType.Operand;
            Value = value;
            NumericValue = numericValue;
            Operator = '\0';  // Null character for "not an operator"
        }
        
        // Constructor for operators
        public Token(char operatorChar)
        {
            Type = TokenType.Operator;
            Value = operatorChar.ToString();
            NumericValue = 0;  // Meaningless for operators
            Operator = operatorChar;
        }
        
        public override string ToString()
        {
            return Value;
        }
    }
    
    // Just two types of tokens - keeps it simple
    public enum TokenType
    {
        Operand,   // Numbers
        Operator   // +, -, *, /
    }
}