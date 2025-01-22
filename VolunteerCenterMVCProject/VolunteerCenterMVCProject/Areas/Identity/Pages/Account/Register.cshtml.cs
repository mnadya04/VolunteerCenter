// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using VolunteerCenterMVCProject.Common;
using VolunteerCenterMVCProject.Models;

namespace VolunteerCenterMVCProject.Areas.Identity.Pages.Account
{
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly IUserStore<User> _userStore;
		private readonly IUserEmailStore<User> _emailStore;
		// private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;

		public RegisterModel(
			UserManager<User> userManager,
			IUserStore<User> userStore,
			SignInManager<User> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender)
		{
			_userManager = userManager;
			_userStore = userStore;
			_emailStore = GetEmailStore();
			_signInManager = signInManager;
			// _logger = logger;
		}

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public string ReturnUrl { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		// public IList<AuthenticationScheme> ExternalLogins { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public class InputModel
		{
			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }


			[Required]
			[Display(Name = "First name")]
			public string FirstName { get; set; }

			[Required]
			[Display(Name = "Last name")]
			public string LastName { get; set; }
		}


		public async Task OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");

			if (ModelState.IsValid)
			{
				var user = new User()
				{
					UserName = Input.Email,
					Email = Input.Email,
					FirstName = Input.FirstName,
					LastName = Input.LastName
				};
				/* await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                 await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
 */

				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{

					await _userManager.AddToRoleAsync(user, Constants.VolunteerRole);

					// _logger.LogInformation("User created a new account with password.");

					await _signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToPage("/Account/Login");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return Page();
		}

		/*	private User CreateUser()
			{
				try
				{
					var user = Activator.CreateInstance<User>();

					// Set FirstName and LastName properties based on the input model
					user.FirstName = Input.FirstName;
					user.LastName = Input.LastName;

					return user;
				}
				catch
				{
					throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
						$"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
						$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
				}
			}*/


		private IUserEmailStore<User> GetEmailStore()
		{
			if (!_userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<User>)_userStore;
		}
	}
}
