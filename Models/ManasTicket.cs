using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace FerryAppApi.Models
{
    public partial class ManasTicket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "No. of Adults is required!")]
        [Range(1, 10)]
        public int AdultCount { get; set; }

        [Required(ErrorMessage = "Cost is required for a ticket!")]
        public double Cost { get; set; }

        [Required(ErrorMessage = "Room No. is required to be assigned to a ticket!")]
        [Range(1, 4000)]
        public int RoomNo { get; set; }

        [Required(ErrorMessage = "User ID is required!")]
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Ferry ID is required!")]
        [ForeignKey("FerryId")]
        public int FerryId { get; set; }

        public virtual ManasFerry Ferry { get; set; }
        public virtual ManasUser User { get; set; }
    }
}
