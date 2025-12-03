using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.User.Models.Profile;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class EducationController : Controller
    {
        private readonly IUserEducationService _userEducationService;
        private readonly UserManager<AppUser> _userManager;
        public EducationController(IUserEducationService userEducationService, UserManager<AppUser> userManager)
        {
            _userEducationService = userEducationService;
            _userManager = userManager;
        }

        // Education Add/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdateEducation(EducationEditViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            UserEducation entity;
            if (model.Id > 0)
            {
                entity = await _userEducationService.GetByIdAsync(model.Id);
                if (entity == null || entity.UserId != currentUser.Id)
                    return Json(new { success = false, message = "Education not found" });
                entity.StartDate = model.StartDate;
                entity.EndDate = model.EndDate;
                entity.SchoolName = model.SchoolName;
                entity.Field = model.Field;
                entity.Degree = model.Degree;
                await _userEducationService.UpdateAsync(entity);
            }
            else
            {
                entity = new UserEducation
                {
                    Degree = model.Degree,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Field = model.Field,
                    SchoolName = model.SchoolName,
                    UserId = currentUser.Id
                };
                await _userEducationService.AddAsync(entity);
            }
            return Json(new
            {
                success = true,
                message = "Education saved successfully",
                data = new
                {
                    id = entity.Id,
                    schoolName = entity.SchoolName,
                    field = entity.Field,
                    degree = entity.Degree,
                    startDateDisplay = entity.StartDate.ToString("MMMM yyyy"),
                    endDateDisplay = entity.EndDate?.ToString("MMMM yyyy") ?? "Present",
                    startDateValue = entity.StartDate.ToString("yyyy-MM-dd"),
                    endDateValue = entity.EndDate?.ToString("yyyy-MM-dd") ?? ""
                }
            });
        }

        // Delete education
        [HttpPost]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var entity = await _userEducationService.GetByIdAsync(id);
            if (entity == null || entity.UserId != currentUser.Id)
                return Json(new { success = false, message = "Education could not be deleted" });
            await _userEducationService.DeleteAsync(entity);
            return Json(new { success = true, message = "Education deleted successfully" });
        }
    }
}
