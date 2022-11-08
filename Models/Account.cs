using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebRazor.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public int? Role { get; set; }
        [JsonIgnore]
        public virtual Customer? Customer { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
