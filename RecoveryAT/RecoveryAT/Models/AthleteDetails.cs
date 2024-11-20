using System;

namespace RecoveryAT
{
    /// <summary>
    /// Represents a combined view of athlete details, including contact information.
    /// </summary>
    public class AthleteDetail
    {
        /// <summary>
        /// Athlete's full name (from AthleteForm).
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Relationship of the contact to the athlete (from AthleteContact).
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// Phone number of the contact (from AthleteContact).
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Type of treatment the athlete is receiving (from AthleteForm).
        /// </summary>
        public string TreatmentType { get; set; }

        /// <summary>
        /// Comments from the athlete (from AthleteForm).
        /// </summary>
        public string AthleteComments { get; set; }

        /// <summary>
        /// Athlete's date of birth (from AthleteForm).
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AthleteDetail"/> class with specified details.
        /// </summary>
        /// <param name="fullName">Full name of the athlete.</param>
        /// <param name="relationship">Relationship of the contact to the athlete.</param>
        /// <param name="phoneNumber">Phone number of the contact.</param>
        /// <param name="treatmentType">Type of treatment the athlete is receiving.</param>
        /// <param name="dateOfBirth">Athlete's date of birth.</param>
        public AthleteDetail(string fullName, string relationship, string phoneNumber, string treatmentType, string athleteComments, DateTime dateOfBirth)
        {
            FullName = fullName;
            Relationship = relationship;
            PhoneNumber = phoneNumber;
            TreatmentType = treatmentType;
            AthleteComments = athleteComments;
            DateOfBirth = dateOfBirth;
        }

        /// <summary>
        /// Creates an <see cref="AthleteDetail"/> instance by combining data from an <see cref="AthleteForm"/> and <see cref="AthleteContact"/>.
        /// </summary>
        /// <param name="form">The athlete's form containing personal and injury details.</param>
        /// <param name="contact">The athlete's contact information.</param>
        /// <returns>An <see cref="AthleteDetail"/> instance combining the form and contact data.</returns>
        public static AthleteDetail FromFormAndContact(AthleteForm form, AthleteContact contact)
        {
            return new AthleteDetail(
                fullName: form.FullName,
                relationship: contact.ContactType,
                phoneNumber: contact.PhoneNumber,
                treatmentType: form.TreatmentType,
                athleteComments: form.AthleteComments,
                dateOfBirth: form.DateOfBirth
            );
        }
    }
}
