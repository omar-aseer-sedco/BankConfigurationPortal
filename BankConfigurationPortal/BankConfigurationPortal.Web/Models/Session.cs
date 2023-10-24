using BankConfigurationPortal.Web.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Web.Models {
    public class Session {
        [Required]
        [MaxLength(SessionsConstants.USERNAME_SIZE)]
        public string Username { get; set; }

        [Required]
        public int SessionId { get; set; }

        [Required]
        public DateTime Expires { get; set; }

        [Required]
        [MaxLength(SessionsConstants.USER_AGENT_SIZE)]
        public string UserAgent { get; set; }

        [Required]
        [MaxLength(SessionsConstants.IP_ADDRESS_SIZE)]
        public string IpAddress { get; set; }
    }
}
