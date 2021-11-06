using SFML.Graphics;

namespace Fractals.Helpers
{
	public class Tools
	{
		public static float Map(float value, float minValue, float maxValue, float newMinValue, float newMaxValue)
		{
			return newMinValue + (newMaxValue - newMinValue) * ((value - minValue) / (maxValue - minValue));
		}

		public static Vertex[] TwoDArrayToOneD(Vertex[,] vertices)
		{
			Vertex[] points = new Vertex[vertices.GetLength(0) * vertices.GetLength(1)];

			int k = 0;
			for (int i = 0; i < vertices.GetLength(0); i++)
			{
				for (int j = 0; j < vertices.GetLength(1); j++)
				{
					points[k] = vertices[i, j];
					k++;
				}
			}

			return points;
		}

		public static void HsvToRgb(double Hue, double S, double V, out int r, out int g, out int b)
		{
			while (Hue < 0) { Hue += 360; };
			while (Hue >= 360) { Hue -= 360; };
			double R, G, B;
			if (V <= 0)
			{ R = G = B = 0; }
			else if (S <= 0)
			{
				R = G = B = V;
			}
			else
			{
				double hf = Hue / 60.0;
				int i = (int)Math.Floor(hf);
				double f = hf - i;
				double pv = V * (1 - S);
				double qv = V * (1 - S * f);
				double tv = V * (1 - S * (1 - f));
				switch (i)
				{

					// Red is the dominant color

					case 0:
						R = V;
						G = tv;
						B = pv;
						break;

					// Green is the dominant color

					case 1:
						R = qv;
						G = V;
						B = pv;
						break;
					case 2:
						R = pv;
						G = V;
						B = tv;
						break;

					// Blue is the dominant color

					case 3:
						R = pv;
						G = qv;
						B = V;
						break;
					case 4:
						R = tv;
						G = pv;
						B = V;
						break;

					// Red is the dominant color

					case 5:
						R = V;
						G = pv;
						B = qv;
						break;

					// Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

					case 6:
						R = V;
						G = tv;
						B = pv;
						break;
					case -1:
						R = V;
						G = pv;
						B = qv;
						break;

					// The color is not defined, we should throw an error.

					default:
						//LFATAL("i Value error in Pixel conversion, Value is %d", i);
						R = G = B = V; // Just pretend its black/white
						break;
				}
			}
			r = Clamp((int)(R * 255.0));
			g = Clamp((int)(G * 255.0));
			b = Clamp((int)(B * 255.0));
		}

		static int Clamp(int i)
		{
			if (i < 0) return 0;
			if (i > 255) return 255;
			return i;
		}
	}
}
