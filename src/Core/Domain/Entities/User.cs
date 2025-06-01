namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RegisterCode { get; set; }
        public bool isActive{ get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
