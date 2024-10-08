using System;

namespace ClassifiedsApi.AppServices.Common.Validators;

/// <summary>
/// Атрибут указывающий на то, что валидатор должен игнорироваться при автоматической регистрации в DI-контейнере.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class IgnoreAutomaticRegistrationAttribute : Attribute
{
    
}