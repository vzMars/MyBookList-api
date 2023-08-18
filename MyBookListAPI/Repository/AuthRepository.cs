using Microsoft.AspNetCore.Identity;
using MyBookListAPI.Data;
using MyBookListAPI.Dto;
using MyBookListAPI.Interfaces;
using MyBookListAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MyBookListAPI.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            var response = new AuthResponse
            {
                Message = "Incorrect email or password."
            };

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                response.Message = "Please complete all required fields.";
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return response;

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);

            if (result.Succeeded)
            {
                response.Success = true;
                response.User = new User
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Username = user.UserName!
                };

                response.Message = "Successfully logged in.";
                return response;
            }

            return response;
        }

        public Task<AuthResponse> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponse> Signup(SignupRequest request)
        {
            var response = new AuthResponse();

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                response.Message = "Please complete all required fields.";
                return response;
            }

            if (!ValidEmail(request.Email))
            {
                response.Message = "The email must be a valid email address.";
                return response;
            }

            if (!ValidUsernameLength(request.Username))
            {
                response.Message = "The username must be between 2-20 characters.";
                return response;
            }

            if (!ValidUsername(request.Username))
            {
                response.Message = "The username may only contain letters and numbers.";
                return response;
            }

            if (!await ValidPassword(request.Password))
            {
                response.Message = "The password is not strong enough.";
                return response;
            }

            var email = await _userManager.FindByEmailAsync(request.Email);
            var username = await _userManager.FindByNameAsync(request.Username);

            if (email != null || username != null)
            {
                response.Message = "The username or email has already been taken.";
                return response;
            }

            var user = new ApplicationUser()
            {
                Email = request.Email,
                UserName = request.Username,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                response.User = new User
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.UserName
                };
                response.Message = "Account has been successfully created.";
                response.Success = true;

                return response;
            }

            response.Message = "There was an error in creating the account. Please try again.";
            return response;
        }

        private bool ValidEmail(string email)
        {
            var e = new EmailAddressAttribute();
            return e.IsValid(email);
        }

        private bool ValidUsernameLength(string username)
        {
            return username.Length >= 2 && username.Length <= 20;
        }

        private bool ValidUsername(string username)
        {
            var u = new RegularExpressionAttribute("^[A-Za-z0-9]*$");
            return u.IsValid(username);
        }

        private async Task<bool> ValidPassword(string password)
        {
            var passwordValidator = new PasswordValidator<ApplicationUser>();
            var validPassword = await passwordValidator.ValidateAsync(_userManager, null, password);

            return validPassword.Succeeded;
        }
    }
}
