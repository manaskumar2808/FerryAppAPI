using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace FerryAppApi.Models
{
    public partial class ManasPort
    {
        public ManasPort()
        {
            ManasFerryDestinations = new HashSet<ManasFerry>();
            ManasFerryOrigins = new HashSet<ManasFerry>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        [MinLength(2)]
        public string Name { get; set; }
        public string Image { get; set; }

        public virtual ICollection<ManasFerry> ManasFerryDestinations { get; set; }
        public virtual ICollection<ManasFerry> ManasFerryOrigins { get; set; }
    }
}
