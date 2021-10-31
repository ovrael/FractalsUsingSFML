using Fractals.Helpers;
using SFML.Graphics;

namespace Fractals.Models
{
	public class Fractal
	{
		public static float mapSize = 1.7f;
		public int Width { get; set; }
		public int Height { get; set; }
		public float WindowRatio { get; set; }
		public Vertex[,] Vertices { get; set; }

		public Fractal(int width, int height)
		{
			Width = width;
			Height = height;
			WindowRatio = (float)Width / Height;
			Vertices = new Vertex[width, height];

			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					Vertices[i, j].Position = new SFML.System.Vector2f(i, j);
				}
			}

			CustomEventHandlers.Vertices = Vertices;
		}
	}
}
