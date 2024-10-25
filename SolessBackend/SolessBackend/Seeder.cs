using SolessBackend.Data;
using SolessBackend.Models;

namespace SolessBackend
{
    public class Seeder
    {
        private readonly DataBaseContext _context;

        public Seeder(DataBaseContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            User[] users =
            [
                new User
                {
                    Name = "Huevo",
                    Email = "huevo@.com",
                    Password = "password",
                },
                new User
                {
                    Name = "Pollo",
                    Email = "pollo@.com",
                    Password = "password2",
                }
            ];

            _context.Users.AddRange( users );
            _context.SaveChanges();
        }
    }
}
