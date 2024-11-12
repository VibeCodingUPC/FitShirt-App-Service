using AutoMapper;
using FitShirt.Application.Security.Exceptions;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.Commands;
using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Security.Services;

namespace FitShirt.Application.Security.Features.CommandServices;

public class UserCommandService : IUserCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IEncryptService _encryptService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IGoogleCaptchaValidator _captchaValidator;

    public UserCommandService(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, IEncryptService encryptService, ITokenService tokenService, IGoogleCaptchaValidator captchaValidator)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _encryptService = encryptService;
        _tokenService = tokenService;
        _mapper = mapper;
        _captchaValidator = captchaValidator;
    }

    public async Task<string> Handle(LoginUserCommand command)
    {
        var userInDatabase = await _userRepository.GetUserByUsernameAsync(command.Username);
        if (userInDatabase == null)
        {
            throw new NotFoundEntityAttributeException(
                nameof(User), nameof(command.Username), command.Username
            );
        }

        if (!_encryptService.Verify(command.Password, userInDatabase.Password))
        {
            throw new IncorrectPasswordException();
        }

        var detailUser = await _userRepository.GetDetailedUserInformationAsync(userInDatabase.Id);

        return _tokenService.GenerateToken(detailUser!);
    }

    public async Task<UserResponse> Handle(RegisterUserCommand command)
    {
        // Verificar el Captcha
        var captchaIsValid = await _captchaValidator.ValidateAsync(command.CaptchaResponse);
        if (!captchaIsValid)
        {
            throw new ArgumentException("Invalid Captcha.");
        }

        // Verificar si el correo ya existe
        var userWithSameEmail = await _userRepository.GetUserByEmailAsync(command.Email);
        if (userWithSameEmail != null)
        {
            throw new DuplicatedUserEmailException(command.Email);
        }
        
        // Verificar si el teléfono ya existe
        var userWithSamePhoneNumber = await _userRepository.GetUserByPhoneNumberAsync(command.Cellphone);
        if (userWithSamePhoneNumber != null)
        {
            throw new DuplicatedUserCellphoneException(command.Cellphone);
        }
        
        // Verificar si el nombre de usuario ya existe
        var userWithSameUsername = await _userRepository.GetUserByUsernameAsync(command.Username);
        if (userWithSameUsername != null)
        {
            throw new DuplicatedUserUsernameException(command.Username);
        }

        // Validar el rol del usuario
        UserRoles userRole;
        if (!Enum.TryParse<UserRoles>(command.UserRole, out userRole))
        {
            throw new ArgumentException("Not valid user role");   
        }

        if (userRole.Equals(UserRoles.ADMIN))
        {
            throw new UnauthorizedAccessException("You cannot register with this role");
        }

        Role roleEntity = await _roleRepository.GetRoleByNameAsync(userRole);
        
        // Crear el usuario basado en el rol
        User userEntity = roleEntity!.Name switch
        {
            UserRoles.CLIENT => new Client(),
            UserRoles.SELLER => new Seller(),
            _ => throw new ArgumentException("Rol de usuario no válido"),
        };

        // Mapear los valores del comando a la entidad del usuario
        _mapper.Map(command, userEntity, typeof(RegisterUserCommand), typeof(User));

        userEntity.Role = roleEntity!;
        userEntity.Password = _encryptService.Encrypt(command.Password);

        // Guardar en la base de datos
        await _userRepository.SaveAsync(userEntity);

        // Crear la respuesta
        var userResponse = _mapper.Map<UserResponse>(userEntity);

        return userResponse;
    }


    public async Task<UserResponse> Handle(int id, UpdateUserCommand command)
    {
        var userToUpdate = await _userRepository.GetDetailedUserInformationAsync(id);
        if (userToUpdate == null)
        {
            throw new NotFoundEntityIdException(nameof(User), id);
        }
        var userWithSameEmail = await _userRepository.GetUserByEmailAsync(command.Email);
        if (userWithSameEmail != null && userToUpdate.Id != userWithSameEmail.Id)
        {
            throw new DuplicatedUserEmailException(command.Email);
        }
        
        var userWithSamePhoneNumber = await _userRepository.GetUserByPhoneNumberAsync(command.Cellphone);
        if (userWithSamePhoneNumber != null && userToUpdate.Id != userWithSamePhoneNumber.Id)
        {
            throw new DuplicatedUserCellphoneException(command.Cellphone);
        }
        
        var userWithSameUsername = await _userRepository.GetUserByUsernameAsync(command.Username);
        if (userWithSameUsername != null && userToUpdate.Id != userWithSameUsername.Id)
        {
            throw new DuplicatedUserUsernameException(command.Username);
        }

        _mapper.Map(command, userToUpdate, typeof(UpdateUserCommand), typeof(User));
        userToUpdate.Password = _encryptService.Encrypt(command.Password);

        await _userRepository.ModifyAsync(userToUpdate);
        var userResponse = _mapper.Map<UserResponse>(userToUpdate);
        return userResponse;

    }

    public async Task<bool> Handle(DeleteUserCommand command)
    {
        var user = await _userRepository.GetByIdAsync(command.Id);
        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), command.Id);
        }

        return await _userRepository.DeleteAsync(command.Id);
    }
}