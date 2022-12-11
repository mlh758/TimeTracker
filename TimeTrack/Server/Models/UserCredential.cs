using Fido2NetLib;
using Fido2NetLib.Objects;
using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Server.Models
{
    public class UserCredential
    {
        [MaxLength(64, ErrorMessage = "Must be less than 64 characters")]
        [Required]
        public string Name { get; set; }
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public User? User { get; set; }
        public byte[] PublicKey { get; set; }
        // unique ID for the credential provided by the browser
        public byte[] CredentialId { get; set; }
        public uint SignatureCounter { get; set; }
        public DateTime RegDate { get; set; }
        public Guid AaGuid { get; set; }

        public UserCredential(string userId)
        {
            Name = "";
            UserId = userId;
            PublicKey = Array.Empty<byte>();
            CredentialId = Array.Empty<byte>();
        }

        public UserCredential(string userId, byte[] publicKey, byte[] credId)
        {
            Name = "";
            UserId = userId;
            PublicKey = publicKey;
            CredentialId = credId;
        }
    }
}
