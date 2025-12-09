TryParse<int> parse = (string text, out int result) => Int32.TryParse(text, out result);

delegate bool TryParse<T>(string text, out T result);
