using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kalkulator
{
    public partial class MainWindow : Window
    {
        private double leftOperand = 0;
        private double rightOperand = 0;
        private string currentOperation = "";
        private bool newInput = true;

        private double lastRightOperand = 0;
        private string lastOperation = "";

        public MainWindow()
        {
            InitializeComponent();
            Display.Text = "0";
        }

        private bool TryGetDisplay(out double value)
        {
            return double.TryParse(
                Display.Text.Replace(',', '.'),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out value);
        }

        private void SetResult(double result)
        {
            Display.Text = result.ToString(CultureInfo.InvariantCulture);
            leftOperand = result;

            currentOperation = "";
            newInput = true;
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;

            string num = btn.Content?.ToString() ?? "0";

            if (newInput || Display.Text == "0")
            {
                Display.Text = num;
                newInput = false;
            }
            else
            {
                Display.Text += num;
            }
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            if (!TryGetDisplay(out double current)) return;

            if (!string.IsNullOrEmpty(currentOperation))
                Calculate(current);
            else
                leftOperand = current;

            currentOperation = btn.Content?.ToString() ?? "0";
            newInput = true;
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetDisplay(out double current)) return;

            if (string.IsNullOrEmpty(currentOperation))
            {
                if (!string.IsNullOrEmpty(lastOperation))
                    CalculateRepeat(current);

                return;
            }

            rightOperand = current;
            lastRightOperand = current;
            lastOperation = currentOperation;

            Calculate(current);
            currentOperation = "";
        }

        private void Calculate(double right)
        {
            switch (currentOperation)
            {
                case "+":
                    leftOperand += right;
                    break;
                case "-":
                    leftOperand -= right;
                    break;
                case "*":
                    leftOperand *= right;
                    break;
                case "/":
                    if (right == 0)
                    {
                        MessageBox.Show("Nie można dzielić przez zero!");
                        return;
                    }
                    leftOperand /= right;
                    break;
            }

            SetResult(leftOperand);
        }

        private void CalculateRepeat(double left)
        {
            switch (lastOperation)
            {
                case "+":
                    leftOperand = left + lastRightOperand;
                    break;
                case "-":
                    leftOperand = left - lastRightOperand;
                    break;
                case "*":
                    leftOperand = left * lastRightOperand;
                    break;
                case "/":
                    if (lastRightOperand == 0)
                    {
                        MessageBox.Show("Nie można dzielić przez zero!");
                        return;
                    }
                    leftOperand = left / lastRightOperand;
                    break;
            }

            SetResult(leftOperand);
        }

        private void Percent_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetDisplay(out double current)) return;

            SetResult(leftOperand * current / 100);
        }

        private void Sqrt_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetDisplay(out double current)) return;

            if (current < 0)
            {
                MessageBox.Show("Nie można pierwiastkować liczby ujemnej");
                return;
            }

            SetResult(Math.Sqrt(current));
        }

        private void Reciprocal_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetDisplay(out double current)) return;

            if (current == 0)
            {
                MessageBox.Show("Nie można dzielić przez zero!");
                return;
            }

            SetResult(1 / current);
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetDisplay(out double current)) return;

            SetResult(current * current);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            leftOperand = 0;
            rightOperand = 0;
            currentOperation = "";
            lastOperation = "";
            lastRightOperand = 0;
            newInput = true;

            Display.Text = "0";
        }

    }
}
