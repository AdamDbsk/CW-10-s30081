namespace CW_10_s30081.DTOs {
    public class ShortClientGetDTO {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
    public class TripReservationPostDTO {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Pesel { get; set; }
        public int TripID { get; set; }
        public string TripName { get; set; }
        
        public DateTime? PaymentDate { get; set; }
    }
}
