using Fractals.Helpers;
using SFML.Graphics;

namespace Fractals.Models
{
	public class JuliaSet : Fractal
	{
		public int MaxIterations { get; set; }
		public float Breakpoint { get; set; }
		public float Radius { get; set; }
		public float Speed { get; set; }
		public float Angle { get; set; }
		public ColorMode ColorMode { get; set; }
		public ColorData ColorData { get; set; }

		public JuliaSet(int width, int height, int maxIterations, float breakpoint, float radius, float speed, float angle) : base(width, height)
		{
			MaxIterations = maxIterations;
			Breakpoint = breakpoint;
			Radius = radius;
			Speed = speed;
			Angle = angle;
			ColorMode = ColorMode.HSV;
			ColorData = new ColorData();
		}

		private void UpdateVerticesColor(int breakIterations, int x, int y)
		{
			int red = 0;
			int green = 0;
			int blue = 0;

			if (ColorMode.Equals(ColorMode.HSV) || ColorMode.Equals(ColorMode.CustomHSV))
			{
				double hu = 0;
				if (breakIterations != MaxIterations)
				{
					hu = Math.Sqrt((double)breakIterations / MaxIterations);
				}

				hu = Tools.Map((float)hu, 0, 1, 0, 360);

				// Tools.HsvToRgbTest(hu, 1, (double)150 / 255, out red, out green, out blue);
				if (ColorMode.Equals(ColorMode.CustomHSV))
					Tools.HsvToRgb(hu, (double)ColorData.Saturation / 255, (double)ColorData.Value / 255, out red, out green, out blue);
				else
					Tools.HsvToRgb(hu, 255d / 255, 150d / 255, out red, out green, out blue);
			}
			else
			{
				var bright = Tools.Map(breakIterations, 0, MaxIterations, 0, 1);
				bright = Tools.Map((float)Math.Sqrt(bright), 0, 1, 0, 255);

				if (breakIterations == MaxIterations)
				{
					bright = 0;
				}

				switch (ColorMode)
				{
					case ColorMode.Red:
						red = (int)bright;
						break;

					case ColorMode.Green:
						green = (int)bright;
						break;

					case ColorMode.Blue:
						blue = (int)bright;
						break;

					case ColorMode.Gray:
						red = (int)bright;
						blue = (int)bright;
						green = (int)bright;
						break;

					case ColorMode.CustomRGB:
						red = (int)(bright * ColorData.Red / 255);
						green = (int)(bright * ColorData.Green / 255);
						blue = (int)(bright * ColorData.Blue / 255);
						break;
				}

			}

			Vertices[x, y].Color = new Color((byte)red, (byte)green, (byte)blue);
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

					UpdateVerticesColor(n, x, y);
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
						float newZ = zSquare + real;

						a = newZ;
						b = twoAB - imaginary;
					}

					UpdateVerticesColor(n, x, y);
				}
			}
			Angle += Speed;
		}
	}
}
