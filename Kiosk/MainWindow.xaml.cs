using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Resources;
using System.Data.OleDb;
using System.IO;

namespace Kiosk
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static string path = System.AppDomain.CurrentDomain.BaseDirectory;
        static string connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "\\terminal.accdb";
        OleDbConnection con = new OleDbConnection(connectionstring);
        Button[] bt = new Button[8];
        static int NUMBER;
        static string s;
        private void main_rtf()
        {
            content1.Children.Clear();
            RichTextBox rtb = new RichTextBox();
            rtb.IsReadOnly = true;
            rtb.Width = content1.Width;
            rtb.BorderBrush = Brushes.White;
            rtb.Height = content1.Height;
            content1.Children.Add(rtb);
            TextRange range;
            FileStream fStream;
            if (File.Exists(path + "\\startwin.rtf"))
            {
                range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                fStream = new FileStream(path + "\\startwin.rtf", FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            bt[0] = Button_1;
            bt[1] = Button_2;
            bt[2] = Button_3;
            bt[3] = Button_4;
            bt[4] = Button_5;
            bt[5] = Button_6;
            bt[6] = Button_7;
            bt[7] = Button_8;
            back.Visibility = Visibility.Hidden;
            main_rtf();

        }
        private string perevod (int i)
        {
            switch (NUMBER)
            {
                case 1: s = "pen"; break;
                case 2: s = "over"; break;
                case 3: s = "soc"; break;
                case 4: s = "nor"; break;
                case 5: s = "ipc"; break;
                case 6: s = "moth"; break;
                case 7: s = "nakop"; break;
                case 8: s = "snils"; break;
                case 9: s = "rab"; break;
                case 10:s = "info"; break;
            }
            return s;
           
        }
        private void vyvod(string s)
        {
            try
            {
                back.Visibility = Visibility.Hidden;
                int count = 0;
                string str1 = "select count (*) from " + s;
                string str2 = "select " + s + "_name ," + s + "_id from " + s + " where " + s + "_id = ";
                content1.Children.Clear();
                con.Open();
                OleDbCommand com = new OleDbCommand();
                com.CommandText = str1;
                com.Connection = con;
                count = Convert.ToInt16(com.ExecuteScalar());
                OleDbDataReader reader = com.ExecuteReader();
                reader.Close();
                string textblock = "";
                string id = "";
                for (int i = 1; i <= count; i++)
                {
                    StackPanel stp = new StackPanel();
                    TextBlock txb = new TextBlock();
                    com.CommandText = str2 + i;
                    reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        textblock = reader.GetValue(0).ToString();
                        id = reader.GetValue(1).ToString();
                    }
                    reader.Close();
                    //MessageBox.Show(str2);
                    stp.Orientation = Orientation.Horizontal;
                    stp.Width = content1.Width;
                    stp.Height = 40;
                    txb.Text = textblock;
                    txb.TextWrapping = TextWrapping.Wrap;
                    txb.FontSize = 14;
                    txb.Tag = id;
                    txb.VerticalAlignment = VerticalAlignment.Center;
                    txb.TextWrapping = TextWrapping.WrapWithOverflow;
                    txb.Width = content1.Width;
                    txb.Foreground = Brushes.Black;
                    txb.FontFamily = new FontFamily("Segoe UI");
                    stp.Margin = new Thickness(10, 10, 0, 0);
                    stp.Background = Brushes.LightSteelBlue;
                    txb.Margin = new Thickness(5, 0, 0, 0);
                    content1.Children.Add(stp);
                    stp.Children.Add(txb);
                    txb.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(txb_MouseUp);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
            finally
            {
                con.Close();
            }
        }
        private void vybor(string str)
        {
            RichTextBox rtb1 = new RichTextBox();
            rtb1.IsReadOnly = true;
            rtb1.Opacity = 1;
            rtb1.Width = content1.Width;
            rtb1.Height = content1.Height;
            rtb1.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;            
            con.Open();
            OleDbCommand com = new OleDbCommand();
            com.Connection = con;
            com.CommandText = str;
            string localpath = "";
            OleDbDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                localpath = reader.GetValue(0).ToString();
            }
            //MessageBox.Show(localpath);
            content1.Children.Clear();
            content1.Children.Add(rtb1);
            TextRange range;
            FileStream fStream;
            if (File.Exists(path + localpath))
            {
                range = new TextRange(rtb1.Document.ContentStart, rtb1.Document.ContentEnd);
                fStream = new FileStream(path + localpath, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }
            
            
        }
        private void txb_MouseUp(object sender, System.EventArgs e)
        {
            
            try
            {
                back.Visibility = Visibility.Visible;
                TextBlock txb = (TextBlock)sender;
                //MessageBox.Show(txb.Text);
                switch (NUMBER)
                {
                    case 1:
                        {
                            string str = "select pen_path from pen where pen_id = " + txb.Tag;
                            vybor(str);                
                            break;
                        }

                    case 2:
                        {
                            string str = "select over_path from over where over_id = " + txb.Tag;
                            vybor(str);
                            break;
                        }
                    case 3:
                        {
                            string str = "select soc_path from soc where soc_id = " + txb.Tag;
                            vybor(str);
                            break;
                        }
                    case 4:
                        {
                            string str = "select nor_path from nor where nor_id = " + txb.Tag;
                            vybor(str);
                            break;
                        }
                    case 5:
                        {
                            string str = "select ipc_path from ipc where ipc_id = " + txb.Tag;
                            vybor(str);
                            break;
                        }
                    case 6:
                        {
                            string str = "select nor_path from nor where nor_id = " + txb.Tag;
                            vybor(str);
                            break;
                        }
                    case 7:
                        {
                            string str = "select nakop_path from nakop where nakop_id = " + txb.Tag;
                            vybor(str);
                            break;
                        }
                    case 8:
                        {
                            string str = "select snils_path from snils where snils_id = " + txb.Tag;
                            vybor(str);
                            break;
                        }
                   case 9:
                        {
                            string str = "select rab_path from rab where rab_id = " + txb.Tag;
                            vybor(str);
                            break;
                        }
                    case 10:
                        {
                            string str = "select info_path from info where info_id = " + txb.Tag;
                            vybor(str);
                            break;
                        }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            if (Button_1.Content.ToString() == "Гражданам".Trim())
            {
                content1.Children.Clear();
                Button_1.Content = "Пенсии";
                but_tbx.Text = "Пенсии проживающим за границей";
                txb_but3.Text = "Социальные выплаты";
                Button_4.Content = "Выплаты пенсионерам-\"северянам\"";
                kostyl.Text = "Сведения об индивидуальном лицевом счете";
                Button_6.Content = "Материнский(семейный) капитал";
                Button_7.Content = "Пенсионные накопления";
                Button_8.Content = "СНИЛС";
                for (int i = 0; i < 8; i++) bt[i].Visibility = Visibility.Visible;
            }
            //MessageBox.Show(Button_1.Content+"");

            else if (Button_1.Content.ToString() == "Пенсии".Trim())
            {
                
                string s = "pen";
                for (int i = 0; i < 8; i++)
                {
                    if (i == 0) bt[i].Background = Brushes.DeepSkyBlue;
                    else bt[i].Background = new SolidColorBrush( Color.FromRgb(45,106,198));
                }
                
                NUMBER = 1;
                vyvod(s);

            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bt[0].Content = "Гражданам";
            back.Visibility = Visibility.Hidden;
            but_tbx.Text = "Работодателям и самозанятому населению";
            txb_but3.Text = "Информационно-разъяснительные материалы ПФР";
            for (int i = 3; i < 8; i++) bt[i].Visibility = Visibility.Hidden;
            main_rtf();
        }

        private void Button_2_Click_1(object sender, RoutedEventArgs e)
        {
            if (but_tbx.Text == "Пенсии проживающим за границей".Trim())
            {
                for (int i = 0; i < 8; i++)
                {
                    if (i == 1) bt[i].Background = Brushes.DeepSkyBlue;
                    else bt[i].Background = new SolidColorBrush(Color.FromRgb(45, 106, 198));
                }
                string s = "over";
                vyvod(s);
                NUMBER = 2;
            }
            else if (but_tbx.Text == "Работодателям и самозанятому населению".Trim())
            {
                for (int i = 0; i < 8; i++)
                {
                    if (i == 1) bt[i].Background = Brushes.DeepSkyBlue;
                    else bt[i].Background = new SolidColorBrush(Color.FromRgb(45, 106, 198));
                }
                content1.Children.Clear();
                string s = "rab";
                vyvod(s);
                NUMBER = 9;
            }
        }

        private void Button_3_Click(object sender, RoutedEventArgs e)
        {
            if (txb_but3.Text == "Информационно-разъяснительные материалы ПФР")
            {
                string s = "info";
                vyvod(s);
                NUMBER = 10;
            }
            else{
                for (int i = 0; i < 8; i++)
                {
                    if (i == 2) bt[i].Background = Brushes.DeepSkyBlue;
                    else bt[i].Background = new SolidColorBrush(Color.FromRgb(45, 106, 198));
                }
                string s = "soc";
                vyvod(s);
                NUMBER = 3;
            }
        }

        private void Button_4_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i == 3) bt[i].Background = Brushes.DeepSkyBlue;
                else bt[i].Background = new SolidColorBrush(Color.FromRgb(45, 106, 198));
            }
            string s = "nor";
            vyvod(s);
            NUMBER = 4;
        }

        private void Button_5_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i == 4) bt[i].Background = Brushes.DeepSkyBlue;
                else bt[i].Background = new SolidColorBrush(Color.FromRgb(45, 106, 198));
            }
            string s = "ipc";
            vyvod(s);
            NUMBER = 5;
        }

        private void Button_6_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i == 5) bt[i].Background = Brushes.DeepSkyBlue;
                else bt[i].Background = new SolidColorBrush(Color.FromRgb(45, 106, 198));
            }
            string s = "moth";
            vyvod(s);
            NUMBER = 6;
        }

        private void Button_7_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i == 6) bt[i].Background = Brushes.DeepSkyBlue;
                else bt[i].Background = new SolidColorBrush(Color.FromRgb(45, 106, 198));
            }
            string s = "nakop";
            vyvod(s);
            NUMBER = 7;
        }

        private void Button_8_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i == 7) bt[i].Background = Brushes.DeepSkyBlue;
                else bt[i].Background = new SolidColorBrush(Color.FromRgb(45, 106, 198));
            }
            NUMBER = 8;
            string s = "snils";
            vyvod(s);
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            back.Visibility = Visibility.Hidden;
            content1.Children.Clear();
            string s = perevod(NUMBER);
            vyvod(s);
            
        }

        private void admin_Click(object sender, RoutedEventArgs e)
        {
            pass p = new pass();
            p.Show();
        }

        private void Button_9_Click(object sender, RoutedEventArgs e)
        {
            content1.Children.Clear();
            RichTextBox rtb = new RichTextBox();
            rtb.IsReadOnly = true;
            rtb.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            rtb.Width = content1.Width;
            rtb.BorderBrush = Brushes.White;
            rtb.Height = content1.Height;
            content1.Children.Add(rtb);
            TextRange range;
            FileStream fStream;
            if (File.Exists(path + "\\help.rtf"))
            {
                range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                fStream = new FileStream(path + "\\help.rtf", FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }
        }

        
    } 
}
