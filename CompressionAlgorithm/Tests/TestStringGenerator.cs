using System.Text;

namespace Clay.Compression.Tests;

public static class TestStringGenerator
{
	public static string GenerateRandomAlphabetString(int length)
	{
		Random random = new();
		StringBuilder sb = new(length);
		for (int i = 0; i < length; ++i)
		{
			char ch = (char)random.Next(85, 91);
			sb.Append(ch);
		}

		return sb.ToString();
	}
}