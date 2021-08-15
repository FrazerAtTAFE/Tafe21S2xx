using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// **************************************************************************
//Start Finance - An to manage your personal finances.

//Start Finance is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//Start Finance is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

//You should have received a copy of the GNU General Public License
//along with Start Finance.If not, see<http://www.gnu.org/licenses/>.

// ** Data          Author              Version                 Comments
// ** 14/08/2021    Shan Wang           0.9                     First Version: Add Personal info page to add, delete or update personal information
// ***************************************************************************


using SQLite;
using StartFinance.Models;
using Windows.UI.Popups;
using SQLite.Net;


namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonalInfoPage : Page
    {

        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public PersonalInfoPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            Results();
        }

        public void Results()
        {
            conn.CreateTable<PersonalInfo>();
            var query1 = conn.Table<PersonalInfo>();
            PersonalInfoView.ItemsSource = query1.ToList();
        }

        private async void AddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FirstName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("First Name Empty", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (LastName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Last Name Empty", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (Birthday.Date == null)
                {
                    MessageDialog dialog = new MessageDialog("Birthday Empty", "Oops..!");
                    await dialog.ShowAsync();
                }

                else if (Gender.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Gender Empty", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (Email.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Email Empty", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (Phone.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Phone Empty", "Oops..!");
                    await dialog.ShowAsync();
                }
                //create contact table, then insert record into contact table
                else
                {
                    string CDay = Birthday.Date.Value.Day.ToString();
                    string CMonth = Birthday.Date.Value.Month.ToString();
                    string CYear = Birthday.Date.Value.Year.ToString();
                    String DateTime = "" + CMonth + "/" + CDay + "/" + CYear;


                    conn.CreateTable<PersonalInfo>();
                    conn.Insert(new PersonalInfo
                    {
                        FirstName = FirstName.Text.ToString(),
                        LastName = LastName.Text.ToString(),
                        DateOfBirth = DateTime,
                        Gender = Gender.Text.ToString(),
                        Email = Email.Text.ToString(),
                        Phone = Phone.Text.ToString()

                    });
                    // Creating table
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter some information", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog(ex.ToString(), "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    /// no idea
                }
            }
        }

        //delete contact by full name
        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (PersonalInfoView.SelectedItem != null)
            {
                try
                {
                    string AccSelection_FirstName = ((PersonalInfo)PersonalInfoView.SelectedItem).FirstName;
                    string AccSelection_LastName = ((PersonalInfo)PersonalInfoView.SelectedItem).LastName;
                    if (AccSelection_FirstName == "")
                    {
                        MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        conn.CreateTable<PersonalInfo>();
                        var query1 = conn.Table<PersonalInfo>();
                        var query3 = conn.Query<PersonalInfo>("DELETE FROM PERSONALINFO WHERE FirstName ='" + AccSelection_FirstName + "'" + "AND LastName ='" + AccSelection_LastName + "'");
                        PersonalInfoView.ItemsSource = query1.ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog dialog = new MessageDialog(ex.ToString(), "Oops..!");
                    await dialog.ShowAsync();
                }

            }
            else
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
                
        }

        private async void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FirstName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("First Name Empty", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (LastName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Last Name Empty", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (Birthday.Date == null)
                {
                    MessageDialog dialog = new MessageDialog("Birthday Empty", "Oops..!");
                    await dialog.ShowAsync();
                }

                else if (Gender.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Gender Empty", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (Email.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Email Empty", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (Phone.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Phone Empty", "Oops..!");
                    await dialog.ShowAsync();
                }
                //create contact table, then insert record into contact table
                else
                {
                    string CDay = Birthday.Date.Value.Day.ToString();
                    string CMonth = Birthday.Date.Value.Month.ToString();
                    string CYear = Birthday.Date.Value.Year.ToString();
                    String DateTime = "" + CMonth + "/" + CDay + "/" + CYear;


                    conn.CreateTable<PersonalInfo>();
                    var query1 = conn.Table<PersonalInfo>();
                    var query3 = conn.Query<PersonalInfo>("UPDATE PERSONALINFO SET DateOfBirth = '" + DateTime + "', Gender = '" + Gender.Text.ToString() + "', Email = '" + Email.Text.ToString() + "', Phone = '" + Phone.Text.ToString() + "'WHERE FirstName = '" + FirstName.Text.ToString() + "'" + "AND LastName = '" + LastName.Text.ToString() + "'");
                    PersonalInfoView.ItemsSource = query1.ToList();
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter some information", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog(ex.ToString(), "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    /// no idea
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }


        //triggered when select item change in PersonalInfo view
        private void PersonalInfoView_SelectChange(object sender, SelectionChangedEventArgs e)
        {

            if (PersonalInfoView.SelectedItem != null)
            {
                string AccSelection_FirstName = ((PersonalInfo)PersonalInfoView.SelectedItem).FirstName;
                if (AccSelection_FirstName != "")
                {
                    FirstName.Text = AccSelection_FirstName;
                    LastName.Text = ((PersonalInfo)PersonalInfoView.SelectedItem).LastName;
                    Birthday.DataContext = ((PersonalInfo)PersonalInfoView.SelectedItem).DateOfBirth;
                    Gender.Text = ((PersonalInfo)PersonalInfoView.SelectedItem).Gender;
                    Email.Text = ((PersonalInfo)PersonalInfoView.SelectedItem).Email;
                    Phone.Text = ((PersonalInfo)PersonalInfoView.SelectedItem).Phone;

                }
            }
        }
    }
}
