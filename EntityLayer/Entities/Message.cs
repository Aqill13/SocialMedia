using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public AppUser Sender { get; set; }
        public string ReceiverId { get; set; }
        public AppUser Receiver { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
