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
using Windows.UI.Popups;
using SQLite.Net;
using StartFinance.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShoppingListPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public ShoppingListPage()
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
            // Creating table
            conn.CreateTable<ShoppingList>();
            var query = conn.Table<ShoppingList>();
            ShoppingListView.ItemsSource = query.ToList();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IDtextBox.Text.ToString() == "" || ShoppingDate.Text.ToString() == "" || NameOfItem.Text.ToString() == "" || PriceQuoted.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Pleas fill in the required fields", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.Insert(new ShoppingList()
                    {
                        ID = IDtextBox.Text,
                        ShoppingDate = ShoppingDate.Text,
                        NameOfItem = PriceQuoted.Text,
                        PriceQuoted = PriceQuoted.Text,
                    });
                    Results();
                }
            }

            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    //No Idea
                }
            }
        }


    

    private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Do You Want To Update", "Important");
            ShowConf.Commands.Add(new UICommand("Yes Update")
            {
                Id = 0
            });
            ShowConf.Commands.Add(new UICommand("Cancel")
            {
                Id = 1
            });
            ShowConf.DefaultCommandIndex = 0;
            ShowConf.CancelCommandIndex = 1;

            var result = await ShowConf.ShowAsync();
            if ((int)result.Id == 0)
            {
                try
                {
                    string ContactLabels = ((ShoppingList)ShoppingListView.SelectedItem).ID;
                    var updateQuery = conn.Query<ShoppingList>("UPDATE ShoppingList set ID ='" + IDtextBox.Text + "', ShoppingDate ='" + ShoppingDate.Text + "', ItemName ='" + NameOfItem.Text + "'PriceQuoted ='" + PriceQuoted.Text + "' where ID =" + ContactLabels);
                    Results();
                }
                catch (NullReferenceException)
                {
                    MessageDialog ClearDailog = new MessageDialog("Please Select the Item to Update", "Oops....!");
                    await ClearDailog.ShowAsync();
                }
            }


        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
           
            MessageDialog ShowConf = new MessageDialog("Deleting this ShoppingList will delete all details of this contact", "Important");
            ShowConf.Commands.Add(new UICommand("Yes, Delete")
            {
                Id = 0
            });
            ShowConf.Commands.Add(new UICommand("Cancel")
            {
                Id = 1
            });
            ShowConf.DefaultCommandIndex = 0;
            ShowConf.CancelCommandIndex = 1;

            var result = await ShowConf.ShowAsync();
            if ((int)result.Id == 0)
            {
                try
                {
                    string ContactLabel = ((ShoppingList)ShoppingListView.SelectedItem).ID;
                    var delQuery = conn.Query<ShoppingList>("DELETE FROM ShoppingList WHERE ID='" + ContactLabel + "'");
                    Results();
                }
                catch (NullReferenceException)
                {
                    MessageDialog ClearDialog1 = new MessageDialog("Please select the item to Delete", "Oops..!");
                    await ClearDialog1.ShowAsync();
                }
            }
            else
            {
                //No Idea
            }

        }

        private void ShoppingListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShoppingListView.SelectedItem != null)
            {
                IDtextBox.Text = ((ShoppingList)ShoppingListView.SelectedItem).ID;
                ShoppingDate.Text = ((ShoppingList)ShoppingListView.SelectedItem).ShoppingDate;
                NameOfItem.Text = ((ShoppingList)ShoppingListView.SelectedItem).NameOfItem;
                PriceQuoted.Text = ((ShoppingList)ShoppingListView.SelectedItem).PriceQuoted;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }
    }
    }
