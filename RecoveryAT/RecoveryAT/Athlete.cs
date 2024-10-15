namespace RecoveryAT
{
    // Represents an athlete with relevant information for tracking injuries and status
    public class Athlete
    {
        // Date associated with the athlete's record (e.g., injury or form submission date)
        public string Date { get; set; }

        // Athlete's name
        public string Name { get; set; }

        // Description of the athlete's injury
        public string Injury { get; set; }

        // Sport the athlete participates in
        public string Sport { get; set; }

        // Current status of the athlete (e.g., "Full Contact", "Limited Contact")
        public string Status { get; set; }

        // Relationship of the emergency contact (e.g., "Parent", "Guardian")
        public string Relationship { get; set; }

        // Emergency contact phone number
        public string PhoneNumber { get; set; }

        // Unique identifier for the athlete's form (could be useful for tracking purposes)
        public string FormNumber { get; set; }
    }
}
