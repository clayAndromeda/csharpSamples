using System.Text;

namespace Clay.Compression.Algorithms;

// Run-Length Encoding (ランレングス符号化）
public static class Rle
{
	// 文字列引数をランレングス符号化する
	public static string Encode(string str)
	{
		var sb = new StringBuilder();
		var count = 1;
		for (var i = 0; i < str.Length; i++)
		{
			if (i + 1 < str.Length && str[i] == str[i + 1])
			{
				count++;
			}
			else
			{
				sb.Append(str[i]);
				sb.Append(count);
				count = 1;
			}
		}
		return sb.ToString();
	}
	
	// ランレングス符号化された文字列を復号する
	public static string Decode(string str)
	{
		var sb = new StringBuilder();

		int i = 0;
		while (i < str.Length)
		{
			var ch = str[i];
			i++;
			var strCount = "";
			while (i < str.Length && char.IsDigit(str[i]))
			{
				strCount += str[i];
				i++;
			}

			int count = int.Parse(strCount);
			for (var j = 0; j < count; j++)
			{
				sb.Append(ch);
			}
		}
		
		return sb.ToString();
	}
}