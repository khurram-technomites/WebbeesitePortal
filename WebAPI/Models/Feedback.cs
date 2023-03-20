using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Feedback
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public float Rating { get; set; }
        public string Message { get; set; }
        public bool IsApproved { get; set; }
        public AppUser User { get; set; }
    }
}
