using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class MediaController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public MediaController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // Change Profile Image
        [HttpPost]
        public async Task<IActionResult> ChangeProfileImage(IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            if (file != null && file.Length > 0)
            {
                var oldImageUrl = user.ImageUrl;
                var newFileName = Guid.NewGuid() + ".png";
                var filePath = Path.Combine("wwwroot", "uploads/profilep", newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                user.ImageUrl = "/uploads/profilep/" + newFileName;
                await _userManager.UpdateAsync(user);
                if (!string.IsNullOrWhiteSpace(oldImageUrl) && !oldImageUrl.Contains("default-profile-account"))
                {
                    var oldPhysicalPath = Path.Combine("wwwroot", oldImageUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(oldPhysicalPath))
                        System.IO.File.Delete(oldPhysicalPath);
                }
                return Json(new
                {
                    success = true,
                    message = "Profile image updated successfully",
                    imageUrl = user.ImageUrl
                });
            }
            return Json(new
            {
                success = false,
                message = "No file selected"
            });
        }

        // Remove Profile Image
        [HttpPost]
        public async Task<IActionResult> RemoveProfileImage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var oldImageUrl = user.ImageUrl;
            user.ImageUrl = "/uploads/profilep/default-profile-account.jpg";
            await _userManager.UpdateAsync(user);
            if (!string.IsNullOrWhiteSpace(oldImageUrl) && !oldImageUrl.Contains("default-profile-account"))
            {
                var oldPhysicalPath = Path.Combine("wwwroot", oldImageUrl.TrimStart('/', '\\'));
                if (System.IO.File.Exists(oldPhysicalPath))
                    System.IO.File.Delete(oldPhysicalPath);
            }
            return Json(new
            {
                success = true,
                message = "Profile image removed successfully",
                imageUrl = user.ImageUrl
            });
        }

        // Change Cover Image
        [HttpPost]
        public async Task<IActionResult> ChangeCoverImage(IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            if (file != null && file.Length > 0)
            {
                var oldImageUrl = user.CoverImageUrl;
                var newFileName = Guid.NewGuid() + ".png";
                var filePath = Path.Combine("wwwroot", "uploads/cover", newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                user.CoverImageUrl = "/uploads/cover/" + newFileName;
                await _userManager.UpdateAsync(user);
                if (!string.IsNullOrWhiteSpace(oldImageUrl) && !oldImageUrl.Contains("default-cover"))
                {
                    var oldPhysicalPath = Path.Combine("wwwroot", oldImageUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(oldPhysicalPath))
                        System.IO.File.Delete(oldPhysicalPath);
                }
                return Json(new
                {
                    success = true,
                    message = "Cover image updated successfully",
                    imageUrl = user.CoverImageUrl
                });
            }
            return Json(new
            {
                success = false,
                message = "No file selected"
            });
        }

        // Remove Cover Image
        [HttpPost]
        public async Task<IActionResult> RemoveCoverImage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var oldImageUrl = user.CoverImageUrl;
            user.CoverImageUrl = "/uploads/cover/default-cover-img.png";
            await _userManager.UpdateAsync(user);
            if (!string.IsNullOrWhiteSpace(oldImageUrl) && !oldImageUrl.Contains("default-cover"))
            {
                var oldPhysicalPath = Path.Combine("wwwroot", oldImageUrl.TrimStart('/', '\\'));
                if (System.IO.File.Exists(oldPhysicalPath))
                    System.IO.File.Delete(oldPhysicalPath);
            }
            return Json(new
            {
                success = true,
                message = "Cover image removed successfully",
                imageUrl = user.CoverImageUrl
            });
        }
    }
}
