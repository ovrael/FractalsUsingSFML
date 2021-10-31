using Fractals.Helpers;
using Fractals.Models;
using SFML.Graphics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FractalsUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static WindowManagement windowManagement;
		private static double size;
		private static VideoModeRatio ratio;
		private double animationSpeed = 0;

		public MainWindow()
		{
			InitializeComponent();
			size = WindowSizeSlider.Value;
			ratio = (VideoModeRatio)WindowRatioCB.SelectedValue;
		}

		private void Window_Closed(object sender, System.EventArgs e)
		{
			if (windowManagement != null)
			{
				windowManagement.RenderWindow.Close();
			}
		}

		private void Draw_Click(object sender, RoutedEventArgs e)
		{
			if (windowManagement != null)
			{
				windowManagement.RenderWindow.Close();
			}

			windowManagement = new WindowManagement((int)size, ratio, "Fractals");

			DrawAnimatedJulia(CreateAnimatedJulia());

			// Chose Fractal
		}

		private void PreviewTextInputInt(object sender, TextCompositionEventArgs e)
		{
			string input = (sender as TextBox).Text;
			if (e.Text.Contains('-'))
				input = e.Text + input;
			else
				input += e.Text;

			e.Handled = !int.TryParse(input, out int x);
		}

		private void PreviewTextInputDouble(object sender, TextCompositionEventArgs e)
		{
			string input = (sender as TextBox).Text;
			if (e.Text.Contains('-'))
				input = e.Text + input;
			else
				input += e.Text;

			e.Handled = !double.TryParse(input, out double x);
		}

		private AnimatedJuliaSet CreateAnimatedJulia()
		{
			bool error = false;
			if (!int.TryParse(AnimatedJuliaIterationsInput.Text, out int maxIterations) && maxIterations >= 50 && maxIterations < 10000)
			{
				AnimatedJuliaIterationsInput.Background = Brushes.Red;
				error = true;
			}

			if (!int.TryParse(AnimatedJuliaBreakpointInput.Text, out int breakpoint) && breakpoint >= 4 && maxIterations < 256)
			{
				AnimatedJuliaBreakpointInput.Background = Brushes.Red;
				error = true;
			}

			if (!double.TryParse(AnimatedJuliaRadiusInput.Text, out double radius)/*&& radius >= 4 && radius < 256*/)
			{
				AnimatedJuliaRadiusInput.Background = Brushes.Red;
				error = true;
			}

			if (!double.TryParse(AnimatedJuliaAngleInput.Text, out double startAngle)/*&& radius >= 4 && radius < 256*/)
			{
				AnimatedJuliaAngleInput.Background = Brushes.Red;
				error = true;
			}

			double speed = AnimatedJuliaSpeedSlider.Value;

			if (!error)
				return new AnimatedJuliaSet(windowManagement.Width, windowManagement.Height, maxIterations, breakpoint, (float)radius, (float)speed, (float)startAngle);
			else
				return null;
		}

		private void TryUpdateValuesAnimatedJulia(AnimatedJuliaSet animatedJuliaSet)
		{
			animatedJuliaSet.Speed = (float)animationSpeed;

			int.TryParse(AnimatedJuliaIterationsInput.Text, out int maxIterations);
			int.TryParse(AnimatedJuliaBreakpointInput.Text, out int breakpoint);
			double.TryParse(AnimatedJuliaRadiusInput.Text, out double radius);
			double.TryParse(AnimatedJuliaAngleInput.Text, out double angle);


			animatedJuliaSet.MaxIterations = maxIterations;
			animatedJuliaSet.Breakpoint = breakpoint;
			animatedJuliaSet.Radius = (float)radius;
			if (animatedJuliaSet.Speed == 0f)
				animatedJuliaSet.Angle = (float)angle;
			else
				AnimatedJuliaAngleInput.Text = string.Format("{0,8:F4}", animatedJuliaSet.Angle);
		}

		private void DrawAnimatedJulia(AnimatedJuliaSet animatedJuliaSet)
		{
			while (windowManagement.RenderWindow.IsOpen)
			{
				windowManagement.RenderWindow.Clear();
				windowManagement.RenderWindow.DispatchEvents();
				TryUpdateValuesAnimatedJulia(animatedJuliaSet);

				if ((bool)AnimatedJuliaCustomPartsCheck.IsChecked)
				{
					double.TryParse(AnimatedJuliaImaginaryPartInput.Text, out double iPart);
					double.TryParse(AnimatedJuliaRealPartInput.Text, out double rPart);
					animatedJuliaSet.CalculateVertices((float)rPart, (float)iPart);
				}
				else
					animatedJuliaSet.CalculateVertices();


				windowManagement.RenderWindow.Draw(Tools.TwoDArrayToOneD(animatedJuliaSet.Vertices), PrimitiveType.Points);
				windowManagement.RenderWindow.Display();
			}
		}

		private void SetActiveAnimatedJuliaCustomParts(bool isEnabled)
		{
			AnimatedJuliaRealPartText.IsEnabled = isEnabled;
			AnimatedJuliaImaginaryPartText.IsEnabled = isEnabled;
			AnimatedJuliaRealPartInput.IsEnabled = isEnabled;
			AnimatedJuliaImaginaryPartInput.IsEnabled = isEnabled;
		}

		private void AnimatedJuliaCustomPartsCheck_Checked(object sender, RoutedEventArgs e)
		{
			SetActiveAnimatedJuliaCustomParts(true);
		}

		private void AnimatedJuliaCustomPartsCheck_Unchecked(object sender, RoutedEventArgs e)
		{
			SetActiveAnimatedJuliaCustomParts(false);
		}

		private void AnimatedJuliaSpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			animationSpeed = e.NewValue;
			if (animationSpeed > 0f)
			{
				AnimatedJuliaCustomPartsCheck.IsChecked = false;
				AnimatedJuliaCustomPartsCheck.IsEnabled = false;
				SetActiveAnimatedJuliaCustomParts(false);
			}
			else
			{
				AnimatedJuliaCustomPartsCheck.IsEnabled = true;
				SetActiveAnimatedJuliaCustomParts(true);
			}
		}

		private void WindowRatioCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ratio = (VideoModeRatio)(sender as ComboBox).SelectedItem;
		}

		private void WindowSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			size = e.NewValue;
		}
	}
}
