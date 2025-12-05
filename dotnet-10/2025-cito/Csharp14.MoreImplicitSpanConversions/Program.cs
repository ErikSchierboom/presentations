int[] numbers = { 1, 2, 3, 4, 5 };
Span<int> numbersSpan = numbers;
ReadOnlySpan<int> numbersReadOnlySpan = numbers;

string message = "Hello, World!";
ReadOnlySpan<char> messageSpan = message;
