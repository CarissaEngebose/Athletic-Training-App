using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace RecoveryAT.ViewModels{
    public partial class AthleteInformationViewModel : ObservableObject{
        [ObservableProperty]
        String searchInput;

        [ObservableProperty]
        ObservableCollection<Athlete> athleteList = new ObservableCollection<Athlete>
            {
                new Athlete { Date = "2024-10-01", Name = "John Smith", Relationship = "Mother", PhoneNumber = "(123) 456-7890", FormNumber = "12" },
                new Athlete { Date = "2024-09-22", Name = "Reece Thomas", Relationship = "Mother", PhoneNumber = "(555) 111-2222", FormNumber = "11" },
                new Athlete { Date = "2024-09-15", Name = "Marcus Rye", Relationship = "Guardian", PhoneNumber = "(111) 222-3333", FormNumber = "9" }
            };

        [RelayCommand]
        void searchButtonClicked(){
            // implement search function
        }

        //!!!!!!!!! FIX !!!!!!!!
        [RelayCommand]
        void menuButtonClicked()
        {
            //IsPresented = true; // this still works for some reason and I dont know why
        }

        
    }
}