using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RecoveryAT.ViewModels
{
    public partial class AthleteContactsViewModel : ObservableObject
    {
        //properties to store needed data
        [ObservableProperty]
        private string phoneNumber;

        [ObservableProperty]
        private string relationship;

        [RelayCommand]
        private void finish()
        {
            // Implement finish logic
        }

        [RelayCommand]
        private void garbage()
        {
            // Implement garbage logic
            PhoneNumber = string.Empty;
            Relationship = string.Empty;
        }

        [RelayCommand]
        private void back()
        {
            // Implement back logic
        }

        [RelayCommand]
        private void add()
        {
            // implement second row
        }
    }
}