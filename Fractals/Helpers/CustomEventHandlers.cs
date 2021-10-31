using Fractals.Models;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Fractals.Helpers
{
	public class CustomEventHandlers
	{
		public static Vertex[,]? Vertices { get; set; }
		private static readonly int movingSpeed = 50;

		public static void ExitHandler(object sender, EventArgs e)
		{
			RenderWindow tmp = (RenderWindow)sender;
			if (tmp != null && tmp.IsOpen)
				tmp.Close();
		}

		public static void ScrollHandler(object sender, MouseWheelScrollEventArgs e)
		{
			if (e.Delta > 0)
			{
				Fractal.mapSize /= 100f / 85f + e.Delta / 100;
			}

			if (e.Delta < 0)
			{
				Fractal.mapSize *= 100f / 85f + e.Delta / 100 * Math.Sign(e.Delta);
			}
		}

		private static void MoveVertically(RenderWindow tmp, int direction)
		{
			View view = tmp.GetView();
			view.Move(new Vector2f(0f, movingSpeed * direction));
			tmp.SetView(view);

			if (Vertices != null)
			{
				for (int i = 0; i < Vertices.GetLength(0); i++)
				{
					for (int j = 0; j < Vertices.GetLength(1); j++)
					{
						Vertices[i, j].Position.Y += movingSpeed * direction;
					}
				}
			}
		}

		private static void MoveHorizontally(RenderWindow tmp, int direction)
		{
			View view = tmp.GetView();
			view.Move(new Vector2f(movingSpeed * direction, 0));
			tmp.SetView(view);

			if (Vertices != null)
			{
				for (int i = 0; i < Vertices.GetLength(0); i++)
				{
					for (int j = 0; j < Vertices.GetLength(1); j++)
					{
						Vertices[i, j].Position.X += movingSpeed * direction;
					}
				}
			}
		}

		public static void KeyHandler(object sender, KeyEventArgs e)
		{

			if (Vertices == null || sender == null)
				return;

			RenderWindow tmp = (RenderWindow)sender;

			switch (e.Code)
			{
				case Keyboard.Key.Unknown:
					break;

				case Keyboard.Key.W:
					MoveVertically(tmp, -1);
					break;

				case Keyboard.Key.S:
					MoveVertically(tmp, 1);
					break;

				case Keyboard.Key.A:
					MoveHorizontally(tmp, -1);
					break;

				case Keyboard.Key.D:
					MoveHorizontally(tmp, 1);
					break;

				case Keyboard.Key.Q:
					tmp.Close();
					break;

				default:
					break;
			}
		}
	}
}
