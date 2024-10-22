using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace RecoveryAT.ViewModels{
    public partial class AthletePastFormsViewModel : ObservableObject{
        [ObservableProperty]
        String selectedStatus;

        [ObservableProperty]
        String searchEntry;

        [ObservableProperty]
        ObservableCollection<Form> formList = new ObservableCollection<Form>
            {
                new Form { Date = "2024-10-01", Name = "John Smith", Sport = "Soccer", Injury = "Ankle", FormNumber = "F001" },
                new Form { Date = "2024-09-22", Name = "Reece Thomas", Sport = "Tennis", Injury = "Shoulder", FormNumber = "F002" },
                new Form { Date = "2024-09-15", Name = "Marcus Rye", Sport = "Football", Injury = "Knee", FormNumber = "F003" },
                new Form { Date = "2024-08-30", Name = "Sophia Lee", Sport = "Basketball", Injury = "Wrist", FormNumber = "F004" },
                new Form { Date = "2024-08-10", Name = "Lucas Brown", Sport = "Soccer", Injury = "Back", FormNumber = "F005" }
            };

        [RelayCommand]
        void menuButtonClicked()
        {
            //implement menu flyout
        }
    }

    public class Form
    {
        public string Date { get; set; } // Date when the form was created
        public string Name { get; set; } // Athlete's name
        public string Sport { get; set; } // Sport the athlete plays
        public string Injury { get; set; } // Type of injury the athlete has
        public string FormNumber { get; set; } // Unique identifier for the form
    }
}