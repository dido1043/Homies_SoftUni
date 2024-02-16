using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homies.Data.Models
{
    public class EventParticipant
    {

        public string HelperId { get; set; }
        [ForeignKey(nameof(HelperId))]
        public IdentityUser Helper { get; set; }
        public int EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; }
    }
}