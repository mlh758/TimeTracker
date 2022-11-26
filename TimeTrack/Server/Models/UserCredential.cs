using Fido2NetLib;
using Fido2NetLib.Objects;

namespace TimeTrack.Server.Models
{
    public class UserCredential
    {
        public int Id { get; set; }
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
            UserId = userId;
            PublicKey = new byte[0];
            CredentialId = new byte[0];
        }

        public UserCredential(string userId, byte[] publicKey, byte[] credId)
        {
            UserId = userId;
            PublicKey = publicKey;
            CredentialId = credId;
        }
    }
}
