using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GiM
{
    /// <summary>
    /// Логика взаимодействия для Dopusk.xaml
    /// </summary>
    public partial class Dopusk : Window
    {
        public Dopusk()
        {
            InitializeComponent();
        }
        static int[] numbersArray = new int[10];
        static string[] operatorsArray = new string[9];

        static string storageVariable;
        static int numbersCounter = 0;
        static int operatorsCounter = 0;
        static int total = 0;
        static bool totalled = false;


        private void One_Click(object sender, RoutedEventArgs e)
        {
            if (totalled == true)
            {
                Display.Text = "";
                totalled = false;
            }
            Display.Text += "1";
            storageVariable += "1";
        }
        private void Two_Click(object sender, RoutedEventArgs e)
        {
            if (totalled == true)
            {
                Display.Text = "";
                totalled = false;
            }
            Display.Text += "2";
            storageVariable += "2";
        }
        private void Three_Click(object sender, RoutedEventArgs e)
        {
            if (totalled == true)
            {
                Display.Text = "";
                totalled = false;
            }
            Display.Text += "3";
            storageVariable += "3";
        }
        private void Four_Click(object sender, RoutedEventArgs e)
        {
            if (totalled == true)
            {
                Display.Text = "";
                totalled = false;
            }
            Display.Text += "4";
            storageVariable += "4";
        }
        private void Five_Click(object sender, RoutedEventArgs e)
        {
            if (totalled == true)
            {
                Display.Text = "";
                totalled = false;
            }
            Display.Text += "5";
            storageVariable += "5";
        }
        private void Six_Click(object sender, RoutedEventArgs e)
        {
            if (totalled == true)
            {
                Display.Text = "";
                totalled = false;
            }
            Display.Text += "6";
            storageVariable += "6";
        }
        private void Seven_Click(object sender, RoutedEventArgs e)
        {
            if (totalled == true)
            {
                Display.Text = "";
                totalled = false;
            }
            Display.Text += "7";
            storageVariable += "7";
        }
        private void Eight_Click(object sender, RoutedEventArgs e)
        {
            if (totalled == true)
            {
                Display.Text = "";
                totalled = false;
            }
            Display.Text += "8";
            storageVariable += "8";
        }
        private void Nine_Click(object sender, RoutedEventArgs e)
        {
            if (totalled == true)
            {
                Display.Text = "";
                totalled = false;
            }
            Display.Text += "9";
            storageVariable += "9";
        }
        private void Zero_Click(object sender, RoutedEventArgs e)
        {
            if (totalled == true)
            {
                Display.Text = "";
                totalled = false;
            }
            Display.Text += "0";
            storageVariable += "0";
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            setNumber(storageVariable);
            setOperator("+");
            Display.Text += "+";
        }
        private void Subtract_Click(object sender, RoutedEventArgs e)
        {
            setNumber(storageVariable);
            setOperator("-");
            Display.Text += "-";
        }
        private void Multiply_Click(object sender, RoutedEventArgs e)
        {
            setNumber(storageVariable);
            setOperator("*");
            Display.Text += "x";
        }
        private void Divide_Click(object sender, RoutedEventArgs e)
        {
            setNumber(storageVariable);
            setOperator("/");
            Display.Text += "/";
        }
        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            setNumber(storageVariable);
            for (int i = 0; i < operatorsCounter; i++)
            {
                if (operatorsArray[i] == "+" && i == 0)
                {
                    total = numbersArray[i] + numbersArray[i + 1];
                }
                else if (operatorsArray[i] == "+")
                {
                    total = total + numbersArray[i + 1];
                }
                else if (operatorsArray[i] == "-" && i == 0)
                {
                    total = numbersArray[i] - numbersArray[i + 1];
                }
                else if (operatorsArray[i] == "-")
                {
                    total = total - numbersArray[i + 1];
                }
                else if (operatorsArray[i] == "*" && i == 0)
                {
                    total = numbersArray[i] * numbersArray[i + 1];
                }
                else if (operatorsArray[i] == "*")
                {
                    total = total * numbersArray[i + 1];
                }
                else if (operatorsArray[i] == "/" && i == 0)
                {
                    total = numbersArray[i] / numbersArray[i + 1];
                }
                else if (operatorsArray[i] == "/")
                {
                    total = total / numbersArray[i + 1];
                }
            }
            Display.Text = total.ToString(); ;
            numbersArray = null;
            operatorsArray = null;
            storageVariable = null;
            numbersCounter = 0;
            operatorsCounter = 0;
            total = 0;
            totalled = true;
        }
        static void setNumber(String Number)
        {
            numbersArray[numbersCounter] = Convert.ToInt32(Number);
            storageVariable = null;
            numbersCounter++;
        }
        static void setOperator(String Op)
        {
            operatorsArray[operatorsCounter] = Op;
            operatorsCounter++;
        }
        private void AC_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "";
            numbersArray = null;
            operatorsArray = null;
            storageVariable = null;
            numbersCounter = 0;
            operatorsCounter = 0;
            total = 0;
        }
    }
}
