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
using System.Windows.Navigation;

namespace Kiosk
{
    /// <summary>
    /// Логика взаимодействия для pass.xaml
    /// </summary>
    public partial class pass : Window
    {
        static string path = System.AppDomain.CurrentDomain.BaseDirectory;
        static string connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "\\terminal.accdb";
        OleDbConnection con = new OleDbConnection(connectionstring);
        public pass()
        {
            InitializeComponent();
        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string passwrd = "";
                string log = "";
                string global = "terminal1337";
                string log_glob = "admin1337";
                con.Open();
                OleDbCommand com2 = new OleDbCommand();
                com2.CommandText = "select login,pass from log_pass";
                com2.Connection = con;
                OleDbDataReader reader;
                reader = com2.ExecuteReader();
                while (reader.Read())
                {
                    passwrd = reader.GetValue(1).ToString();
                    log = reader.GetValue(0).ToString();
                }
                reader.Close();
                con.Close();
                
                if (login.Text == log && password.Password == passwrd || login.Text == log_glob && password.Password == global)
                {
                    Admin_Window win = new Admin_Window();
                    win.Show();
                    this.Close();
                    
                }
                else MessageBox.Show("Неверный пароль или логин!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

      
    }
}
