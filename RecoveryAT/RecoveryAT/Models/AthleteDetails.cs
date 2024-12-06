/**
    Date: 12/05/24
    Description: Class to store athlete details taken from the AthleteForm and AthleteContact classes as an easy way to see
    all the athlete's details in one place.
    Bugs: None that we know of.
    Reflection: This didn't take very long and was pretty straightforward to set up. It was very beneficial for the AthleteInformation screen
    where we needed to display all the information of the athlete from various tables in the database.
**/
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
                fullName: form.FullName ?? "Unknown Athlete",
                relationship: contact.ContactType ?? "Unknown Contact",
                phoneNumber: contact.PhoneNumber ?? "No Phone Number",
                treatmentType: form.TreatmentType ?? "Unknown Treatment",
                athleteComments: form.AthleteComments ?? "No Comments",
                dateOfBirth: form.DateOfBirth != default ? form.DateOfBirth : DateTime.MinValue
            );
        }

    }
}
