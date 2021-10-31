using SFML.Graphics;
using SFML.Window;

namespace Fractals.Helpers
{
	public class WindowManagement
	{
		public RenderWindow RenderWindow { get; }
		public int Width { get; }
		public int Height { get; }

		public WindowManagement(int windowSize, VideoModeRatio ratio, string title)
		{
			int[] ratioVector = CreateRatioVector(ratio);

			Width = 10 * windowSize * ratioVector[0];
			Height = 10 * windowSize * ratioVector[1];

			VideoMode videoMode = new VideoMode((uint)Width, (uint)Height, 0);
			RenderWindow = new RenderWindow(videoMode, title, Styles.Close, new ContextSettings(0, 0, 0));

			RenderWindow.MouseWheelScrolled += new EventHandler<MouseWheelScrollEventArgs>(CustomEventHandlers.ScrollHandler);
			RenderWindow.KeyPressed += new EventHandler<KeyEventArgs>(CustomEventHandlers.KeyHandler);
			RenderWindow.Closed += new EventHandler(CustomEventHandlers.ExitHandler);

			RenderWindow.SetActive();
			RenderWindow.Clear();
		}

		private static int[] CreateRatioVector(VideoModeRatio ratio)
		{
			int[] ratioVector = new int[2];
			switch (ratio)
			{
				case VideoModeRatio.Square:
					ratioVector[0] = 9;
					ratioVector[1] = 9;
					break;
				case VideoModeRatio.Wide:
					ratioVector[0] = 16;
					ratioVector[1] = 9;
					break;
				case VideoModeRatio.UltraWide:
					ratioVector[0] = 21;
					ratioVector[1] = 9;
					break;
				default:
					break;
			}
			return ratioVector;
		}

	}
}
