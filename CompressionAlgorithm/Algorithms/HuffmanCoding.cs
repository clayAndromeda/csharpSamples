using System.Collections;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace Clay.Compression.Algorithms;

public static class HuffmanCoding
{
	// (char,int)のペアを格納するBinaryHeapクラス
	internal class HuffmanTree
	{
		private class Node
		{
			public char Character;
			public int Frequency;
			
			public Node? Left;
			public Node? Right;

			public List<bool> Bits;
			
			public Node(char chr, int freq, Node? left = null, Node? right = null)
			{
				Character = chr;
				Frequency = freq;
				Left = left;
				Right = right;
				Bits = new List<bool>();
			}

			public void SetBits(List<bool> bits)
			{
				Console.WriteLine($"SetBits: {Character}, {Frequency:D2}, {string.Join("", bits.Select(x => x ? 1 : 0))}");
				Bits = bits;
			}
			
			public override string ToString()
			{
				return $"{Character}: {Frequency:D2}, {Left?.Character}, {Right?.Character}, ({string.Join("", Bits.Select(x => x ? 1 : 0))})";
			}
		}

		private List<Node> _nodes;
		private Dictionary<char, List<bool>> _bitDictionary = new();

		public HuffmanTree(string str)
		{
			// 各文字が出現する回数を数えてListに格納する
			_nodes = str.GroupBy(x => x).Select(x => new Node(x.Key, x.Count())).ToList();

			// 各文字が出現する頻度が少ない順に取り出せる優先つきキューを作る
			var priorityQueue = new PriorityQueue<Node, int>(_nodes.Count);
			foreach (var node in _nodes)
			{
				priorityQueue.Enqueue(node, node.Frequency);
			}
			
			while (priorityQueue.Count > 1)
			{
				// 出現頻度が少ない2つの文字を取り出す
				var rightNode = priorityQueue.Dequeue();
				var leftNode = priorityQueue.Dequeue();
				
				// それらを子とするノードを作る（左 > 右の関係の二分木）
				var parentNode = new Node('0', leftNode.Frequency + rightNode.Frequency, leftNode, rightNode);
				
				// TODO: ここで、作ったノードと元のノードの優先度が同じだと、次つなぐ順番が狂ってしまいハフマン木が壊れる
				priorityQueue.Enqueue(parentNode, parentNode.Frequency);
				
				// 作ったノードを_nodesに追加する
				_nodes.Add(parentNode);
			}
			
			// 値から深さ優先で、各ノードに対応するBitArrayを作る
			MakeBits();
			
			// 各文字に対応するBitArrayを辞書に登録する
			_bitDictionary = _nodes.Where(c => c.Character != '0').ToDictionary(node => node.Character, node => node.Bits);
			
			// 木を表示する
			foreach (var node in _nodes)
			{
				Console.WriteLine(node);
			}
			
			// 辞書の中身を表示する
			foreach (var (key, value) in _bitDictionary)
			{
				Console.WriteLine($"{key}: {string.Join("", value.Select(x => x ? 1 : 0))}");
			}
		}

		public BitArray Encode(string original)
		{
			// 各文字に対応するBitArrayを連結して返す
			var encoded = new List<bool>();
			foreach (var ch in original)
			{
				var bits = _bitDictionary[ch];
				encoded.AddRange(bits);
			}
			
			// Decodeできるように、ハフマン木の情報を先頭に付加する
			// 
			

			return new BitArray(encoded.ToArray());
		}
		
		private void MakeBits()
		{
			var root = _nodes.LastOrDefault();
			if (root == null)
			{
				return;
			}

			root.SetBits(new List<bool>());
			
			// rootから順に深さ優先探索でビット列を計算する
			if (root.Left == null && root.Right == null)
			{
				return;
			}

			MakeBits(root.Left, new List<bool>() { false });
			MakeBits(root.Right, new List<bool>() { true });
		}

		private void MakeBits(Node? node, List<bool> bitList)
		{
			if (node == null)
			{
				return;
			}
			
			node.SetBits(bitList);

			if (node.Left != null)
			{
				var leftBits = new List<bool>(bitList);
				leftBits.Add(false);
				MakeBits(node.Left, leftBits);
			}
			
			if (node.Right != null)
			{
				var rightBits = new List<bool>(bitList);
				rightBits.Add(true);
				MakeBits(node.Right, rightBits);
			}
		}
	}
	
	// 文字列引数をハフマン符号化する
	public static BitArray Encode(string inputStr)
	{
		// TODO: 入力文字列が1種類の文字で作られる時にも対応する

		HuffmanTree tree = new HuffmanTree(inputStr);
		var encoded = tree.Encode(inputStr);
		
		return encoded;
	}
}