using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.User.Models.Profile;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    //[Authorize]
    [Route("User/[controller]/[action]/{username?}")]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPostService _postService;

        public ProfileController(UserManager<AppUser> userManager, IPostService postService)
        {
            _userManager = userManager;
            _postService = postService;
        }
        // Posts
        public async Task<IActionResult> Posts(string? username)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });

            AppUser? profileUser = currentUser;
            if (!string.IsNullOrEmpty(username) && username != currentUser.UserName)
                profileUser = await _userManager.FindByNameAsync(username);
            if (profileUser == null)
                return NotFound();
            var posts = await _postService.GetProfilePostsAsync(profileUser.Id, currentUser.Id);
            var model = new ProfilePostsViewModel
            {
                ProfileUser = profileUser,
                Posts = posts,
                ActiveTopTab = "posts",
                IsOwner = profileUser.Id == currentUser.Id
            };
            return View(model);
        }

        // About
        public async Task<IActionResult> About(string? username, string section = "profile", string sub = "personalInfo")
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            AppUser? profileUser = currentUser;
            if (!string.IsNullOrEmpty(username) && username != currentUser.UserName)
                profileUser = await _userManager.FindByNameAsync(username);
            if (profileUser == null)
                return NotFound();
            var model = new ProfileAboutViewModel
            {
                ProfileUser = profileUser,
                ActiveTopTab = "about",
                ActiveSection = section,
                ActiveSubSection = sub,
                IsOwner = profileUser.Id == currentUser.Id
            };
            return View(model);
        }
    }
}
