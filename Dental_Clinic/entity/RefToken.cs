namespace Dental_Clinic.entity
{
    public class RefToken : AbstractEntity
    {
        public string? token { get; set; }
        public DateTime? lifeTime { get; set; }
        public User? user { get; set; }

        public RefToken() { }
        public RefToken(long id, string? token, DateTime? lifeTime, User? user)
        {
            this.id = id;
            this.token = token;
            this.lifeTime = lifeTime;
            this.user = user;
        }



    }
}
