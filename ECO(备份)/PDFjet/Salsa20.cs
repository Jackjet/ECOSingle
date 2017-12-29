using System;
using System.Text;
public class Salsa20
{
	private string id;
	public Salsa20()
	{
		int[] array = new int[16];
		StringBuilder stringBuilder = new StringBuilder(DateTime.Now.Ticks.ToString("x"));
		int num = 128 - stringBuilder.Length;
		for (int i = 0; i < num; i++)
		{
			stringBuilder.Append('0');
		}
		for (int j = 0; j < 128; j += 8)
		{
			array[j / 8] = (int)Convert.ToInt64(stringBuilder.ToString().Substring(j, 8), 16);
		}
		this.id = this.bin2hex(this.salsa20_word_specification(array));
	}
	private int R(int a, int b)
	{
		int num = 32 - b;
		return a << b | (int)((uint)a >> num);
	}
	private int[] salsa20_word_specification(int[] a_in)
	{
		int[] array = new int[16];
		int[] array2 = new int[16];
		for (int i = 0; i < 16; i++)
		{
			array2[i] = a_in[i];
		}
		for (int j = 20; j > 0; j -= 2)
		{
			array2[4] ^= this.R(array2[0] + array2[12], 7);
			array2[8] ^= this.R(array2[4] + array2[0], 9);
			array2[12] ^= this.R(array2[8] + array2[4], 13);
			array2[0] ^= this.R(array2[12] + array2[8], 18);
			array2[9] ^= this.R(array2[5] + array2[1], 7);
			array2[13] ^= this.R(array2[9] + array2[5], 9);
			array2[1] ^= this.R(array2[13] + array2[9], 13);
			array2[5] ^= this.R(array2[1] + array2[13], 18);
			array2[14] ^= this.R(array2[10] + array2[6], 7);
			array2[2] ^= this.R(array2[14] + array2[10], 9);
			array2[6] ^= this.R(array2[2] + array2[14], 13);
			array2[10] ^= this.R(array2[6] + array2[2], 18);
			array2[3] ^= this.R(array2[15] + array2[11], 7);
			array2[7] ^= this.R(array2[3] + array2[15], 9);
			array2[11] ^= this.R(array2[7] + array2[3], 13);
			array2[15] ^= this.R(array2[11] + array2[7], 18);
			array2[1] ^= this.R(array2[0] + array2[3], 7);
			array2[2] ^= this.R(array2[1] + array2[0], 9);
			array2[3] ^= this.R(array2[2] + array2[1], 13);
			array2[0] ^= this.R(array2[3] + array2[2], 18);
			array2[6] ^= this.R(array2[5] + array2[4], 7);
			array2[7] ^= this.R(array2[6] + array2[5], 9);
			array2[4] ^= this.R(array2[7] + array2[6], 13);
			array2[5] ^= this.R(array2[4] + array2[7], 18);
			array2[11] ^= this.R(array2[10] + array2[9], 7);
			array2[8] ^= this.R(array2[11] + array2[10], 9);
			array2[9] ^= this.R(array2[8] + array2[11], 13);
			array2[10] ^= this.R(array2[9] + array2[8], 18);
			array2[12] ^= this.R(array2[15] + array2[14], 7);
			array2[13] ^= this.R(array2[12] + array2[15], 9);
			array2[14] ^= this.R(array2[13] + array2[12], 13);
			array2[15] ^= this.R(array2[14] + array2[13], 18);
		}
		for (int k = 0; k < 16; k++)
		{
			array[k] = array2[k] + a_in[k];
		}
		return array;
	}
	private string bin2hex(int[] binarray)
	{
		string text = "0123456789abcdef";
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < binarray.Length; i++)
		{
			int num = binarray[i];
			stringBuilder.Append(text[num >> 28 & 15]);
			stringBuilder.Append(text[num >> 24 & 15]);
			stringBuilder.Append(text[num >> 20 & 15]);
			stringBuilder.Append(text[num >> 16 & 15]);
			stringBuilder.Append(text[num >> 12 & 15]);
			stringBuilder.Append(text[num >> 8 & 15]);
			stringBuilder.Append(text[num >> 4 & 15]);
			stringBuilder.Append(text[num & 15]);
		}
		return stringBuilder.ToString().Substring(0, 32);
	}
	public string getID()
	{
		return this.id;
	}
	public static void Main(string[] args)
	{
		new Salsa20();
	}
}
