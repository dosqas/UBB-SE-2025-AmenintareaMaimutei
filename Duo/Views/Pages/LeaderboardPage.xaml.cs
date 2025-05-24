using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using DuoClassLibrary.Models;
using System.Collections.ObjectModel;

using System.Data;
using DuolingoNou.Views.Pages;
using Duo.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Pages;


/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LeaderboardPage : Page
{
    public ObservableCollection<LeaderboardEntry> Leaderboard { get; set; }
    private LeaderboardViewModel _leaderboardViewModel;
    private string _selectedMode = "Global";
    private int currentUserId = App.CurrentUser.UserId;
    
    public LeaderboardPage()
    {
        this.InitializeComponent();
        _leaderboardViewModel = App.ServiceProvider.GetRequiredService<LeaderboardViewModel>();
        Leaderboard = new ObservableCollection<LeaderboardEntry>();//check here later 
        LeaderboardListView.ItemsSource = Leaderboard;
        CurrentUserRank.Text = $"Your Rank: {_leaderboardViewModel.GetCurrentUserGlobalRank(currentUserId, "Accuracy")}";
    }

    // Event handler for Global button click
    private async void GlobalButton_Click(object sender, RoutedEventArgs e)
    {
        // Update the Leaderboard to display global rankings
        _selectedMode = "Global";
        LeaderboardListView.ItemsSource = await _leaderboardViewModel.GetGlobalLeaderboard("Accuracy");
        CurrentUserRank.Text = $"Your Rank: {await _leaderboardViewModel.GetCurrentUserGlobalRank(currentUserId, "Accuracy")}";
        RankingCriteriaComboBox.SelectedItem = SortBy;
    }

    // Event handler for Friends button click
    private async void FriendsButton_Click(object sender, RoutedEventArgs e)
    {
        // Update the Leaderboard to display friends' rankings
        _selectedMode = "Friends";
        var friendsLeaderboard = await _leaderboardViewModel.GetFriendsLeaderboard(currentUserId, "Accuracy");
        LeaderboardListView.ItemsSource = friendsLeaderboard;
        CurrentUserRank.Text = $"Your Rank: {await _leaderboardViewModel.GetCurrentUserFriendsRank(currentUserId, "Accuracy")}";
        RankingCriteriaComboBox.SelectedItem = SortBy;
    }

    private async void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        // Refresh the Leaderboard
        if (_selectedMode == "Global")
        {
            Leaderboard = new ObservableCollection<LeaderboardEntry>(await _leaderboardViewModel.GetGlobalLeaderboard("Accuracy"));
            CurrentUserRank.Text = $"Your Rank: {_leaderboardViewModel.GetCurrentUserGlobalRank(currentUserId, "Accuracy")}";
        }
        else
        {
            var friendsLeaderboard = await _leaderboardViewModel.GetFriendsLeaderboard(currentUserId, "Accuracy");
            Leaderboard = new ObservableCollection<LeaderboardEntry>(friendsLeaderboard);
            CurrentUserRank.Text = $"Your Rank: {await _leaderboardViewModel.GetCurrentUserFriendsRank(currentUserId, "Accuracy")}";
        }
        LeaderboardListView.ItemsSource = Leaderboard;
        RankingCriteriaComboBox.SelectedItem = SortBy;
    }

    private async void RankingCriteriaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Get the selected item
        var selectedItem = (ComboBoxItem)RankingCriteriaComboBox.SelectedItem;

        if (selectedItem != null)
        {
            string selectedCriteria = selectedItem.Content.ToString();

            switch (selectedCriteria)
            {
                case "Accuracy":
                    if (_selectedMode == "Global")
                    {
                        Leaderboard = new ObservableCollection<LeaderboardEntry>(await _leaderboardViewModel.GetGlobalLeaderboard("Accuracy"));
                        CurrentUserRank.Text = $"Your Rank: {_leaderboardViewModel.GetCurrentUserGlobalRank(currentUserId, "Accuracy")}";
                    }
                    else
                    {
                        var friendsLeaderboard = await _leaderboardViewModel.GetFriendsLeaderboard(currentUserId, "Accuracy");
                        Leaderboard = new ObservableCollection<LeaderboardEntry>(friendsLeaderboard);
                        CurrentUserRank.Text = $"Your Rank: {await _leaderboardViewModel.GetCurrentUserFriendsRank(currentUserId, "Accuracy")}";
                    }
                    LeaderboardListView.ItemsSource = Leaderboard;
                    break;
                
                case "CompletedQuizzes":
                    if (_selectedMode == "Global")
                    {
                        Leaderboard = new ObservableCollection<LeaderboardEntry>(await _leaderboardViewModel.GetGlobalLeaderboard("CompletedQuizzes"));
                        CurrentUserRank.Text = $"Your Rank: {_leaderboardViewModel.GetCurrentUserGlobalRank(currentUserId, "CompletedQuizzes")}";
                    }
                    else
                    {
                        var friendsLeaderboard = await _leaderboardViewModel.GetFriendsLeaderboard(currentUserId, "CompletedQuizzes");
                        Leaderboard = new ObservableCollection<LeaderboardEntry>(friendsLeaderboard);
                        CurrentUserRank.Text = $"Your Rank: {await _leaderboardViewModel.GetCurrentUserFriendsRank(currentUserId, "CompletedQuizzes")}";
                    }
                    LeaderboardListView.ItemsSource = Leaderboard;
                    break;
            }
        }
    }
}
