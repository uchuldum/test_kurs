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
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;
using System.IO;
using Microsoft.Win32;
namespace Kiosk
{
    /// <summary>
    /// Логика взаимодействия для Admin_Window.xaml
    /// </summary>
    public partial class Admin_Window : Window
    {
        public Admin_Window()
        {
            InitializeComponent();
        }
        static string table_name;
        static string path = System.AppDomain.CurrentDomain.BaseDirectory;
        static string connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "\\terminal.accdb";
        OleDbConnection con = new OleDbConnection(connectionstring);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string str = "select true_name from main_table where id = ";
            OleDbCommand com = new OleDbCommand();
            com.Connection = con;
            con.Open();
            for (int i = 1; i <= 10; i++)
            {
                com.CommandText = str + i;
                OleDbDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetValue(0).ToString());
                }
                reader.Close();
            }
            con.Close();
        }
        private string select(string s)
        {
           string j= perevod(s);
           return j;
        }
        private string perevod(string s )
        {
            string a ="";
            switch (s)
            {
                case "Пенсии": a = "pen"; break;
                case "Пенсии проживающим за границей": a = "over"; break;
                case "Социальные выплаты": a = "soc"; break;
                case "Выплаты пенсионерам-\"северянам\"": a = "nor"; break;
                case "Сведения об индивидуальном лицевом счете": a = "ipc"; break;
                case "Материнский(семейный) капитал": a = "moth"; break;
                case "Пенсионные накопления": a = "nakop"; break;
                case "СНИЛС": a = "snils"; break;
                case "Работодателям и самозанятому населению": a = "rab"; break;
                case "Информационно-разъяснительные материалы ПФР": a = "info"; break;
            }
            return a;

        }
        DataGrid dg = new DataGrid();
        private void table_ok_Click(object sender, RoutedEventArgs e)
        {
            table.Children.Clear();
            try
            {
                
                table_name = select(comboBox1.SelectedItem.ToString());
                string str = "select " + table_name + "_id as Номер," + table_name + "_name as Название," + table_name + "_path as Путь from " + table_name;
                table.Children.Add(dg);
                StackPanel stp = new StackPanel();
                table.Children.Add(stp);
                stp.Orientation = Orientation.Horizontal;
                Button add = new Button();
                add.Width = 100;
                add.Height = 40;
                Button delete = new Button();
                delete.Width = 100;
                delete.Height = 40;
                delete.Content = "Удалить";
                add.Content = "Добавить";
                Button exit = new Button();
                exit.Width = add.Width;
                exit.Height = add.Height;
                exit.Margin = new Thickness(10, 0, 0, 0);
                exit.Content = "Выход";
                exit.Click += new RoutedEventHandler(exit_Click);
                delete.Click += new RoutedEventHandler(delete_Click);
                add.Click += new RoutedEventHandler(add_Click);
                stp.HorizontalAlignment = HorizontalAlignment.Center;
                delete.Margin = new Thickness(10, 0, 0, 0);
                stp.Children.Add(add);
                stp.Children.Add(delete);
                stp.Children.Add(exit);
                stp.Margin = new Thickness(0, 15, 0, 0);
                dg.Margin = new Thickness(0, 10, 0, 0);
                con.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(str, con);
                OleDbCommandBuilder build = new OleDbCommandBuilder(adapter);
                DataSet ds = new DataSet();
                adapter.Fill(ds, table_name);
                dg.ItemsSource = ds.Tables[table_name].DefaultView;
            }
            catch
            {

            }
            finally
            {
                con.Close();
            }
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
        /// <summary>
        /// /////////////DELETE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
           {

                OleDbCommand com = new OleDbCommand();
                com.CommandText = "delete * from "+table_name+" where "+table_name+"_id = " + ((DataRowView)dg.SelectedItems[0]).Row["Номер"].ToString();
                com.Connection = con;
                con.Open();
                com.ExecuteNonQuery();
                string delete_file = ((DataRowView)dg.SelectedItems[0]).Row["Путь"].ToString();
                //MessageBox.Show(delete_file);
                if (File.Exists(path + delete_file))
                    File.Delete(path + delete_file);
               // MessageBox.Show(((DataRowView)dg.SelectedItems[0]).Row["Номер"].ToString());
                

            }
            catch 
            {
                MessageBox.Show("Выберите непустую строку в таблице перед удалением");
                con.Close();
            }
            finally
            {
                con.Close();
                
                table_ok_Click(sender, e);
            }
        }
            ////////////////////INSERT
        OpenFileDialog ofd = new OpenFileDialog();
        private void add_Click(object sender, RoutedEventArgs e)
        {   
            //////////Выбрать файл
            ofd.Filter = "rtf files (*.rtf)|*.rtf";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog()==true)
            {
                try
                {
                    string asd = "";
                    con.Open();
                    File.Copy(ofd.FileName, path + "\\" + table_name + "\\" + ofd.SafeFileName);
                    OleDbCommand com = new OleDbCommand();
                    com.Connection = con;
                    string str2 = "select max ("+table_name+"_id) from " + table_name;
                    //MessageBox.Show(str2);
                    com.CommandText = str2;
                    int count = 0;
                    asd = com.ExecuteScalar().ToString();
                    if (asd != "")
                        count = Convert.ToInt16(asd);
                    //MessageBox.Show(count + "");
                    string str = "insert into " + table_name + "(" + table_name + "_id," + table_name + "_name," + table_name + "_path) values(" + (count + 1) + ",'" + (ofd.SafeFileName).Substring(0, ofd.SafeFileName.Length - 4) + "'," + "'\\" + table_name + "\\" + ofd.SafeFileName + "')";
                    com.CommandText = str;
                    //MessageBox.Show(str);
                    com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex + "");
                }
                finally
                {
                    con.Close();
                    table_ok_Click(sender, e);
                    
                }
                //MessageBox.Show(str);
            }
            else
            {
                MessageBox.Show("ERROR");
            }
            
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            table_ok_Click(sender, e);
        }
    }
    
}
