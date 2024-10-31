/**
    Name: Carissa Engebose
    Date: 10/27/24
    Description: Class to store athlete contact information such as the contact ID, form key, contact type, and phone number.
    Bugs: None that I know of.
    Reflection: This didn't take very long and was pretty straightforward to set up.
**/

namespace RecoveryAT
{
    // Represents an athlete with relevant information for tracking injuries and status
    public class AthleteContact
    {
        // Unique identifier for the athlete's form 
        public long ContactID { get; set; }

        // Unique identifier for the athlete's form 
        public long FormKey { get; set; }

        // Identifier for the contact's relationship to the athlete
        public string ContactType { get; set; }

        // Identifier for the contact's phone number
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AthleteContact"/> class with specified details.
        /// </summary>
        /// <param name="contactID">The identifier of the contact's ID.</param>
        /// <param name="formKey">The identifier of the athlete's form.</param>
        /// <param name="contactType">The relationship of the contact to the athlete.</param>
        /// <param name="phoneNumber">The phone number of the contact.</param>
        public AthleteContact(long contactID, long formKey, string contactType, string phoneNumber)
        {
            ContactID = contactID;
            FormKey = formKey;
            ContactType = contactType;
            PhoneNumber = phoneNumber;
        }
    }
}