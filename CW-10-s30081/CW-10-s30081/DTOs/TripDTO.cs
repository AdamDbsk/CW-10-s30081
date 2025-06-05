
using CW_10_s30081.Models;
using System.Collections.ObjectModel;

namespace CW_10_s30081.DTOs {
    public class TripPagedGetDTO {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int AllPages { get; set; }
        public ICollection<TripWithClientsAndCountiresGetDTO> TripsInPage { get; set; }
    }
    public class TripWithClientsAndCountiresGetDTO {
        public int IdTrip { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int MaxPeople { get; set; }

        public ICollection<ShortClientGetDTO> Clients { get; set; }
        public ICollection<string> Countries { get; set; }

    }
}
