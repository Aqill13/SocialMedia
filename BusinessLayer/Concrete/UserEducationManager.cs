using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserEducationManager : GenericManager<UserEducation>, IUserEducationService
    {
        private readonly IUserEducationRepository _userEducationRepository;

        public UserEducationManager(IUserEducationRepository userEducationRepository) : base(userEducationRepository)
        {
            _userEducationRepository = userEducationRepository;
        }
    }
}
