using System;

namespace ClassifiedsApi.AppServices.Exceptions.Accounts;

public class IncorrectCredentialsException : Exception
{
    public IncorrectCredentialsException() : base("Неправильные данные для входа в аккаунт.") { }
}