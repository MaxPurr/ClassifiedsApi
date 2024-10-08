using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <summary>
/// Валидатор названия характеристики.
/// </summary>

[IgnoreAutomaticRegistration]
public class CharacteristicNameValidator : AbstractValidator<string?>
{
    private readonly Guid _advertId;
    private readonly ICharacteristicRepository _characteristicRepository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicNameValidator"/>.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="characteristicRepository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    public CharacteristicNameValidator(Guid advertId, ICharacteristicRepository characteristicRepository)
    {
        _advertId = advertId;
        _characteristicRepository = characteristicRepository;
        
        RuleFor(name => name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255)
            .MustAsync(IsNameAvailableAsync)
            .WithName("Name")
            .WithMessage("Характеристика с таким названием уже существует.");
    }

    private async Task<bool> IsNameAvailableAsync(string name, CancellationToken token)
    {
        var exists = await _characteristicRepository.IsExistsAsync(_advertId, name, token);
        return !exists;
    }
}