using System;

namespace RPNConverter
{
    /// <summary>
    /// Interface for parsing Reverse Polish Notation (RPN) expressions
    /// </summary>
    public interface IRPNParser
    {
        /// <summary>
        /// Converts an RPN expression to infix notation
        /// </summary>
        /// <param name="rpnExpression">The RPN expression as a string</param>
        /// <returns>The equivalent infix expression with proper grouping</returns>
        /// <exception cref="ArgumentException">Thrown when the RPN expression is invalid</exception>
        string ConvertToInfix(string rpnExpression);
        
        /// <summary>
        /// Evaluates an RPN expression and returns the result
        /// </summary>
        /// <param name="rpnExpression">The RPN expression as a string</param>
        /// <returns>The calculated result</returns>
        /// <exception cref="ArgumentException">Thrown when the RPN expression is invalid</exception>
        double Evaluate(string rpnExpression);
        
        /// <summary>
        /// Validates if an RPN expression is syntactically correct
        /// </summary>
        /// <param name="rpnExpression">The RPN expression to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        bool IsValidRPN(string rpnExpression);
    }
}