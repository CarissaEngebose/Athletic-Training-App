using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace RecoveryAT.ViewModels{
    public partial class AthleteStatusesViewModel : ObservableObject{
        [ObservableProperty]
        String selectedStatus;

        [ObservableProperty]
        String searchEntry;

        [ObservableProperty]
        ObservableCollection<Athlete> athleteList = new ObservableCollection<Athlete>
            {
                new Athlete { Name = "John Smith", Injury = "Ankle", Sport = "Soccer", Status = "Total Rest" },
                new Athlete { Name = "Reece Thomas", Injury = "Shoulder", Sport = "Tennis", Status = "Limited Contact" },
                new Athlete { Name = "Marcus Rye", Injury = "Knee", Sport = "Football", Status = "Activity as Tolerated" },
                new Athlete { Name = "Sophia Lee", Injury = "Wrist", Sport = "Basketball", Status = "Total Rest" },
                new Athlete { Name = "Lucas Brown", Injury = "Back", Sport = "Soccer", Status = "Limited Contact" }
            };

        [RelayCommand]
        void menuButtonClicked()
        {
            //implement menu flyout
        }

        [RelayCommand]
        void Search(){
            //implement search functionality
        }
    }
}