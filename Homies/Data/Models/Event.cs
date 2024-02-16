using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homies.Data.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ValidationConstants.NameMaxLength)]
        [MinLength(ValidationConstants.ValidationConstants.NameMinLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ValidationConstants.DescriptionMaxLength)]
        [MinLength(ValidationConstants.ValidationConstants.DescriptionMinLength)]
        public string Description { get; set; }


        [Required]
        public string OrganiserId { get; set; }
        [Required]
        [ForeignKey(nameof(OrganiserId))]
        public IdentityUser Organiser { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public int TypeId { get; set; }
        [ForeignKey(nameof(TypeId))]
        public Type Type { get; set; }

        public IEnumerable<EventParticipant> EventsParticipants { get; set; }
            = new List<EventParticipant>();
    }
}