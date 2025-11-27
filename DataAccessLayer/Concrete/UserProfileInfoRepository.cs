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
    public class UserProfileInfoRepository : GenericRepository<UserProfileInfo, AppDbContext>, IUserProfileInfoRepository
    {
        public UserProfileInfoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
