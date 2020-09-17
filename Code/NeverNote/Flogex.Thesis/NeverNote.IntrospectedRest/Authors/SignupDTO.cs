using System;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Authors
{
    public class SignupDTO
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public string UserName { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public DateTime? BirthDate { get; set; }
    }
}
