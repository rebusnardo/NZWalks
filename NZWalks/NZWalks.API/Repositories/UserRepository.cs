using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NZWalksDbContext nZWalksDbcontext;

        public UserRepository(NZWalksDbContext nZWalksDbcontext)
        {
            this.nZWalksDbcontext = nZWalksDbcontext;
        }
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await nZWalksDbcontext.Users
                .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower() && x.Password == password);

            if (user == null)
            {
                return null;
            }

            var userRoles = await nZWalksDbcontext.Users_Roles.Where(x => x.UserId == user.Id).ToListAsync();

            if (userRoles.Any())

                user.Roles = new List<string>();
                foreach (var userRole in userRoles)
                {
                    var role = await nZWalksDbcontext.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);
                    if (role != null)
                    { 
                        user.Roles.Add(role.Name);
                    }
                }

            user.Password = null;
            return user;

        }
    }
}
