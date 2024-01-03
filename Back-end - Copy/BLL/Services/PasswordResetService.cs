using BLL.Interfaces;
using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;

namespace BLL.Services;

public class PasswordResetService : IPasswordResetService
{
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IMailService _mailService;
    private readonly IUserService _userService;

    public PasswordResetService(IPasswordResetRepository passwordResetRepository, IMailService mailService, IUserService userService)
    {
        _passwordResetRepository = passwordResetRepository;
        _mailService = mailService;
        _userService = userService;
    }

    /// <summary>
    /// Generates a random verification code with the specified length.
    /// </summary>
    /// <param name="length">The length of the verification code to generate.</param>
    /// <returns>A string representing the generated verification code.</returns>
    private static string GenerateVerificationCode(int length)
    {
        Random random = new Random();
        const string chars = "0123456789";

        char[] code = new char[length];
        for (int i = 0; i < length; i++)
        {
            code[i] = chars[random.Next(chars.Length)];
        }

        return new string(code);
    }

    /// <summary>
    /// Posts a reset code for password recovery and sends an email notification.
    /// </summary>
    /// <param name="email">The email address of the user requesting a password reset.</param>
    public async Task CreateResetCodeAsync(string email)
    {
        string code = GenerateVerificationCode(6);
        User user = await _passwordResetRepository.CreateResetCodeAsync(code, email);
        _mailService.SendPasswordEmail(code, email, "Verificatie code.", user.Name);
    }

    /// <summary>
    /// Checks the entered verification code during the password recovery process.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="code">The verification code entered by the user.</param>
    public async Task CheckEnteredCodeAsync(string email, string code)
    {
        await _passwordResetRepository.CheckEnteredCodeAsync(email, code);
    }
    
    /// <summary>
    /// Posts a new password for a user after the password recovery process.
    /// </summary>
    /// <param name="requestDto">A PasswordChangeRequest object containing the new password and associated information.</param>
    public Task CreatePasswordAsync(CreatePasswordRequestDto requestDto)
    {
        if (requestDto.Password1 != requestDto.Password2)
        {
            throw new InputValidationException("Wachtwoorden zijn niet gelijk aan elkaar!");
        }

        return _passwordResetRepository.CreatePasswordAsync(requestDto.Email, requestDto.Password1, requestDto.Code);
    }
    
    /// <summary>
    /// Updates the password for the specified user based on the provided request.
    /// </summary>
    /// <param name="updatePasswordRequestDto">The data transfer object containing update password information.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdatePasswordAsync(UpdatePasswordRequestDto updatePasswordRequestDto)
    {
        
        LoginRequestDto dto = new LoginRequestDto()
        {
            Email = updatePasswordRequestDto.Email,
            Password = updatePasswordRequestDto.Password1
        };

        await _userService.CheckCredentialsAsync(dto);
        
        if (updatePasswordRequestDto.Password2 != updatePasswordRequestDto.Password3)
        {
            throw new InputValidationException("Wachtwoorden zijn niet gelijk aan elkaar!");
        }

        User user = new User()
        {
            Email = updatePasswordRequestDto.Email,
            Name = updatePasswordRequestDto.Name,
            PasswordHash = updatePasswordRequestDto.PasswordHash,
            PasswordSalt = updatePasswordRequestDto.PasswordSalt,
            UserId = updatePasswordRequestDto.UserId
        };
        
        await _passwordResetRepository.UpdatePasswordAsync(user, updatePasswordRequestDto.Password2);
    }
}