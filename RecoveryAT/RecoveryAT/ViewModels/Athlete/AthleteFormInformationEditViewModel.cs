using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace RecoveryAT.ViewModels{
    public partial class AthleteFormInformationEditViewModel : ObservableObject{
        [ObservableProperty]
        String name = "Jane Joe";

        [ObservableProperty]
        String grade = "9";

        [ObservableProperty]
        String sport = "Volleyball";

        [ObservableProperty]
        String injury = "Stubbed Toe";

        [ObservableProperty]
        String athleteComments = "My right shoulder has been bothering me, and last night after the volleyball game, I felt a sharp pain that won't go away.";

        [ObservableProperty]
        String trainerComments = "Rest and ice the shoulder. If pain persists, consider a visit to the doctor.";

        [ObservableProperty]
        String athleteStatus = "Under observation; no activity until cleared by a trainer.";

        [RelayCommand]
        void back(){
            // implement functionality to go back
        }

        [RelayCommand]
        void saveChanges(){
            // implement functionality to go back
        }
    }
}