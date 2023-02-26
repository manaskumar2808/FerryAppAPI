using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace FerryAppApi.Models
{
    public partial class ManasFerry
    {
        public ManasFerry()
        {
            ManasTickets = new HashSet<ManasTicket>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Rooms Count is required!")]
        [Range(0, 4000)]
        public int RoomsLeft { get; set; }

        [Required(ErrorMessage = "Charge is required!")]
        public double Charge { get; set; }
        public string Image { get; set; }
        public DateTime Departure { get; set; }

        [Required(ErrorMessage = "Origin ID is necessary!")]
        [ForeignKey("OriginId")]
        public int OriginId { get; set; }

        [Required(ErrorMessage = "Destination ID is necessary!")]
        [ForeignKey("DestinationId")]
        public int DestinationId { get; set; }

        public virtual ManasPort Destination { get; set; }
        public virtual ManasPort Origin { get; set; }
        public virtual ICollection<ManasTicket> ManasTickets { get; set; }
    }
}
