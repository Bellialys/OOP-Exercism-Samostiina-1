using System.Text;

public static class RotationalCipher
{
    public static string Rotate(string text, int shiftKey)
    {
        var result = new StringBuilder();
        foreach (char symbol in text)
        {
            if (symbol >= 'A' && symbol <= 'Z')
                result.Append((char)('A' + (symbol - 'A' + shiftKey) % 26));
            else if (symbol >= 'a' && symbol <= 'z')
                result.Append((char)('a' + (symbol - 'a' + shiftKey) % 26));
            else result.Append(symbol);
        }
        return result.ToString();
    }
}
