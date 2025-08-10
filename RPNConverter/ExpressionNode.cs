using System;

namespace RPNConverter
{
    // Base class for all nodes in our expression tree
    // Each node knows how to convert itself to readable math and calculate its value
    public abstract class ExpressionNode
    {
        public abstract string ToInfix();
        public abstract double Evaluate();
    }
    
    // Simple number node - just holds a value
    public class OperandNode : ExpressionNode
    {
        private readonly double _value;
        private readonly string _originalText;
        
        public OperandNode(double value, string originalText)
        {
            _value = value;
            _originalText = originalText;
        }
        
        // Just return the original text - keeps formatting like "3.14" instead of "3.1400000"
        public override string ToInfix()
        {
            return _originalText;
        }
        
        // Numbers evaluate to themselves (obviously!)
        public override double Evaluate()
        {
            return _value;
        }
    }
    
    // Operator node - holds an operation and its two operands
    // This is where the actual math happens
    public class OperatorNode : ExpressionNode
    {
        private readonly char _operator;
        private readonly ExpressionNode _left;
        private readonly ExpressionNode _right;
        
        public OperatorNode(char operatorChar, ExpressionNode left, ExpressionNode right)
        {
            _operator = operatorChar;
            _left = left ?? throw new ArgumentNullException(nameof(left));
            _right = right ?? throw new ArgumentNullException(nameof(right));
        }
        
        // Wrap everything in parentheses to be safe - better too many than wrong precedence!
        public override string ToInfix()
        {
            return $"({_left.ToInfix()} {_operator} {_right.ToInfix()})";
        }
        
        // Do the actual calculation based on the operator
        public override double Evaluate()
        {
            double leftValue = _left.Evaluate();
            double rightValue = _right.Evaluate();
            
            // Good old switch statement - simple and reliable
            switch (_operator)
            {
                case '+':
                    return leftValue + rightValue;
                case '-':
                    return leftValue - rightValue;
                case '*':
                    return leftValue * rightValue;
                case '/':
                    if (rightValue == 0)
                        throw new DivideByZeroException("Oops! Can't divide by zero");
                    return leftValue / rightValue;
                default:
                    // This should never happen if our tokenizer is working right
                    throw new InvalidOperationException($"Hmm, I don't know how to handle '{_operator}'");
            }
        }
    }
}