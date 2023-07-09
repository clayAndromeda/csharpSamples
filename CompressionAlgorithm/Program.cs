using Clay.Compression.Algorithms;
using Clay.Compression.Tests;

// テストを繰り返す回数
const int testCount = 10;

// 平均圧縮率と最低圧縮率、最高圧縮率を記録する
double averageCompressionRate = 0;
double minCompressionRate = Double.MaxValue;
double maxCompressionRate = Double.MinValue;

for (int i = 0; i < testCount; ++i)
{
	var input = TestStringGenerator.GenerateRandomAlphabetString(1000000);
	var encoded = Rle.Encode(input);
	var decoded = Rle.Decode(encoded);
	
	// 圧縮率を計算する
	var compressionRate = (double)encoded.Length / input.Length;

	if (input != decoded)
	{
		// 複合化に失敗したことを通知する
		Console.WriteLine("Failed to decode.");
	}
	
	// 平均圧縮率と最低圧縮率、最高圧縮率を記録する
	averageCompressionRate += compressionRate;
	minCompressionRate = Double.Min(minCompressionRate, compressionRate);
	maxCompressionRate = Double.Max(maxCompressionRate, compressionRate);
}

// 平均圧縮率を計算する
averageCompressionRate /= testCount;

// 圧縮率を表示する
Console.WriteLine($"Average compression rate: {averageCompressionRate}");
Console.WriteLine($"Min compression rate: {minCompressionRate}");
Console.WriteLine($"Max compression rate: {maxCompressionRate}");
