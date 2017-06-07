using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiberDAMM.Models
{
    public class ContactType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Kontakttyp")]
        public string Name { get; set; }

        public virtual ICollection<ContactData> ContactData { get; set; }
        public object Client { get; internal set; }
    }
}