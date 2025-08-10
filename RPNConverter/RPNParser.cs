using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace RPNConverter
{
    // Main RPN parser - handles conversion from postfix to infix and evaluation
    // TODO: Maybe add support for more operators like ^ (power) in the future?
    public class RPNParser : IRPNParser
    {
        // Basic math operators we support - keeping it simple for now
        // NOTE: Could probably use a Dictionary<char, int> for precedence if we ever need it
        private static readonly HashSet<char> SupportedOperators = new HashSet<char> { '+', '-', '*', '/' };
        
        // Regex to match valid numbers (including decimals and negatives)
        // Had to tweak this pattern a few times to get it right!
        private static readonly Regex NumberPattern = new Regex(@"^-?\d+(\.\d+)?$", RegexOptions.Compiled);
        
        // Main conversion method - turns "3 4 +" into "(3 + 4)"
        public string ConvertToInfix(string rpnExpression)
        {
            if (string.IsNullOrWhiteSpace(rpnExpression))
                throw new ArgumentException("Hey, you need to give me something to work with!", nameof(rpnExpression));
            
            var tokens = TokenizeExpression(rpnExpression);
            var expressionTree = BuildExpressionTree(tokens);
            
            return expressionTree.ToInfix();
        }
        
        // Does the actual math calculation
        public double Evaluate(string rpnExpression)
        {
            if (string.IsNullOrWhiteSpace(rpnExpression))
                throw new ArgumentException("Can't calculate nothing!", nameof(rpnExpression));
            
            var tokens = TokenizeExpression(rpnExpression);
            var expressionTree = BuildExpressionTree(tokens);
            
            return expressionTree.Evaluate();
        }
        
        // Quick check to see if the RPN looks valid before we try to process it
        // Saves us from crashes on obviously bad input
        public bool IsValidRPN(string rpnExpression)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rpnExpression))
                    return false;
                
                var tokens = TokenizeExpression(rpnExpression);
                BuildExpressionTree(tokens);
                return true;
            }
            catch
            {
                // If anything goes wrong, it's probably not valid RPN
                return false;
            }
        }
        
        // Break down the input string into individual pieces (numbers and operators)
        // This was trickier than I thought - had to handle spaces and tabs properly
        private List<Token> TokenizeExpression(string expression)
        {
            var tokens = new List<Token>();
            // Split on spaces and tabs, ignore empty entries
            var parts = expression.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var part in parts)
            {
                // Check if it's one of our supported operators
                if (part.Length == 1 && SupportedOperators.Contains(part[0]))
                {
                    tokens.Add(new Token(part[0]));
                }
                // Check if it looks like a number
                else if (NumberPattern.IsMatch(part))
                {
                    if (double.TryParse(part, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                    {
                        tokens.Add(new Token(part, value));
                    }
                    else
                    {
                        throw new ArgumentException($"Hmm, '{part}' looks like a number but I can't parse it");
                    }
                }
                else
                {
                    throw new ArgumentException($"I don't know what '{part}' is supposed to be");
                }
            }
            
            return tokens;
        }
        
        // This is where the magic happens - build a tree structure from the tokens
        // Uses a stack to keep track of operands and operators (classic RPN algorithm)
        private ExpressionNode BuildExpressionTree(List<Token> tokens)
        {
            if (tokens.Count == 0)
                throw new ArgumentException("Can't build a tree from nothing!");
            
            var stack = new Stack<ExpressionNode>();
            
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Operand)
                {
                    // Numbers just go straight onto the stack
                    stack.Push(new OperandNode(token.NumericValue, token.Value));
                }
                else if (token.Type == TokenType.Operator)
                {
                    if (stack.Count < 2)
                        throw new ArgumentException($"Not enough numbers for '{token.Operator}' - need 2 but only have {stack.Count}");
                    
                    // IMPORTANT: Order matters here! In RPN, the second item popped is the left operand
                    // Took me a while to get this right the first time I implemented RPN
                    var right = stack.Pop();
                    var left = stack.Pop();
                    
                    stack.Push(new OperatorNode(token.Operator, left, right));
                }
            }
            
            // Should have exactly one item left - that's our complete expression
            if (stack.Count != 1)
                throw new ArgumentException($"Something's not right - ended up with {stack.Count} items instead of 1");
            
            return stack.Pop();
        }
    }
}