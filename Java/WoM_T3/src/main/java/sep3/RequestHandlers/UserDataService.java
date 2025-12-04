package sep3.RequestHandlers;

import org.springframework.stereotype.Service;
import sep3.Mapping.UserMapper;
import sep3.dao.UserDAO;
import sep3.dto.UserCreationDTO;
import sep3.dto.UserDataDTO;
import sep3.dto.UserLoginDTO;
import sep3.entity.User;

import java.util.List;

@Service public class UserDataService implements IUserDataService
{
  private final UserDAO userDAO;

  public UserDataService(UserDAO userDAO)
  {
    this.userDAO = userDAO;
  }

  //  CREATE/ADD NEW USER
  @Override
  public UserDataDTO registerUser(UserCreationDTO creationDTO)
  {
    // Check if email already exists
    if (userDAO.existsByEmail(creationDTO.getEmail()))
    {
      throw new RuntimeException("Email already registered: " + creationDTO.getEmail());
    }

    // Map DTO -> Entity
    User newUser = UserMapper.toEntity(creationDTO);

    // Save to DB
    User savedUser = userDAO.save(newUser);

    // Return the clean Data DTO
    return UserMapper.toDTO(savedUser);
  }

  // READ/GET ONE USER (For loading profile, etc.)
  @Override
  public UserDataDTO getUserById(long id)
  {
    User user = userDAO.findById(id)
        .orElseThrow(() -> new RuntimeException("User not found"));
    return UserMapper.toDTO(user);
  }
  @Override
  public List<UserDataDTO> getUsersByName(String name)
  {
    List<User> foundUsers = userDAO.findByNameContainingIgnoreCase(name);
    return foundUsers.stream().map(UserMapper::toDTO).toList();
  }

  //READ/GET ALL USERS
  @Override
  public List<UserDataDTO> getAllUsers()
  {
    return userDAO.findAll().stream().map(UserMapper::toDTO).toList();
  }

  //UPDATE USER
  @Override
  public UserDataDTO updateUser(UserDataDTO user)
  {
    User userToUpdate = userDAO.findById(user.getId()).orElseThrow(() -> new RuntimeException("User not found: " + user.getId()));
    User updatedUser = userDAO.save(userToUpdate);
    return UserMapper.toDTO(updatedUser);
  }

  //DELETE A USER
  @Override
  public void deleteUser(long id)
  {
    userDAO.deleteById(id);
  }

  // EXTRA - CREDENTIAL VERIFICATION (Login Support)
  // This is NOT full authentication. It just checks if the password is correct.
  @Override
  public UserDataDTO validateUser(UserLoginDTO loginDTO)
  {
    // Find user by email
    User user = userDAO.findByEmail(loginDTO.getEmail())
        .orElseThrow(() -> new RuntimeException("User not found"));

    // password verification is done in the User entity.
    // This ensures the service doesn't need to know the specific hashing algorithm.
    try
    {
      user.checkPassword(loginDTO.getPassword());
    }
    catch (RuntimeException e)
    {
      throw new RuntimeException("Incorrect password");
    }

    // Return the user data (Role, ID, etc) so T2 knows who logged in
    return UserMapper.toDTO(user);
  }

}