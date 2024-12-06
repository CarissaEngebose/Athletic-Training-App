/**
    Date: 12/06/24
    Description: Class to store athlete form information that will be used throughout the RecoveryAT app.
    Bugs: None that we know of.
    Reflection: The class didn't take very long to create once we looked at the database to see the information
    we wanted to store for each form.
**/

namespace RecoveryAT
{
    /// <summary>
    /// Represents an athlete's form with relevant information for tracking injuries and status.
    /// </summary>
    public class AthleteForm
    {
        /// <summary>
        /// Unique identifier for the athlete's form.
        /// </summary>
        public long? FormKey { get; set; }

        /// <summary>
        /// Identifier for the athlete's school code.
        /// </summary>
        public string SchoolCode { get; set; }

        /// <summary>
        /// Athlete's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Athlete's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Full name for display purposes.
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Athlete's sport.
        /// </summary>
        public string Sport { get; set; }

        /// <summary>
        /// Location of the athlete's injury.
        /// </summary>
        public string InjuredArea { get; set; }

        /// <summary>
        /// Side of the athlete's injury.
        /// </summary>
        public string InjuredSide { get; set; }

        /// <summary>
        /// Athlete's treatment type.
        /// </summary>
        public string TreatmentType { get; set; }

        /// <summary>
        /// Athlete's comments.
        /// </summary>
        public string? AthleteComments { get; set; }

        /// <summary>
        /// Current status of the athlete (e.g., "Full Contact", "Limited Contact").
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Date the form was created.
        /// </summary>
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Date the athlete was seen.
        /// </summary>
        public DateTime? DateSeen { get; set; }

        /// <summary>
        /// Athlete's date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AthleteForm"/> class with full details.
        /// </summary>
        /// <param name="formKey">Unique identifier for the form (nullable for new forms).</param>
        /// <param name="schoolCode">Identifier for the athlete's school.</param>
        /// <param name="firstName">Athlete's first name.</param>
        /// <param name="lastName">Athlete's last name.</param>
        /// <param name="sport">Athlete's sport.</param>
        /// <param name="injuredArea">Location of the athlete's injury.</param>
        /// <param name="injuredSide">Side of the athlete's injury.</param>
        /// <param name="treatmentType">Type of treatment the athlete is receiving.</param>
        /// <param name="dateCreated">Date the form was created.</param>
        /// <param name="dateSeen">Date the athlete was seen (optional).</param>
        /// <param name="dateOfBirth">Athlete's date of birth.</param>
        /// <param name="athleteComments">Comments from the athlete (optional).</param>
        /// <param name="status">Current status of the athlete (optional).</param>
        public AthleteForm(long? formKey, string schoolCode, string firstName, string lastName, string sport,
                           string injuredArea, string injuredSide, string treatmentType, DateTime dateCreated,
                           DateTime? dateSeen, DateTime dateOfBirth, string? athleteComments = null,
                           string? status = null)
        {
            FormKey = formKey;
            SchoolCode = schoolCode;
            FirstName = firstName;
            LastName = lastName;
            Sport = sport;
            InjuredArea = injuredArea;
            InjuredSide = injuredSide;
            TreatmentType = treatmentType;
            DateCreated = dateCreated;
            DateSeen = dateSeen;
            DateOfBirth = dateOfBirth;
            AthleteComments = athleteComments;
            Status = status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AthleteForm"/> class for creating a new form.
        /// </summary>
        /// <param name="schoolCode">Identifier for the athlete's school.</param>
        /// <param name="firstName">Athlete's first name.</param>
        /// <param name="lastName">Athlete's last name.</param>
        /// <param name="sport">Athlete's sport.</param>
        /// <param name="injuredArea">Location of the athlete's injury.</param>
        /// <param name="injuredSide">Side of the athlete's injury.</param>
        /// <param name="treatmentType">Type of treatment the athlete is receiving.</param>
        /// <param name="dateOfBirth">Athlete's date of birth.</param>
        /// <param name="athleteComments">Comments from the athlete (optional).</param>
        /// <param name="status">Current status of the athlete (optional).</param>
        public AthleteForm(string schoolCode, string firstName, string lastName, string sport, string injuredArea,
                           string injuredSide, string treatmentType, DateTime dateOfBirth, string? athleteComments = null,
                           string? status = null)
        {
            SchoolCode = schoolCode;
            FirstName = firstName;
            LastName = lastName;
            Sport = sport;
            InjuredArea = injuredArea;
            InjuredSide = injuredSide;
            TreatmentType = treatmentType;
            DateCreated = DateTime.Now; // Default to current date
            DateOfBirth = dateOfBirth;
            AthleteComments = athleteComments;
            Status = status;
        }
    }
}
