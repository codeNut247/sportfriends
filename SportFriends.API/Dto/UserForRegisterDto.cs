using System.ComponentModel.DataAnnotations;

namespace SportFriends.API.Dto
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(16, MinimumLength=4, ErrorMessage="You must specify a password between 4 and 16.")]
        public string Password { get; set; }
    }
}