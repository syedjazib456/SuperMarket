// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Admin_panel.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Admin_panel.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly Applicationdbcontext _dbcontext;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, Applicationdbcontext dbcontext)
        {
            _dbcontext = dbcontext;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                var claimidentity = (ClaimsIdentity)User.Identity;
                var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
                var appuser = _dbcontext.UserRoles.FirstOrDefault(x => x.UserId == claims.Value);
                var role = _dbcontext.Roles.FirstOrDefault(a => a.Id == appuser.RoleId);
                if(role.Name == "Admin") { 
                return LocalRedirect("~/Identity/Account/Login");
                    }
                else if(role.Name == "Customer")
                {
                    return LocalRedirect("~/Web/Index");
                }
                else
                {
                    return RedirectToPage();
                }
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
