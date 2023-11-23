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

    public PasswordResetService(IPasswordResetRepository passwordResetRepository, IMailService mailService)
    {
        _passwordResetRepository = passwordResetRepository;
        _mailService = mailService;
    }

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

    public async Task PostResetCode(string email)
    {
        string code = GenerateVerificationCode(6);
        User user = await _passwordResetRepository.PostResetCode(code, email);
        _mailService.SendPasswordEmail(code, email, "Verificatie code.", user.Name);
    }

    public async Task CheckEnteredCode(string email, string code)
    {
        await _passwordResetRepository.CheckEnteredCode(email, code);
    }
    
    public async Task PostPassword(PasswordChangeRequest request)
    {
        if (request.Password1 != request.Password2)
        {
            throw new InputValidationException("Wachtwoorden zijn niet gelijk aan elkaar!");
        }
        
        await _passwordResetRepository.PostPassword(request.Email, request.Password1, request.Code);
    }
}