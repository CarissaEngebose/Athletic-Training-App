namespace RecoveryAT
{
    public partial class TrainerSchoolInformation : ContentPage
    {
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _email;
        private readonly string _hashedPassword;
        private readonly string _hashedSecurityQuestions;

        public TrainerSchoolInformation(string firstName, string lastName, string email, string hashedPassword, string hashedSecurityQuestions)
        {
            InitializeComponent();

            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _hashedPassword = hashedPassword;
            _hashedSecurityQuestions = hashedSecurityQuestions;

            // Attach TextChanged event handlers to move between entry boxes
            AttachTextChangedHandlers();
        }

        private void AttachTextChangedHandlers()
        {
            CodeEntry1.TextChanged += (s, e) => MoveToNextEntry(CodeEntry1, CodeEntry2);
            CodeEntry2.TextChanged += (s, e) => MoveToNextEntry(CodeEntry2, CodeEntry3);
            CodeEntry3.TextChanged += (s, e) => MoveToNextEntry(CodeEntry3, CodeEntry4);
            CodeEntry4.TextChanged += (s, e) => MoveToNextEntry(CodeEntry4, CodeEntry5);
        }

        private void MoveToNextEntry(Entry current, Entry next)
        {
            // Move focus to the next entry when current entry is filled
            if (current.Text?.Length == current.MaxLength)
            {
                next.Focus();
            }
        }

        private async void OnCreateSchoolClicked(object sender, EventArgs e)
        {
            // Retrieve school name and individual parts of the school code
            var codePart1 = CodeEntry1.Text;
            var codePart2 = CodeEntry2.Text;
            var codePart3 = CodeEntry3.Text;
            var codePart4 = CodeEntry4.Text;
            var codePart5 = CodeEntry5.Text;

            // Validate school name and code parts
            if (string.IsNullOrWhiteSpace(schoolNameEntry.Text) ||
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

            var (key, iv) = EncryptionHelper.GenerateKeyAndIV(); // get the key to encrypt the school name
            var encryptedSchoolName = EncryptionHelper.Encrypt(schoolNameEntry.Text, key, iv);

            // Create the user account in the database
            var resultMessage = MauiProgram.BusinessLogic.InsertUser(_firstName, _lastName, _email, _hashedPassword, encryptedSchoolName, schoolCode, key, iv, _hashedSecurityQuestions);

            // Notify user of success or failure
            await DisplayAlert("Result", resultMessage, "OK");

            try
            {
                if (resultMessage == "User account created successfully.")
                {
                    await Navigation.PushModalAsync(new MainTabbedPage());
                }
            }
            catch (Exception ex)
            {
                // bug fix for navigation error after going back to login - dominick
                await Navigation.PopToRootAsync(); // go back to the first page
            }
        }

        // Method to concatenate parts of the school code
        private string ConcatSchoolCode(string part1, string part2, string part3, string part4, string part5)
        {
            return string.Concat(part1?.Trim(), part2?.Trim(), part3?.Trim(), part4?.Trim(), part5?.Trim());
        }
    }
}
