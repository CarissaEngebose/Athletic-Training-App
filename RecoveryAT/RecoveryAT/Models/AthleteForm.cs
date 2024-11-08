/**
    Name: Carissa Engebose
    Date: 10/27/24
    Description: Class to store athlete form information that will be used throughout the RecoveryAT app.
    Bugs: None that I know of.
    Reflection: The class didn't take very long to create once I looked at my database to see the information
    I wanted to store for each form.
**/

namespace RecoveryAT
{
    // Represents an athlete with relevant information for tracking injuries and status
    public class AthleteForm
    {
        // Unique identifier for the athlete's form 
        public long? FormKey { get; set; }

        // Identifier for the athlete's school code 
        public string SchoolCode { get; set; }

        // Athlete's first name
        public string FirstName { get; set; }

        // Athlete's last name
        public string LastName { get; set; }

        //Full name for ease of access
        public string FullName => $"{FirstName} {LastName}";

        // Athlete's grade (6 - 12)
        public int Grade { get; set; }

        // Sport the athlete participates in
        public string Sport { get; set; }

        // Location of the athlete's injury
        public string InjuredArea { get; set; }

        // Side of the athlete's injury
        public string InjuredSide { get; set; }

        // Athlete's treatment type
        public string TreatmentType { get; set; }

        // Athlete's comments 
        public string ?AthleteComments { get; set; }

        // Trainer's comments 
        public string ?TrainerComments { get; set; }

        // Current status of the athlete (e.g., "Full Contact", "Limited Contact")
        public string ?Status { get; set; }

        // Date the form was submitted
        public DateTime Date { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AthleteForm"/> class with specified details.
        /// </summary>
        /// <param name="schoolCode">The identifier of the athlete's school.</param>
        /// <param name="firstName">The athlete's first name.</param>
        /// <param name="lastName">The athlete's last name.</param>
        /// <param name="fullName">The athlete's full name.</param>
        /// <param name="grade">The athlete's grade (6 - 12).</param>
        /// <param name="sport">The sport the athlete participates in.</param>
        /// <param name="injuredArea">The location of the athlete's injury.</param>
        /// <param name="injuredSide">The side of the athlete's injury.</param>
        /// <param name="treatmentType">The type of treatment the athlete is receiving.</param>
        /// <param name="athleteComments">Comments from the athlete.</param>
        /// <param name="trainerComments">Comments from the trainer.</param>
        /// <param name="status">The current status of the athlete.</param>
        /// <param name="date">The date the form was submitted.</param>
        public AthleteForm(string schoolCode, string firstName, string lastName, int grade,
                            string sport, string injuredArea, string injuredSide, string treatmentType,  DateTime date,
                            string? athleteComments = null, string? trainerComments = null, string? status = null)
        {
            SchoolCode = schoolCode;
            FirstName = firstName;
            LastName = lastName;
            Grade = grade;
            Sport = sport;
            InjuredArea = injuredArea;
            InjuredSide = injuredSide;
            TreatmentType = treatmentType;
            Date = date;
            AthleteComments = athleteComments;
            TrainerComments = trainerComments;
            Status = status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AthleteForm"/> class with specified details.
        /// </summary>
        /// <param name="formKey">The identifier of the athlete's form (if it has already been created).</param>
        /// <param name="schoolCode">The identifier of the athlete's school.</param>
        /// <param name="firstName">The athlete's first name.</param>
        /// <param name="lastName">The athlete's last name.</param>
        /// <param name="grade">The athlete's grade (6 - 12).</param>
        /// <param name="sport">The sport the athlete participates in.</param>
        /// <param name="injuredArea">The location of the athlete's injury.</param>
        /// <param name="injuredSide">The side of the athlete's injury.</param>
        /// <param name="treatmentType">The type of treatment the athlete is receiving.</param>
        /// <param name="athleteComments">Comments from the athlete.</param>
        /// <param name="trainerComments">Comments from the trainer.</param>
        /// <param name="status">The current status of the athlete.</param>
        /// <param name="date">The date the form was submitted.</param>
        public AthleteForm(long formKey, string schoolCode, string firstName, string lastName, int grade,
                            string sport, string injuredArea, string injuredSide, string treatmentType,  DateTime date,
                            string? athleteComments = null, string? trainerComments = null, string? status = null)
        {
            FormKey = formKey;
            SchoolCode = schoolCode;
            FirstName = firstName;
            LastName = lastName;
            Grade = grade;
            Sport = sport;
            InjuredArea = injuredArea;
            InjuredSide = injuredSide;
            TreatmentType = treatmentType;
            Date = date;
            AthleteComments = athleteComments;
            TrainerComments = trainerComments;
            Status = status;
        }

        // ONLY USED FOR TESTING PURPOSES - WILL REMOVE WHEN DATABASE IS IN EFFECT FOR ATHLETE STATUSES
        public AthleteForm(string firstName, string lastName, string sport, string injuredArea, string status)
        {
            SchoolCode = "12345";
            FirstName = firstName;
            LastName = lastName;
            Sport = sport;
            InjuredArea = injuredArea;
            Status = status;
            InjuredSide = "Left";
            TreatmentType = "Tape";
        }
    }
}
