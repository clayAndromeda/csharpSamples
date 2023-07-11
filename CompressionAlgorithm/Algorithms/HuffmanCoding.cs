namespace Clay.Compression.Algorithms;

public static class HuffmanCoding
{
	// (char,int)のペアを格納するBinaryHeapクラス
	internal class HuffmanTree
	{
		public class Node
		{
			public char Character;
			public int Frequency;
			public Node(char c, int f)
			{
				Character = c;
				Frequency = f;
			}
		}

		public Node Right;
		public Node Left;

		private List<Node> _nodes;

		public HuffmanTree(string str)
		{
			// 各文字が出現する回数を数えてListに格納する
			List<Node> baseNodes = str.GroupBy(x => x).Select(x => new Node(x.Key, x.Count())).ToList();

			// 各文字が出現する頻度が少ない順に取り出せる優先つきキューを作る
			var priorityQueue = new PriorityQueue<char, int>(baseNodes.Count);
			foreach (var node in baseNodes)
			{
				priorityQueue.Enqueue(node.Character, node.Frequency);
			}
		}
	}
	
	// 文字列引数をハフマン符号化する
	public static string Encode(string inputStr)
	{
		
		// まずは、各文字の出現回数を数える
		
		// 重複を除いた文字の集合を作る
		var uniqueChars = new HashSet<char>(inputStr);
		
		// ハフマン木をPriorityQueueを使って作る
		// その文字が出現する回数が少ない順に取り出せる
		var priorityQueue = new PriorityQueue<char, int>(uniqueChars.Count);
		foreach (var chr in uniqueChars)
		{
			priorityQueue.Enqueue(chr, inputStr.Count(x => x == chr));
		}
		
		while (priorityQueue.TryDequeue(out char chr, out int count))
		{
			Console.WriteLine($"{chr}: {count}");
		}
		return String.Empty;
	}
}