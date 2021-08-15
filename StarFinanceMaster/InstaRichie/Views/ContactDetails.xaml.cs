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
// ** 14/08/2021    Shan Wang           0.9                     First Version: Add contact details page to add, delete or update contact information
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
    public sealed partial class ContactDetailsPage : Page
    {

        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public ContactDetailsPage()
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
            conn.CreateTable<Contact>();
            var query1 = conn.Table<Contact>();
            ContactView.ItemsSource = query1.ToList();
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
                else if (CompanyName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Company Name Empty", "Oops..!");
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
                    conn.CreateTable<Contact>();
                    conn.Insert(new Contact
                    {
                        FirstName = FirstName.Text.ToString(),
                        LastName = LastName.Text.ToString(),
                        CompanyName = CompanyName.Text.ToString(),
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
            if (ContactView.SelectedItem != null)
            {
                try
                {
                    string AccSelection_FirstName = ((Contact)ContactView.SelectedItem).FirstName;
                    string AccSelection_LastName = ((Contact)ContactView.SelectedItem).LastName;
                    if (AccSelection_FirstName == "")
                    {
                        MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        conn.CreateTable<Contact>();
                        var query1 = conn.Table<Contact>();
                        var query3 = conn.Query<Contact>("DELETE FROM Contact WHERE FirstName ='" + AccSelection_FirstName + "'" + "AND LastName ='" + AccSelection_LastName + "'");
                        ContactView.ItemsSource = query1.ToList();
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
                
                else if (CompanyName.Text.ToString() == "")
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
                    conn.CreateTable<Contact>();
                    var query1 = conn.Table<Contact>();
                    var query3 = conn.Query<Contact>("UPDATE CONTACT SET CompanyName = '" + CompanyName.Text.ToString() + "', Phone = '" + Phone.Text.ToString() + "'WHERE FirstName = '" + FirstName.Text.ToString() + "'" + "AND LastName = '" + LastName.Text.ToString() + "'");
                    ContactView.ItemsSource = query1.ToList();
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


        //triggered when select item change in contact view
        private void ContactView_SelectChange(object sender, SelectionChangedEventArgs e)
        {

            if (ContactView.SelectedItem != null)
            {
                string AccSelection_FirstName = ((Contact)ContactView.SelectedItem).FirstName;
                if (AccSelection_FirstName != "")
                {
                    FirstName.Text = AccSelection_FirstName;
                    LastName.Text = ((Contact)ContactView.SelectedItem).LastName;
                    CompanyName.Text = ((Contact)ContactView.SelectedItem).CompanyName;
                    Phone.Text = ((Contact)ContactView.SelectedItem).Phone;

                }
            }
        }
    }
}

