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

//You should have received a copy of the GNU General Public License
//along with Start Finance.If not, see<http://www.gnu.org/licenses/>.
// ***************************************************************************

using SQLite.Net;
using StartFinance.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

/*
    Page Title: AppointmentPage.xaml.cs
    Author:     Vadim Kolosov
    Project:    5TSD Assignment
    
    Last Updated: 19.09.2021

    Date        Notes
    ================================================
    19.09.2021  First version

 */

namespace StartFinance.Views
{
    // ShoppingList -> Appointment DELETE

    public sealed partial class AppointmentPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public AppointmentPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            
            // Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            Results();
        }

        public void Results()
        {
            // conn.DropTable<Appointment>();
            // Create Appointment table
            conn.CreateTable<Appointment>();
            var query1 = conn.Table<Appointment>();

            // Set the Appointment database table as the source for the ListView
            AppointmentView.ItemsSource = query1.ToList();
        }

        private async void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            // VALIDATE INPUT
            if (eventName.Text.ToString() == "")
            {
                MessageDialog dialog = new MessageDialog("Please enter Event Name", "Oops..!");
                await dialog.ShowAsync();
            }
            else if (eventLocation.Text.ToString() == "")
            {
                MessageDialog dialog = new MessageDialog("Please enter Event Location", "Oops..!");
                await dialog.ShowAsync();
            }
            else if (eventDate.SelectedDate == null)
            {
                MessageDialog dialog = new MessageDialog("Please enter start date and time", "Oops..!");
                await dialog.ShowAsync();
            }
            else if (eventDateFinish.SelectedDate == null)
            {
                MessageDialog dialog = new MessageDialog("Please enter finish date and time", "Oops..!");
                await dialog.ShowAsync();
            }
            else
            {
                try
                {
                    /* convert date time
                    store it as datetime type.
                    DateTime date;
                    date = dateTimePicker1.Value;

                    DATETIME SQLite format: YYYY-MM-DD HH:MI:SS

                    // Convert from DateTimeOffset to DateTime
                    string CDay = Birthday.Date.Value.Day.ToString();
                    string CMonth = Birthday.Date.Value.Month.ToString();
                    string CYear = Birthday.Date.Value.Year.ToString();
                    String DateTime = "" + CMonth + "/" + CDay + "/" + CYear;
                    */

                    DateTime tempDate = eventDate.Date.DateTime;
                    DateTime tempDateFinish = eventDateFinish.Date.DateTime;

                    conn.CreateTable<Appointment>();

                    // ShopName -> Location DELETE
                    // ItemName -> EventName DELETE 
                    // Price -> DateTimeFinish

                    // Insert current data to database
                    conn.Insert(new Appointment
                    {
                        EventName = eventName.Text.ToString(),
                        Location = eventLocation.Text.ToString(),
                        EventDateTime = tempDate,
                        EventDateTimeFinish = tempDateFinish
                    });

                    // Update ListView
                    Results();
                }
                catch (Exception ex)
                {
                    if (ex is FormatException)
                    {
                        MessageDialog dialog = new MessageDialog("Please, fill in all the form items", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else if (ex is SQLiteException)
                    {
                        MessageDialog dialog = new MessageDialog("Appointment could not be writtent to the database", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        MessageDialog dialog = new MessageDialog("Unknown Error has occured", "Oops..!");
                        await dialog.ShowAsync();
                    }
                }
            }
        }

        private async void EditeAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (AppointmentView.SelectedIndex == -1)
            {
                MessageDialog dialog = new MessageDialog("Please select an appointment to edit", "Oops..!");
                await dialog.ShowAsync();
            }
            else
            {
                // DateTime tempDate = Date.Date.DateTime;
                string tempEvent = eventName.Text.ToString();
                string tempLocation = eventLocation.Text.ToString();
                // DateTime tempDate = DateTime.Date.DateTime;
                // DateTime tempDateFinish = DateTimeFinish.Date.DateTime;
                DateTime tempDate = DateTime.Now;
                DateTime tempDateFinish = DateTime.Now;

                // VALIDATE INPUT
                if (tempEvent == "")
                {
                    MessageDialog dialog = new MessageDialog("Please enter Item Name", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (tempLocation == "")
                {
                    MessageDialog dialog = new MessageDialog("Please enter Shop Name", "Oops..!");
                    await dialog.ShowAsync();
                }
                
                // else if (tempDate.SelectedDate == null)
                else if (tempDate == null)
                        {
                    MessageDialog dialog = new MessageDialog("Please enter when event starts", "Oops..!");
                    await dialog.ShowAsync();
                }
                // also below:
                else if (tempDateFinish == null)
                {
                    MessageDialog dialog = new MessageDialog("Please enter when event finishes", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    try
                    {
                        // WRITE CHANGES TO DATABASE
                        // Get currently selected item
                        int selection = ((Appointment)AppointmentView.SelectedItem).ID;

                        conn.CreateTable<Appointment>();
                        conn.Table<Appointment>();

                        // Update selected record with new data to database
                        conn.Update(new Appointment
                        {
                            ID = selection,
                            EventName = tempEvent,
                            Location = tempLocation,
                            EventDateTime = tempDate,
                            EventDateTimeFinish = tempDateFinish
                        });

                        // Update ListView
                        Results();
                    }
                    catch (Exception ex)
                    {
                        if (ex is SQLiteException)
                        {
                            MessageDialog dialog = new MessageDialog(ex.ToString(), "Oops..!");
                            await dialog.ShowAsync();
                        }
                    }
                }
            }
        }

        private async void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            // Check if listview item is slected
            if (AppointmentView.SelectedIndex == -1)
            {
                MessageDialog dialog = new MessageDialog("Please select an event to delete", "Oops..!");
                await dialog.ShowAsync();
            }
            else
            {
                try
                {
                    // WRITE CHANGES TO DATABASE
                    // Get currently selected item
                    int selection = ((Appointment)AppointmentView.SelectedItem).ID;
                    
                    conn.CreateTable<Appointment>();
                    conn.Table<Appointment>();
                    
                    // Delete the item
                    conn.Query<Appointment>("DELETE FROM Appointment WHERE ID ='" + selection + "'");

                    // Update the listview
                    Results();
                }
                catch (Exception ex)
                {
                    if (ex is SQLiteException)
                    {
                        MessageDialog dialog = new MessageDialog(ex.ToString(), "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        MessageDialog dialog = new MessageDialog(ex.ToString(), "Oops..!");
                        await dialog.ShowAsync();
                    }
                }
            }
        }

        private void Appointment_SelectChange(object sender, SelectionChangedEventArgs e)
        {
            // Update the form input fields when a ListView item is selected
            if (AppointmentView.SelectedItem != null)
            {
                // 1. Get selected data
                string tempEvent = ((Appointment)AppointmentView.SelectedItem).EventName;
                string tempLocation = ((Appointment)AppointmentView.SelectedItem).Location;
                DateTime tempDate = ((Appointment)AppointmentView.SelectedItem).EventDateTime;
                DateTime tempDateFinish = ((Appointment)AppointmentView.SelectedItem).EventDateTimeFinish;

                // 2. populate input fields
                eventName.Text = tempEvent;
                eventLocation.Text = tempLocation;
                eventDate.Date = tempDate;
                eventDateFinish.Date = tempDateFinish;
            }
        }
    }
}
