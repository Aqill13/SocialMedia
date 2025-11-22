using DataAccessLayer.Abstract;
using DataAccessLayer.DbContext;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class UserSocialLinkRepository : GenericRepository<UserSocialLink, AppDbContext>, IUserSocialLinkRepository
    {
        public UserSocialLinkRepository(AppDbContext context) : base(context)
        {
        }
    }
}
