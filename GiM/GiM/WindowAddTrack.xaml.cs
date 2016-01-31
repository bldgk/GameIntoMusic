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
using GiM.Classes;
using GiM.Classes.Data_Classes;
using GiM.Classes.Programm_Classes;

namespace GiM
{
    /// <summary>
    /// Логика взаимодействия для WindowAddTrack.xaml
    /// </summary>
    public partial class WindowAddTrack : Window
    {
        public WindowAddTrack()
        {
            InitializeComponent();
        }

        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(tb_FullName.Text))
                    return tb_FullName.Text;
                else return "New Track ";
            }
            set
            {
                tb_FullName.Text = value;
            }
        }

        public string ShortName
        {
            get
            {
                if (string.IsNullOrEmpty(tb_FullName.Text))
                    return "";
                else return tb_FullName.Text;
            }
            set
            {
                tb_FullName.Text = value;
            }
        }

        //public string Color

        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(tb_Color.Text))
        //            return "Red";
        //        else return tb_FullName.Text;
        //    }
        //    set
        //    {
        //        tb_FullName.Text = value;
        //    }
        //}

        public InstrumentFamily Family
        {
            get
            {
                return (InstrumentFamily)lb_Families.SelectedItem;
            }
            set
            {
                lb_Families.SelectedItem = value;
            }
        }

        public InstrumentType Type
        {
            get
            {
                return (InstrumentType)lb_Types.SelectedItem;
            }
            set
            {
                lb_Types.SelectedItem = value;
            }
        }

        public InstrumentFeature Feature
        {
            get
            {
                return (InstrumentFeature)lb_Features.SelectedItem;
            }
            set
            {
                lb_Features.SelectedItem = value;
            }
        }

       
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        static public void EnumToListBox(Type EnumType, ListBox TheListBox)
        {
            Array Values = System.Enum.GetValues(EnumType);

            foreach (int Value in Values)
            {
                string Display = Enum.GetName(EnumType, Value);
                
            }
            TheListBox.ItemsSource = Values;

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            EnumToListBox(typeof(InstrumentFamily), lb_Families);
            EnumToListBox(typeof(InstrumentFeature), lb_Features);
            EnumToListBox(typeof(InstrumentType), lb_Types);
        }
    }
}
