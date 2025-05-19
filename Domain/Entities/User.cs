namespace Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }

        public string PasswordHash { get; private set; }

        public User(string name, string email, string role, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("User name cannot be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("User email cannot be empty", nameof(email));
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("User role cannot be empty", nameof(role));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

            Name = name;
            Email = email;
            Role = role;
            PasswordHash = passwordHash;
        }

        public void UpdateEmail(string newEmail)
        {
            Email = newEmail;
        }
    }
}
