/**
    Date: 12/06/24
    Description: Class to store athlete contact information such as the contact ID, form key, contact type, and phone number.
    Bugs: None that we know of.
    Reflection: This didn't take very long and was pretty straightforward to set up.
**/

namespace RecoveryAT
{
    /// <summary>
    /// Represents an athlete contact with relevant information for the athlete
    /// </summary>
    public class AthleteContact
    {
        /// <summary>
        /// Unique identifier for the contact
        /// </summary>
        public long ContactID { get; set; }

        /// <summary>
        /// Unique identifier for the athlete's form 
        /// </summary>
        public long FormKey { get; set; }

        /// <summary>
        /// Identifier for the contact's relationship to the athlete
        /// </summary> 
        public string ContactType { get; set; }

        /// <summary>
        /// Identifier for the contact's phone number
        /// </summary> 
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