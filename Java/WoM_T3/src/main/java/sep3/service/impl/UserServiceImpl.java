package sep3.service.impl;

import jakarta.persistence.EntityNotFoundException; // IMPORTANT IMPORT
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import sep3.mapping.UserMapper;
import sep3.dao.UserDAO;
import sep3.dto.userDTO.UserCreationDTO;
import sep3.dto.userDTO.UserDataDTO;
import sep3.dto.userDTO.UserLoginDTO;
import sep3.entity.user.User;
import sep3.service.interfaces.IUserDataService;

import java.util.List;

@Service public class UserServiceImpl implements IUserDataService
{
  private final String DEFAULT_PASSWORD = "0000"; //placed here for easy access
  private final UserDAO userDAO;
  private final PasswordEncoder passwordEncoder; // 1. Inject the tool

  public UserServiceImpl(UserDAO userDAO, PasswordEncoder passwordEncoder)
  {
    this.userDAO = userDAO;
    this.passwordEncoder = passwordEncoder;
  }

  //  CREATE/ADD NEW USER
  @Override public UserDataDTO registerUser(UserCreationDTO creationDTO)
  {
    // Check if email already exists
    if (userDAO.existsByEmail(creationDTO.getEmail()))
    {
      // CHANGED: RuntimeException -> IllegalArgumentException (Maps to gRPC INVALID_ARGUMENT)
      throw new IllegalArgumentException("Email already registered: " + creationDTO.getEmail());
    }
    // Hash the password BEFORE calling the mapper
    String rawPassword = creationDTO.getPassword();
    String encodedPassword = passwordEncoder.encode(rawPassword);

    // We update the DTO to carry the hashed password.
    // The mapper will then unknowingly pass this hash to the Entity constructor.
    creationDTO.setPassword(encodedPassword);

    // Map DTO -> Entity
    User newUser = UserMapper.toEntity(creationDTO);

    // Save to DB
    User savedUser = userDAO.save(newUser);

    // Return the clean Data DTO
    return UserMapper.toDTO(savedUser);
  }

  // READ/GET ONE USER
  @Override public UserDataDTO getUserById(long id)
  {
    // CHANGED: RuntimeException -> EntityNotFoundException (Maps to gRPC NOT_FOUND)
    User user = userDAO.findById(id).orElseThrow(
        () -> new EntityNotFoundException("User not found with id: " + id));
    return UserMapper.toDTO(user);
  }

  @Override public List<UserDataDTO> getUsersByName(String name)
  {
    List<User> foundUsers = userDAO.findByNameContainingIgnoreCase(name);
    return foundUsers.stream().map(UserMapper::toDTO).toList();
  }

  // READ/GET ALL USERS
  @Override public List<UserDataDTO> getAllUsers()
  {
    return userDAO.findAll().stream().map(UserMapper::toDTO).toList();
  }

  // UPDATE USER
  @Override public UserDataDTO updateUser(UserDataDTO user)
  {
    // CHANGED: RuntimeException -> EntityNotFoundException
    User userToUpdate = userDAO.findById(user.getId()).orElseThrow(
        () -> new EntityNotFoundException("User not found: " + user.getId()));

    //update specific mapper
    UserMapper.updateEntity(user, userToUpdate);

    User updatedUser = userDAO.save(userToUpdate);
    return UserMapper.toDTO(updatedUser);
  }

  // DELETE A USER
  @Override public void deleteUser(long id)
  {
    if (!userDAO.existsById(id))
    {
      // Add check and throw EntityNotFoundException
      throw new EntityNotFoundException("User not found with id: " + id);
    }
    userDAO.deleteById(id);
  }

  // VALIDATE USER (Login)
  @Override public UserDataDTO validateUser(UserLoginDTO loginDTO)
  {
    // Find user by email
    User user = userDAO.findByEmail(loginDTO.getEmail()).orElseThrow(
        () -> new EntityNotFoundException("User not found with email: " + loginDTO.getEmail()));

    // Use the Encoder to verify the password
    // .matches(raw, hashed) checks if the input matches the DB hash
    if (!passwordEncoder.matches(loginDTO.getPassword(), user.getPassword()))
    {
      throw new IllegalArgumentException("Incorrect password");
    }

    // Return the user data (Role, ID, etc) so T2 knows who logged in
    return UserMapper.toDTO(user);
  }

  //CHANGE PASSWORD
  public boolean changePassword(String oldpassword, String newpassword, long userid)
  {
    User user = userDAO.findById(userid).orElseThrow(
        () -> new EntityNotFoundException("User not found with id: " + userid));

    // 4. Verify old password using encoder
    if (!passwordEncoder.matches(oldpassword, user.getPassword()))
    {
      throw new IllegalArgumentException("Old password is incorrect");
    }

    // Hash the NEW password before saving
    user.setPassword(passwordEncoder.encode(newpassword));
    userDAO.save(user);
    return true;
  }

  //RESET PASSWORD
  public void resetPassword(long userid)
  {
    User user = userDAO.findById(userid).orElseThrow(
        () -> new EntityNotFoundException("User not found with id: " + userid));

    // Hash the default password
    user.setPassword(passwordEncoder.encode(DEFAULT_PASSWORD));
    userDAO.save(user);
  }
}