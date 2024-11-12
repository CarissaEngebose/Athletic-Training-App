/*
    Name: Luke Kastern
    Date: 10/14/2024
    Description: TrainerSchoolInformation Screen
    Bugs: None known
    Reflection: This was a very easy screen to implement. The only challenge was aligning the 5 boxes 
                for the 5-digit code to make it look nice and closer to the prototype.
*/

namespace RecoveryAT
{
    public partial class TrainerSchoolInformation : ContentPage
    {
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _email;
        private readonly string _hashedPassword;

        public TrainerSchoolInformation(string firstName, string lastName, string email, string hashedPassword)
        {
            InitializeComponent();

            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _hashedPassword = hashedPassword;
        }

        private async void OnCreateSchoolClicked(object sender, EventArgs e)
        {
            // Retrieve school name and individual parts of the school code
            var schoolName = schoolNameEntry.Text;
            var codePart1 = CodeEntry1.Text;
            var codePart2 = CodeEntry2.Text;
            var codePart3 = CodeEntry3.Text;
            var codePart4 = CodeEntry4.Text;
            var codePart5 = CodeEntry5.Text;

            // Validate school name and code parts
            if (string.IsNullOrWhiteSpace(schoolName) || 
                string.IsNullOrWhiteSpace(codePart1) || 
                string.IsNullOrWhiteSpace(codePart2) || 
                string.IsNullOrWhiteSpace(codePart3) || 
                string.IsNullOrWhiteSpace(codePart4) || 
                string.IsNullOrWhiteSpace(codePart5))
            {
                await DisplayAlert("Error", "Please enter all school information.", "OK");
                return;
            }

            // Concatenate the parts to form the full school code
            var schoolCode = ConcatSchoolCode(codePart1, codePart2, codePart3, codePart4, codePart5);

            // Check if the school code already exists in the database
            if (MauiProgram.BusinessLogic.SchoolCodeExists(schoolCode))
            {
                await DisplayAlert("Error", "This school code already exists. Please enter a unique code.", "OK");
                return;
            }

            // Create the user account in the database
            var resultMessage = MauiProgram.BusinessLogic.InsertUser(_firstName, _lastName, _email, _hashedPassword, schoolName, schoolCode);

            // Notify user of success or failure
            await DisplayAlert("Result", resultMessage, "OK");

            // Optionally, navigate to another page if account creation is successful
            if (resultMessage == "User account created successfully.")
            {
                await Navigation.PushModalAsync(new MainTabbedPage(schoolCode));
            }
        }

        // Method to concatenate parts of the school code
        private string ConcatSchoolCode(string part1, string part2, string part3, string part4, string part5)
        {
            return string.Concat(part1, part2, part3, part4, part5);
        }
    }
}
