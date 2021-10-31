using Fractals.Helpers;
using SFML.Graphics;

namespace Fractals.Models
{
	public class AnimatedJuliaSet : Fractal
	{
		public int MaxIterations { get; set; }
		public float Breakpoint { get; set; }
		public float Radius { get; set; }
		public float Speed { get; set; }
		public float Angle { get; set; }

		public AnimatedJuliaSet(int width, int height, int maxIterations, float breakpoint, float radius, float speed, float angle) : base(width, height)
		{
			MaxIterations = maxIterations;
			Breakpoint = breakpoint;
			Radius = radius;
			Speed = speed;
			Angle = Angle;
		}

		public void CalculateVertices(float realPart, float imaginaryPart)
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					float a = Tools.Map(Vertices[x, y].Position.X, 0, Width, -mapSize * WindowRatio, mapSize * WindowRatio);
					float b = Tools.Map(Vertices[x, y].Position.Y, 0, Height, -mapSize, mapSize);

					int n = 0;
					for (n = 0; n < MaxIterations; n++)
					{
						float aSquare = a * a;
						float bSquare = b * b;
						float twoAB = 2 * a * b;

						if (aSquare + bSquare > Breakpoint)
						{
							break;
						}

						float zSquare = aSquare - bSquare;

						float newZ = zSquare + realPart;
						a = newZ;
						b = twoAB - imaginaryPart;
					}

					double hu = Math.Sqrt((double)n / MaxIterations);
					hu = Tools.Map((float)hu, 0, 1, 0, 360);

					Tools.HsvToRgbTest(hu, 1, (double)150 / 255, out int r, out int g, out int blue);
					Vertices[x, y].Color = new Color((byte)r, (byte)g, (byte)blue);
				}
			}
		}

		public void CalculateVertices()
		{
			float real = (float)(Radius * Math.Cos(Angle) + Radius * Math.Sin(Angle));
			float imaginary = (float)Math.Cos(Angle);

			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					float a = Tools.Map(Vertices[x, y].Position.X, 0, Width, -mapSize * WindowRatio, mapSize * WindowRatio);
					float b = Tools.Map(Vertices[x, y].Position.Y, 0, Height, -mapSize, mapSize);

					int n = 0;
					for (n = 0; n < MaxIterations; n++)
					{
						float aSquare = a * a;
						float bSquare = b * b;
						float twoAB = 2 * a * b;

						if (aSquare + bSquare > Breakpoint)
						{
							break;
						}

						float zSquare = aSquare - bSquare;

						// real : −0.8
						// imaginary: +0.156

						//float newZ = zSquare - 0.8f;
						float newZ = zSquare + real;
						a = newZ;
						//b= twoAB + 0.156f;
						b = twoAB - imaginary;
					}
					//var bright = Map(n, 0, maxIterations, 0, 1);
					//bright = Map((float)Math.Sqrt(bright), 0, 1, 0, 255);

					//if (n == maxIterations)
					//{
					//	bright = 0;
					//}
					//vertices[x, y].Color = new Color((byte)bright, (byte)bright, (byte)bright);

					double hu = Math.Sqrt((double)n / MaxIterations);
					hu = Tools.Map((float)hu, 0, 1, 0, 360);

					Tools.HsvToRgbTest(hu, 1, (double)150 / 255, out int r, out int g, out int blue);
					Vertices[x, y].Color = new Color((byte)r, (byte)g, (byte)blue);
				}
			}
			Angle += Speed;
		}
	}
}
