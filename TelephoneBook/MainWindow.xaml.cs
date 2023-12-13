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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using youtube;
using static System.Collections.Specialized.BitVector32;

namespace TelephoneBook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataBase _db;
        string floor;
        string nz;
        public MainWindow()
        {
            InitializeComponent();
            _db = new DataBase();
            collapps();
            LWindow.Visibility = Visibility.Visible;
        }
        private void collapps()
        {
            LWindow.Visibility = Visibility.Collapsed;
            RWindow.Visibility = Visibility.Collapsed;
            admin.Visibility = Visibility.Collapsed;
            DWindow.Visibility = Visibility.Collapsed;
            EWindow.Visibility = Visibility.Collapsed;
        }
        private void regsbtn_Click(object sender, RoutedEventArgs e)
        {
            collapps();
            RWindow.Visibility = Visibility.Visible;
        }
        private void regbtn_Click(object sender, RoutedEventArgs e)
        {
            infolr.Content = null;
            collapps();
            _db.quarry($"select count(*) from \"Userses\" where login = @value1;", new string[] { regtbl.Text });
            _db.reader.Read();
            int k = Convert.ToInt32(_db.reader[0].ToString());
            if (k != 0)
            {
                infolr.Content = "Логин уже занят.";
            }
            else
            {
                _db.close();
                _db.quarry($"select count(*) from \"Userses\" ;", new string[] { });
                _db.reader.Read();
                k = Convert.ToInt32(_db.reader[0].ToString());
                _db.close();
                _db.quarry($" insert into \"Userses\" values(@value1,@value2,@value3);", new string[] {k.ToString(), regtbl.Text, regtbp.Text });
                RWindow.Visibility = Visibility.Collapsed;
                admin.Visibility = Visibility.Visible;
                
                _db.close();
            }
        }
        private void M_Click(object sender, RoutedEventArgs e)
        {
            floor = "Мужской";
        }
        private void F_Click(object sender, RoutedEventArgs e)
        {
            floor = "Женский";
        }
        private void logbtn_Click(object sender, RoutedEventArgs e)
        {
            infoll.Content = null;
            _db.quarry($"select count(*) from \"Userses\" where login = @value1 and password = @value2; ", new string[] { logtbl.Text, logtbp.Text });
            try
            {
                _db.reader.Read();
                if (_db.reader[0].ToString() != "0")
                {

                    collapps();
                    admin.Visibility = Visibility.Visible;
                }
                _db.close();
            }
            catch
            {
                _db.close();
                if (LWindow.Visibility != Visibility.Collapsed)
                {
                    infoll.Content = "Неверный логин или пароль.";
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _db.close();
            _db.quarry($"select count(*) from \"Telephone_numb\";", new string[] {  });
            _db.reader.Read();
            int k = Convert.ToInt32(_db.reader[0].ToString());
            _db.close();
            _db.quarry($"select count(*) from \"Telephone_numb\" where numb_country = @value1 and numb_city = @value2 and numb = @value3; ", new string[] { names.Text, names_Copy.Text, fileChoise.Text });
            _db.reader.Read();
            if (_db.reader[0].ToString() == "0")
            {
                _db.close();
                _db.quarry($"select count(*) from \"Telephone_user\";", new string[] { });
                _db.reader.Read();
                int k1 = Convert.ToInt32(_db.reader[0].ToString());
                _db.close();
                _db.quarry($"insert into  \"Telephone_user\" values(@value1,@value2,@value3,@value4,@value5);", new string[] { k1.ToString(), fname.Text, sname.Text, srdname.Text, floor });

                _db.close();
                _db.quarry($"insert into  \"Telephone_numb\" values(@value1,@value2,@value3,@value4);", new string[] { k1.ToString(), names.Text, names_Copy.Text, fileChoise.Text });
                _db.close();
                }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            collapps();
            DWindow.Visibility = Visibility.Visible;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            collapps();
            admin.Visibility = Visibility.Visible;
        }
        private void Double_click(object sender, RoutedEventArgs e)
        {
            var s = db.SelectedItem;
            collapps();
            EWindow.Visibility = Visibility.Visible;
            _db.close();
            _db.quarry($"select * from \"Telephone_numb\" where CONCAT(numb_country, numb_city, numb) = @value1 ; ", new string[] { (s as archive).Ball });
            _db.reader.Read();
            names1.Text=_db.reader[1].ToString();
            names_Copy1.Text=_db.reader[2].ToString();
            fileChoise1.Text=_db.reader[3].ToString();

            nz = _db.reader[0].ToString();
            _db.close();
            _db.quarry($"select * from \"Telephone_user\" where n_z = @value1 ; ", new string[] { nz });
            _db.reader.Read();
            fname.Text = _db.reader[1].ToString();
            sname.Text = _db.reader[2].ToString();
            srdname.Text = _db.reader[3].ToString();
            if(_db.reader[4].ToString()== "Мужской")
            {
                cb.SelectedIndex = 0;
                floor = "Мужской";
            }
                
            else
            {
                floor= "Женский";
               cb.SelectedIndex = 1;
            }
        }

        

        private void admin_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (admin.Visibility != Visibility.Collapsed)
            {
                _db.close();
                _db.quarry($"select * from \"Telephone_numb\" JOIN \"Telephone_user\" ON \"Telephone_numb\".n_z = \"Telephone_user\".n_z ;", new string[] { });
                List<archive> aList = new List<archive>();
                while (_db.reader.Read())
                {
                    aList.Add(new archive
                    {
                        Country = _db.reader[1].ToString(),
                        City = _db.reader[2].ToString(),
                        Numb = _db.reader[3].ToString(),
                        User = _db.reader[5].ToString()+" "+ _db.reader[6].ToString() + " " + _db.reader[7].ToString(),
                        Data_add = _db.reader[8].ToString(),
                        Ball = _db.reader[1].ToString()+ _db.reader[2].ToString()+ _db.reader[3].ToString(),
                    });
                }

                db.ItemsSource = aList;

            }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            _db.close();
            _db.quarry($"UPDATE \"Telephone_numb\" SET numb_country=@value2, numb_city=@value3, numb=@value4 WHERE n_z = @value1;", new string[] { nz, names1.Text, names_Copy1.Text, fileChoise1.Text});
            _db.close();
            _db.quarry($"UPDATE \"Telephone_user\" SET name=@value2,f_name=@value3,s_name=@value4,floor=@value5 WHERE n_z = @value1;", new string[] { nz, fname1.Text, sname1.Text, srdname1.Text, floor });
            collapps();
            admin.Visibility = Visibility.Visible;
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            _db.close();
            _db.quarry($"delete from \"Telephone_numb\" WHERE n_z = @value1;", new string[] { nz });
            _db.close();
            _db.quarry($"delete from \"Telephone_user\"  WHERE n_z = @value1;", new string[] { nz});
            collapps();
            admin.Visibility = Visibility.Visible;

        }
    }
}
