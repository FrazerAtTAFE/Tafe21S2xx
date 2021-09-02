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
 *  Page Title: ShoppingList.cs
 *  Author:     Frazer Dempsey
 *  Project:    5TSD Assignment
 */

namespace StartFinance.Views
{
    public sealed partial class ShoppingListPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public ShoppingListPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            
            // Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            Results();
        }

        public void Results()
        {
            // Create ShoppingList table
            conn.CreateTable<ShoppingList>();
            var query1 = conn.Table<ShoppingList>();

            // Set the ShoppingList database table as the source for the ListView
            ShoppingListView.ItemsSource = query1.ToList();
        }

        private async void AddItem_Click(object sender, RoutedEventArgs e)
        {
            // VALIDATE INPUT
            if (Date.SelectedDate == null)
            {
                MessageDialog dialog = new MessageDialog("Please enter Date", "Oops..!");
                await dialog.ShowAsync();
            }
            else if (ShoppingItem.Text.ToString() == "")
            {
                MessageDialog dialog = new MessageDialog("Please enter Item Name", "Oops..!");
                await dialog.ShowAsync();
            }
            else if (ShopName.Text.ToString() == "")
            {
                MessageDialog dialog = new MessageDialog("Please enter Shop Name", "Oops..!");
                await dialog.ShowAsync();
            }
            else if (Price.ToString() == "")
            {
                MessageDialog dialog = new MessageDialog("Please enter Price", "Oops..!");
                await dialog.ShowAsync();
            }
            else
            {
                try
                {
                    // Convert from DateTimeOffset to DateTime
                    DateTime tempDate = Date.Date.DateTime;
                    decimal TempMoney = Convert.ToDecimal(Price.Text);

                    conn.CreateTable<ShoppingList>();

                    // Insert current data to database
                    conn.Insert(new ShoppingList
                    {
                        DateTime = tempDate,
                        ItemName = ShoppingItem.Text.ToString(),
                        ShopName = ShopName.Text.ToString(),
                        Price = TempMoney
                    });

                    // Update ListView
                    Results();
                }
                catch (Exception ex)
                {
                    if (ex is FormatException)
                    {
                        MessageDialog dialog = new MessageDialog("You forgot to enter the Amount or entered an invalid Amount", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else if (ex is SQLiteException)
                    {
                        MessageDialog dialog = new MessageDialog("Item Name already exist, Try Different Name", "Oops..!");
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

        private async void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            if (ShoppingListView.SelectedIndex == -1)
            {
                MessageDialog dialog = new MessageDialog("Please select an item to update", "Oops..!");
                await dialog.ShowAsync();
            }
            else
            {
                DateTime tempDate = Date.Date.DateTime;
                string tempItem = ShoppingItem.Text.ToString();
                string tempShop = ShopName.Text.ToString();
                decimal tempPrice = Convert.ToDecimal(Price.Text);

                // VALIDATE INPUT
                if (Date.SelectedDate == null)
                {
                    MessageDialog dialog = new MessageDialog("Please enter Date", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (tempItem == "")
                {
                    MessageDialog dialog = new MessageDialog("Please enter Item Name", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (tempShop == "")
                {
                    MessageDialog dialog = new MessageDialog("Please enter Shop Name", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (tempPrice.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Please enter Price", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    try
                    {
                        // WRITE CHANGES TO DATABASE
                        // Get currently selected item
                        int selection = ((ShoppingList)ShoppingListView.SelectedItem).ID;

                        conn.CreateTable<ShoppingList>();
                        conn.Table<ShoppingList>();

                        // Update selected record with new data to database
                        conn.Update(new ShoppingList
                        {
                            ID = selection,
                            DateTime = tempDate,
                            ItemName = tempItem,
                            ShopName = tempShop,
                            Price = tempPrice
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

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            // Check if listview item is slected
            if (ShoppingListView.SelectedIndex == -1)
            {
                MessageDialog dialog = new MessageDialog("Please select an item to delete", "Oops..!");
                await dialog.ShowAsync();
            }
            else
            {
                try
                {
                    // WRITE CHANGES TO DATABASE
                    // Get currently selected item
                    int selection = ((ShoppingList)ShoppingListView.SelectedItem).ID;
                    
                    conn.CreateTable<ShoppingList>();
                    conn.Table<ShoppingList>();
                    
                    // Delete the item
                    conn.Query<ShoppingList>("DELETE FROM ShoppingList WHERE ID ='" + selection + "'");

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

        private void ShoppingList_SelectChange(object sender, SelectionChangedEventArgs e)
        {
            // Update the form input fields when a ListView item is selected
            if (ShoppingListView.SelectedItem != null)
            {
                // 1. Get selected data
                DateTime tempDate = ((ShoppingList)ShoppingListView.SelectedItem).DateTime;
                string tempItem = ((ShoppingList)ShoppingListView.SelectedItem).ItemName;
                string tempShop = ((ShoppingList)ShoppingListView.SelectedItem).ShopName;
                decimal tempPrice = ((ShoppingList)ShoppingListView.SelectedItem).Price;

                // 2. populate input fields
                Date.Date = tempDate;
                ShoppingItem.Text = tempItem;
                ShopName.Text = tempShop;
                Price.Text = tempPrice.ToString();
            }
        }
    }
}
