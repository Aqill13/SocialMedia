using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserProfileVisibilityService
    {
        Task<Dictionary<ProfileField, VisibilityLevel>> GetUserProfileVisibilitySettingsAsync(string userId);
        Task SetUserProfileVisibilityAsync(string userId, ProfileField field, VisibilityLevel visibility);
    }
}
