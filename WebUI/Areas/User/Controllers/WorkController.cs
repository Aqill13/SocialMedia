using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.User.Models.Profile;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class WorkController : Controller
    {
        private readonly IUserWorkExperienceService _userWorkExperienceService;
        private readonly UserManager<AppUser> _userManager;

        public WorkController(IUserWorkExperienceService userWorkExperienceService, UserManager<AppUser> userManager)
        {
            _userWorkExperienceService = userWorkExperienceService;
            _userManager = userManager;
        }


        // Work Experience Add/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdateWorkExperience(WorkExperienceEditViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            UserWorkExperience entity;
            if (model.Id > 0)
            {
                entity = await _userWorkExperienceService.GetByIdAsync(model.Id);
                if (entity == null || entity.UserId != currentUser.Id)
                    return Json(new { success = false, message = "Work experience not found" });
                entity.CompanyName = model.CompanyName;
                entity.Position = model.Position;
                entity.StartDate = model.StartDate;
                entity.EndDate = model.EndDate;
                await _userWorkExperienceService.UpdateAsync(entity);
            }
            else
            {
                entity = new UserWorkExperience
                {
                    UserId = currentUser.Id,
                    CompanyName = model.CompanyName,
                    Position = model.Position,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };
                await _userWorkExperienceService.AddAsync(entity);
            }
            return Json(new
            {
                success = true,
                message = "Work experience saved successfully",
                data = new
                {
                    id = entity.Id,
                    companyName = entity.CompanyName,
                    position = entity.Position,
                    startDateDisplay = entity.StartDate.ToString("MMMM yyyy"),
                    endDateDisplay = entity.EndDate?.ToString("MMMM yyyy") ?? "Present",
                    startDateValue = entity.StartDate.ToString("yyyy-MM-dd"),
                    endDateValue = entity.EndDate?.ToString("yyyy-MM-dd") ?? ""
                }
            });
        }

        // Work Experience Delete
        [HttpPost]
        public async Task<IActionResult> DeleteWorkExperience(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var entity = await _userWorkExperienceService.GetByIdAsync(id);
            if (entity == null || entity.UserId != currentUser.Id)
                return Json(new { success = false, message = "Work experience could not be deleted" });
            await _userWorkExperienceService.DeleteAsync(entity);
            return Json(new { success = true, message = "Work experience deleted successfully" });
        }
    }
}
