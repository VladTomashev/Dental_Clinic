namespace Dental_Clinic.entity
{
    public class User
    {
        public long id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string role { get; set; }

        public long? userEntityId { get; set; }

        public RefToken? refreshToken { get; set; }

    }
}
