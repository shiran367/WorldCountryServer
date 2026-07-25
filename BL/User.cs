using BCrypt.Net;


namespace WorldCountry.BL

{
    public class User
    {


        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsLocked { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }



        public User(int userId, string username, string email, string passwordHash, string fullName, DateTime registrationDate, bool isLocked, bool isAdmin, bool isActive)
        {
            UserId = userId;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            FullName = fullName;
            RegistrationDate = registrationDate;
            IsLocked = isLocked;
            IsAdmin = isAdmin;
            IsActive = isActive;
        }


        public User()
        {


        }


        public User Read(int UserId)
        {
            DBservices dbs = new DBservices();

            return dbs.ReadUserById(UserId);
        }


        public int Register()
        {
            DBservices dbs = new DBservices();
            this.PasswordHash = BCrypt.Net.BCrypt.HashPassword(this.PasswordHash);

            return dbs.Register(this);
        }

        public (int UserId, string Username, string Email, bool IsAdmin, bool IsLocked) Login(string email, string password)
        {

            DBservices dbs = new DBservices();


            return dbs.login(email, password);

        }


        public bool UpdateUser(User user)
        {

            DBservices dbs = new DBservices();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            int result = dbs.UpdateUser(user);

            return result > 0;


        }


        public int Logout(int UserId)
        {
            DBservices dbs = new DBservices();
            return dbs.Logout(UserId);
        }


        public List<User> AllUsers()
        {
            DBservices dbs = new DBservices();
            return dbs.AllUsers();
        }

        public int Locked(int UserId)
        {
            DBservices dbs = new DBservices();

          return  dbs.Locked(UserId);
        }
    }
}