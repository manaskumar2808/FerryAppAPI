using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace FerryAppApi.Models
{
    public partial class ManasUser
    {
        public ManasUser()
        {
            ManasTickets = new HashSet<ManasTicket>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string UserName { get; set; }

        [Required]
        [MinLength(4)]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [Required]
        public double Wallet { get; set; }

        public virtual ICollection<ManasTicket> ManasTickets { get; set; }
    }
}
