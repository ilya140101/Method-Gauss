using Method_Gauss.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace labLegkovWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBox[,] matrixTextBox;
        TextBox[] minorTextBox;
        int n, m;
        public MainWindow()
        {
            InitializeComponent();
            n = m = 0;
           
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void NumberMinusValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            matrixGrid.Children.Clear();
            matrixGrid.RowDefinitions.Clear();
            matrixGrid.ColumnDefinitions.Clear();
            minorGrid.Children.Clear();
            minorGrid.RowDefinitions.Clear();
            minorGrid.ColumnDefinitions.Clear();
            Int32.TryParse(nTextBox.Text, out n);
            Int32.TryParse(mTextBox.Text, out m);
            Console.WriteLine(n + " " + m);
            if (n > m || n <= 0 || m <= 0)
            {
                MessageBox.Show("Некорректные размеры");
                return;
            }
            matrixTextBox = new TextBox[n, m];
            minorTextBox = new TextBox[n];

            minorGrid.RowDefinitions.Add(new RowDefinition());
            minorGrid.RowDefinitions[0].Height = new GridLength(50);

            for (int i = 0; i < n; i++)
            {
                matrixGrid.RowDefinitions.Add(new RowDefinition());
                matrixGrid.RowDefinitions[i].Height = new GridLength(50);

                minorGrid.ColumnDefinitions.Add(new ColumnDefinition());
                minorGrid.ColumnDefinitions[i].Width = new GridLength(50);
                
            }
            for (int i = 0; i < m; i++)
            {
                matrixGrid.ColumnDefinitions.Add(new ColumnDefinition());

                matrixGrid.ColumnDefinitions[i].Width = new GridLength(50);
            }

            for (int i = 0; i < n; i++)
            {
                minorTextBox[i] = new TextBox();
                minorTextBox[i].PreviewTextInput += NumberValidationTextBox;
                minorTextBox[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                minorTextBox[i].VerticalContentAlignment = VerticalAlignment.Center;
                Grid.SetColumn(minorTextBox[i], i);
                minorGrid.Children.Add(minorTextBox[i]);
                for (int j = 0; j < m; j++)
                {
                    matrixTextBox[i, j] = new TextBox();
                    matrixTextBox[i, j].PreviewTextInput += NumberMinusValidationTextBox;
                    matrixTextBox[i, j].HorizontalContentAlignment = HorizontalAlignment.Center;
                    matrixTextBox[i, j].VerticalContentAlignment = VerticalAlignment.Center;                   
                    Grid.SetRow(matrixTextBox[i, j], i);
                    Grid.SetColumn(matrixTextBox[i, j], j);
                    matrixGrid.Children.Add(matrixTextBox[i, j]);
                }
            }
            EnterMatrix.Visibility = Visibility.Visible;
            EnterMinor.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            matrixGrid.Children.Clear();
            matrixGrid.RowDefinitions.Clear();
            matrixGrid.ColumnDefinitions.Clear();
            minorGrid.Children.Clear();
            minorGrid.RowDefinitions.Clear();
            minorGrid.ColumnDefinitions.Clear();
            nTextBox.Text = "";
            mTextBox.Text = "";
            EnterMatrix.Visibility = Visibility.Collapsed;
            EnterMinor.Visibility = Visibility.Collapsed;
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            Method_Gauss.Class.Matrix matrix = new Method_Gauss.Class.Matrix(n, m);
            int[,] mat;
            int[] basis;
            mat = new int[n, m];
            basis = new int[n];
            for (int i = 0; i < n; i++)
            {
                Int32.TryParse(minorTextBox[i].Text, out basis[i]);
                for (int j = 0; j < m; j++)
                {
                    Int32.TryParse(matrixTextBox[i, j].Text, out mat[i, j]);
                    
                }                
            }
            try
            {

                matrix.completion(mat, basis);
                if (Standart.IsPressed)
                    matrix.Gauss(false);
                else
                    matrix.Gauss(true);
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                        matrixTextBox[i, j].Text = matrix.matrix[i, j].ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
