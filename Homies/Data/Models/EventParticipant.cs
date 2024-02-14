using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homies.Data.Models
{
    public class EventParticipant
    {
        [ForeignKey(nameof(HelperId))]
        public int HelperId { get; set; }
        public IdentityUser Helper { get; set; }

        [ForeignKey(nameof(EventId))]
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
