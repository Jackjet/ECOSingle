using System;
namespace PDFjet.NET
{
	public class RGB
	{
		public static float[] BLACK;
		public static float[] WHITE;
		public static float[] RED;
		public static float[] GREEN;
		public static float[] BLUE;
		public static float[] YELLOW;
		public static float[] CYAN;
		public static float[] MAGENTA;
		public static float[] LIGHT_GRAY;
		public static float[] GRAY;
		public static float[] DARK_GRAY;
		public static float[] LIGHT_GREY;
		public static float[] GREY;
		public static float[] DARK_GREY;
		public static float[] OLD_GLORY_BLUE;
		public static float[] OLD_GLORY_RED;
		static RGB()
		{
			// Note: this type is marked as 'beforefieldinit'.
			float[] bLACK = new float[3];
			RGB.BLACK = bLACK;
			RGB.WHITE = new float[]
			{
				1f,
				1f,
				1f
			};
			float[] array = new float[3];
			array[0] = 1f;
			RGB.RED = array;
			float[] array2 = new float[3];
			array2[1] = 1f;
			RGB.GREEN = array2;
			RGB.BLUE = new float[]
			{
				0f,
				0f,
				1f
			};
			float[] array3 = new float[3];
			array3[0] = 1f;
			array3[1] = 1f;
			RGB.YELLOW = array3;
			RGB.CYAN = new float[]
			{
				0f,
				1f,
				1f
			};
			RGB.MAGENTA = new float[]
			{
				1f,
				0f,
				1f
			};
			RGB.LIGHT_GRAY = new float[]
			{
				0.75f,
				0.75f,
				0.75f
			};
			RGB.GRAY = new float[]
			{
				0.5f,
				0.5f,
				0.5f
			};
			RGB.DARK_GRAY = new float[]
			{
				0.25f,
				0.25f,
				0.25f
			};
			RGB.LIGHT_GREY = new float[]
			{
				0.75f,
				0.75f,
				0.75f
			};
			RGB.GREY = new float[]
			{
				0.5f,
				0.5f,
				0.5f
			};
			RGB.DARK_GREY = new float[]
			{
				0.25f,
				0.25f,
				0.25f
			};
			RGB.OLD_GLORY_BLUE = new float[]
			{
				0f,
				0.192156866f,
				0.407843143f
			};
			RGB.OLD_GLORY_RED = new float[]
			{
				0.7490196f,
				0.0392156877f,
				0.1882353f
			};
		}
	}
}
