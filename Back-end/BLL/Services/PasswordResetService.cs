﻿using System.Threading.Tasks;
using BLL.Services;
using DAL.Repositories;
using PL.Models;

public class PasswordResetService : IPasswordResetService
{
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IMailService _mailService;

    public PasswordResetService(IPasswordResetRepository passwordResetRepository, IMailService mailService)
    {
        _passwordResetRepository = passwordResetRepository;
        _mailService = mailService;
    }

    static string GenerateVerificationCode(int length)
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
        await _passwordResetRepository.PostResetCode(code, email);
        _mailService.SendPasswordEmail(code, email, "Verificatie code.");
    }

    public async Task CheckEnteredCode(string email, string code)
    {
        string password = await _passwordResetRepository.CheckEnteredCode(email, code);
        _mailService.SendPasswordEmail(password, email, "Nieuw wachtwoord.");
    }
}