# RPN Calculator

A simple C# app that converts Reverse Polish Notation (like `3 4 +`) into regular math notation (like `(3 + 4)`) and calculates the result.

## What it does

- Converts RPN to readable math expressions
- Calculates the actual result
- Supports +, -, *, / and decimal numbers
- Handles errors gracefully

## How to run it

1. Open `RPNConverter.sln` in Visual Studio 2022
2. Hit F5 to run
3. Type RPN expressions when prompted
4. Type `quit` to exit

**Note:** You need Visual Studio to build this - it's a .NET Framework project.

## Examples

```
RPN> 3 4 +
Math: (3 + 4)
= 7

RPN> 3 4 + 2 *
Math: ((3 + 4) * 2)
= 14

RPN> 8 2 /
Math: (8 / 2)
= 4
```

## How RPN works

Instead of writing `3 + 4`, you write `3 4 +`. Numbers first, then the operator.

For complex expressions like `(3 + 4) * 2`, you'd write `3 4 + 2 *`.

## That's it!

Pretty straightforward. The code uses expression trees to parse and evaluate the RPN, but you don't need to worry about that unless you're looking at the implementation.