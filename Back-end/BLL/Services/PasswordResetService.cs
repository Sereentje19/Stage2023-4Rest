using System.Threading.Tasks;
using BLL.Services;
using DAL.Repositories;
using PL.Exceptions;
using PL.Models;
using PL.Models.Requests;

public class PasswordResetService : IPasswordResetService
{
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IMailService _mailService;
    private readonly ILoginService _loginService;

    public PasswordResetService(IPasswordResetRepository passwordResetRepository, IMailService mailService, ILoginService loginService)
    {
        _passwordResetRepository = passwordResetRepository;
        _mailService = mailService;
        _loginService = loginService;
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
    public async Task PostResetCode(string email)
    {
        string code = GenerateVerificationCode(6);
        User user = await _passwordResetRepository.PostResetCode(code, email);
        _mailService.SendPasswordEmail(code, email, "Verificatie code.", user.Name);
    }

    /// <summary>
    /// Checks the entered verification code during the password recovery process.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="code">The verification code entered by the user.</param>
    public async Task CheckEnteredCode(string email, string code)
    {
        await _passwordResetRepository.CheckEnteredCode(email, code);
    }
    
    /// <summary>
    /// Posts a new password for a user after the password recovery process.
    /// </summary>
    /// <param name="request">A PasswordChangeRequest object containing the new password and associated information.</param>
    public async Task PostPassword(PasswordChangeRequest request)
    {
        if (request.Password1 != request.Password2)
        {
            throw new InputValidationException("Wachtwoorden zijn niet gelijk aan elkaar!");
        }
        
        await _passwordResetRepository.PostPassword(request.Email, request.Password1, request.Code);
    }
    
    public async Task PutPassword(User user, string password1, string password2, string password3)
    {
        LoginRequestDTO dto = new LoginRequestDTO()
        {
            Email = user.Email,
            Password = password1
        };

        await _loginService.CheckCredentials(dto);
        
        if (password2 != password3)
        {
            throw new InputValidationException("Wachtwoorden zijn niet gelijk aan elkaar!");
        }
        
        await _passwordResetRepository.PutPassword(user, password2);
    }
}