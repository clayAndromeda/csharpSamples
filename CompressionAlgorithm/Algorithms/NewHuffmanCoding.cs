using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace Clay.Compression.Algorithms;

public class HuffmanNode
{
	public HuffmanNode Left { get; set; }
	public HuffmanNode Right { get; set; }
	public HuffmanNode Parent { get; set; }
	public char Character { get; set; }
	public int Frequency { get; set; }

	public List<bool> Traverse(char symbol, List<bool> data)
	{
		// Leaf node
		if (Right == null && Left == null)
		{
			if (symbol.Equals(this.Character))
			{
				return data;
			}
			else
			{
				return null;
			}
		}
		else
		{
			List<bool> left = null;
			List<bool> right = null;

			if (Left != null)
			{
				List<bool> leftPath = new List<bool>();
				leftPath.AddRange(data);
				leftPath.Add(false);

				left = Left.Traverse(symbol, leftPath);
			}

			if (Right != null)
			{
				List<bool> rightPath = new List<bool>();
				rightPath.AddRange(data);
				rightPath.Add(true);

				right = Right.Traverse(symbol, rightPath);
			}

			if (left != null)
			{
				return left;
			}
			else
			{
				return right;
			}
		}
	}

}

public class HuffmanTree
{
	private List<HuffmanNode> nodes = new List<HuffmanNode>();
	public HuffmanNode Root { get; set; }
	public Dictionary<char, int> Frequencies = new Dictionary<char, int>();
	
	public void Build(string source)
	{
		for (int i = 0; i < source.Length; i++)
		{
			if (!Frequencies.ContainsKey(source[i]))
			{
				Frequencies.Add(source[i], 0);
			}

			Frequencies[source[i]]++;
		}

		foreach (KeyValuePair<char, int> symbol in Frequencies)
		{
			nodes.Add(new HuffmanNode() { Character = symbol.Key, Frequency = symbol.Value });
		}

		while (nodes.Count > 1)
		{
			List<HuffmanNode> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList<HuffmanNode>();
			if (orderedNodes.Count >= 2)
			{
				List<HuffmanNode> taken = orderedNodes.Take(2).ToList<HuffmanNode>();
				HuffmanNode parent = new HuffmanNode()
				{
					Character = '*', Frequency = taken[0].Frequency + taken[1].Frequency, Left = taken[0],
					Right = taken[1]
				};
				taken[0].Parent = parent;
				taken[1].Parent = parent;
				nodes.Remove(taken[0]);
				nodes.Remove(taken[1]);
				nodes.Add(parent);
			}

			this.Root = nodes.FirstOrDefault();
		}
	}

	public BitArray Encode(string source)
	{
		List<bool> encodedSource = new List<bool>();
		for (int i = 0; i < source.Length; i++)
		{
			List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
			encodedSource.AddRange(encodedSymbol);
		}

		BitArray bits = new BitArray(encodedSource.ToArray());
		return bits;
	}

	public string Decode(BitArray bits)
	{
		HuffmanNode current = this.Root;
		string decoded = "";
		foreach (bool bit in bits)
		{
			if (bit)
			{
				if (current.Right != null)
				{
					current = current.Right;
				}
			}
			else
			{
				if (current.Left != null)
				{
					current = current.Left;
				}
			}

			if (IsLeaf(current))
			{
				decoded += current.Character;
				current = this.Root;
			}
		}

		return decoded;
	}

	public bool IsLeaf(HuffmanNode node)
	{
		return (node.Left == null && node.Right == null);
	}
}

public static class HuffmanAlgorithm
{
	public static void CompressTest(string text)
	{
		HuffmanTree huffmanTree = new HuffmanTree();
		huffmanTree.Build(text);
		BitArray encoded = huffmanTree.Encode(text);
		string decoded = huffmanTree.Decode(encoded);
		
		Console.WriteLine($"{text} | {decoded}");
		if (text != decoded)
		{
			Console.WriteLine($"ERROR {text} | {decoded}");
		}
		
		Console.WriteLine($"{text.Length * 8}, {encoded.Count}");
	}
}