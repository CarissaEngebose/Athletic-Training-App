/**
    Date: 10/14/2024
    Description: Allows the trainer to enter their school name and school code.
    Bugs: None Known
    Reflection: This was a very easy screen to implement. The only challenge was lining up the 5 boxes for the 5-digit code so that it looked nice and closer to the prototype.
**/

namespace RecoveryAT
{
    public partial class TrainerSchoolInformation : ContentPage
    {
        // Private fields to store trainer account information
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _email;
        private readonly string _hashedPassword;
        private readonly string _hashedSecurityQuestions;

        /// <summary>
        /// Constructor initializes the trainer information and attaches event handlers.
        /// </summary>
        /// <param name="firstName">Trainer's first name.</param>
        /// <param name="lastName">Trainer's last name.</param>
        /// <param name="email">Trainer's email address.</param>
        /// <param name="hashedPassword">Hashed password for the account.</param>
        /// <param name="hashedSecurityQuestions">Hashed security questions for the account.</param>
        public TrainerSchoolInformation(string firstName, string lastName, string email, string hashedPassword, string hashedSecurityQuestions)
        {
            InitializeComponent();

            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _hashedPassword = hashedPassword;
            _hashedSecurityQuestions = hashedSecurityQuestions;

            // Attach event handlers for moving focus between code entry fields
            AttachTextChangedHandlers();
        }

        /// <summary>
        /// Attaches event handlers to the code entry fields for managing focus transitions.
        /// </summary>
        private void AttachTextChangedHandlers()
        {
            CodeEntry1.TextChanged += (s, e) => MoveToNextEntry(CodeEntry1, CodeEntry2);
            CodeEntry2.TextChanged += (s, e) => MoveToNextEntry(CodeEntry2, CodeEntry3);
            CodeEntry3.TextChanged += (s, e) => MoveToNextEntry(CodeEntry3, CodeEntry4);
            CodeEntry4.TextChanged += (s, e) => MoveToNextEntry(CodeEntry4, CodeEntry5);
        }

        /// <summary>
        /// Moves focus to the next entry field when the current field is filled.
        /// </summary>
        /// <param name="current">The current entry field.</param>
        /// <param name="next">The next entry field.</param>
        private void MoveToNextEntry(Entry current, Entry next)
        {
            if (current.Text?.Length == current.MaxLength)
            {
                next.Focus(); // Move focus to the next entry
            }
        }

        /// <summary>
        /// Handles the "Create School" button click. Validates and processes the entered information.
        /// </summary>
        private async void OnCreateSchoolClicked(object sender, EventArgs e)
        {
            // Retrieve school name and code parts from the input fields
            var codePart1 = CodeEntry1.Text;
            var codePart2 = CodeEntry2.Text;
            var codePart3 = CodeEntry3.Text;
            var codePart4 = CodeEntry4.Text;
            var codePart5 = CodeEntry5.Text;

            // Validate that all fields are filled
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

            // Concatenate the school code parts into a full code
            var schoolCode = ConcatSchoolCode(codePart1, codePart2, codePart3, codePart4, codePart5);

            // Check if the school code already exists
            if (MauiProgram.BusinessLogic.SchoolCodeExists(schoolCode))
            {
                await DisplayAlert("Error", "This school code already exists. Please enter a unique code.", "OK");
                return;
            }

            // Encrypt the school name using a generated key and IV
            var (key, iv) = EncryptionHelper.GenerateKeyAndIV();
            var encryptedSchoolName = EncryptionHelper.Encrypt(schoolNameEntry.Text, key, iv);

            // Create the user account in the database
            var resultMessage = MauiProgram.BusinessLogic.InsertUser(_firstName, _lastName, _email, _hashedPassword, encryptedSchoolName, schoolCode, key, iv, _hashedSecurityQuestions);

            // Notify the user of the result
            await DisplayAlert("Result", resultMessage, "OK");

            // Navigate to the main page or handle navigation errors
            try
            {
                if (resultMessage == "User account created successfully.")
                {
                    await Navigation.PushModalAsync(new MainTabbedPage());
                }
            }
            catch (Exception ex)
            {
                await Navigation.PopToRootAsync(); // Fallback navigation in case of error
            }
        }

        /// <summary>
        /// Concatenates the parts of the school code into a single string.
        /// </summary>
        /// <param name="part1">First part of the code.</param>
        /// <param name="part2">Second part of the code.</param>
        /// <param name="part3">Third part of the code.</param>
        /// <param name="part4">Fourth part of the code.</param>
        /// <param name="part5">Fifth part of the code.</param>
        /// <returns>Concatenated school code as a single string.</returns>
        private string ConcatSchoolCode(string part1, string part2, string part3, string part4, string part5)
        {
            return string.Concat(part1?.Trim(), part2?.Trim(), part3?.Trim(), part4?.Trim(), part5?.Trim());
        }
    }
}
