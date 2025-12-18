package sep3.service.impl;

import jakarta.persistence.EntityNotFoundException;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.ArgumentCaptor;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.security.crypto.password.PasswordEncoder;
import sep3.dto.userDTO.UserCreationDTO;
import sep3.dto.userDTO.UserDataDTO;
import sep3.dto.userDTO.UserLoginDTO;
import sep3.entity.user.Owner; // Owner is a concrete implementation of User
import sep3.entity.user.User;
import sep3.entity.user.UserRole;
import sep3.repository.UserRepository;

import java.util.Optional;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class UserServiceImplTest {

  @Mock
  private UserRepository mockUserRepository;

  @Mock
  private PasswordEncoder mockPasswordEncoder;

  @InjectMocks
  private UserServiceImpl userService;

  // REGISTRATION & HASHING TESTS

  @Test
  void testRegisterUser_Success_And_HashesPassword() {
    // 1. Arrange
    String rawPassword = "mySecretPassword";
    String encodedPassword = "encoded_mySecretPassword_123";

    UserCreationDTO creationDTO = new UserCreationDTO();
    creationDTO.setName("name");
    creationDTO.setEmail("new@email.com");
    creationDTO.setPassword(rawPassword);
    creationDTO.setName("Test User");
    creationDTO.setRole("OWNER");

    // Mock the encoder to return a specific "hashed" string
    when(mockPasswordEncoder.encode(rawPassword)).thenReturn(encodedPassword);

    // Mock the repository to check if email exists (return false = safe to register)
    when(mockUserRepository.existsByEmail(creationDTO.getEmail())).thenReturn(false);

    // Mock saving the user
    // We return a dummy user to satisfy the method signature
    User savedUser = new Owner();
    savedUser.setName(creationDTO.getName());
    savedUser.setId(1L);
    savedUser.setEmail(creationDTO.getEmail());
    savedUser.setPassword(encodedPassword);
    savedUser.setRole(UserRole.OWNER);

    //Mock returning the saved user from the repository
    when(mockUserRepository.save(any(User.class))).thenReturn(savedUser);

    // 2. Act
    UserDataDTO result = userService.registerUser(creationDTO);

    // 3. Assert
    assertNotNull(result);

    // --- HASHED PASSWORD VERIFICATION ---
    // We use an ArgumentCaptor to steal the object that was passed to .save()
    // so we can inspect it.
    ArgumentCaptor<User> userCaptor = ArgumentCaptor.forClass(User.class);
    verify(mockUserRepository).save(userCaptor.capture());

    User actuallySavedUser = userCaptor.getValue();

    // CRITICAL: The saved user must have the ENCODED password, not the raw one
    assertEquals(encodedPassword, actuallySavedUser.getPassword());
    assertNotEquals(rawPassword, actuallySavedUser.getPassword());

    // Verify the encoder was actually called
    verify(mockPasswordEncoder, times(1)).encode(rawPassword);
  }

  @Test
  void testRegisterUser_EmailAlreadyExists() {
    // 1. Arrange
    UserCreationDTO creationDTO = new UserCreationDTO();
    creationDTO.setEmail("existing@email.com");

    when(mockUserRepository.existsByEmail("existing@email.com")).thenReturn(true);

    // 2. Act & Assert
    IllegalArgumentException thrown = assertThrows(IllegalArgumentException.class, () -> {
      userService.registerUser(creationDTO);
    });

    assertEquals("Email already registered: existing@email.com", thrown.getMessage());
    // Verify we never tried to save anything
    verify(mockUserRepository, never()).save(any());
  }

   // LOGIN / VALIDATION TESTS

  @Test
  void testValidateUser_Success() {
    // 1. Arrange
    String email = "valid@email.com";
    String rawPass = "password123";
    String dbHash = "hashed_password123";

    UserLoginDTO loginDTO = new UserLoginDTO();
    loginDTO.setEmail(email);
    loginDTO.setPassword(rawPass);

    User dbUser = new Owner();
    dbUser.setEmail(email);
    dbUser.setPassword(dbHash);
    dbUser.setRole(UserRole.OWNER);

    when(mockUserRepository.findByEmail(email)).thenReturn(Optional.of(dbUser));

    // Important: Mock matches() return TRUE here to simulate correct password
    when(mockPasswordEncoder.matches(rawPass, dbHash)).thenReturn(true);

    // 2. Act
    UserDataDTO result = userService.validateUser(loginDTO);

    // 3. Assert
    assertNotNull(result);
    assertEquals(email, result.getEmail());
    assertEquals("OWNER", result.getRole());
  }

  @Test
  void testValidateUser_WrongPassword() {
    // 1. Arrange
    String email = "valid@email.com";
    String rawPass = "wrongPassword";
    String dbHash = "hashed_realPassword";

    UserLoginDTO loginDTO = new UserLoginDTO();
    loginDTO.setEmail(email);
    loginDTO.setPassword(rawPass);

    User dbUser = new Owner();
    dbUser.setEmail(email);
    dbUser.setPassword(dbHash);

    when(mockUserRepository.findByEmail(email)).thenReturn(Optional.of(dbUser));

    // Important: Mock matches() to return FALSE
    when(mockPasswordEncoder.matches(rawPass, dbHash)).thenReturn(false);

    // 2. Act & Assert
    assertThrows(IllegalArgumentException.class, () -> {
      userService.validateUser(loginDTO);
    });
  }

 // PASSWORD CHANGE TESTS

  @Test
  void testChangePassword_Success() {
    // 1. Arrange
    long userId = 1L;
    String oldRaw = "oldPass";
    String newRaw = "newPass";
    String oldHash = "hash_oldPass";
    String newHash = "hash_newPass";

    User dbUser = new Owner();
    dbUser.setId(userId);
    dbUser.setPassword(oldHash);

    when(mockUserRepository.findById(userId)).thenReturn(Optional.of(dbUser));
    when(mockPasswordEncoder.matches(oldRaw, oldHash)).thenReturn(true); // Old pass correct
    when(mockPasswordEncoder.encode(newRaw)).thenReturn(newHash); // Encode new pass

    // 2. Act
    boolean result = userService.changePassword(oldRaw, newRaw, userId);

    // 3. Assert
    assertTrue(result);

    // Check that the saved user now has the NEW hash
    assertEquals(newHash, dbUser.getPassword());
    verify(mockUserRepository).save(dbUser);
  }

  @Test
  void testResetPassword_Success() {
    // 1. Arrange
    long userId = 5L;
    String expectedHash = "hashed_default_secret";

    User mockUser = new Owner();
    mockUser.setId(userId);
    // User starts with some random old password
    mockUser.setPassword("old_random_pass");

    when(mockUserRepository.findById(userId)).thenReturn(Optional.of(mockUser));

    // Use the class constant (DEFAULT_PASSWORD)
    // We tell the encoder: "When you are asked to encode the DEFAULT PASSWORD, return this hash"
    when(mockPasswordEncoder.encode(UserServiceImpl.DEFAULT_PASSWORD)).thenReturn(expectedHash);

    // 2. Act
    userService.resetPassword(userId);

    // 3. Assert
    // Capture the saved user to check what password ended up in the DB
    ArgumentCaptor<User> userCaptor = ArgumentCaptor.forClass(User.class);
    verify(mockUserRepository).save(userCaptor.capture());

    User savedUser = userCaptor.getValue();

    // Verify the saved user has the HASHED version of the default password
    assertEquals(expectedHash, savedUser.getPassword());
  }

 // BASIC CRUD TESTS
  //CREATE was already tested above,
  // technically UPDATE too, but we'll test some other U scenarios anyway

  @Test
  void testGetUserById_NotFound() {
    //1. Arrange
    when(mockUserRepository.findById(99L)).thenReturn(Optional.empty());
    //2. Act & Assert
    assertThrows(EntityNotFoundException.class, () -> {
      userService.getUserById(99L);
    });
  }

  @Test
  void testUpdateUser_Success() {
    // 1. Arrange
    long userId = 1L;

    // A. Create the "Old" User currently in the Database
    User existingUser = new Owner(); // or new Worker/Vet
    existingUser.setId(userId);
    existingUser.setName("Old Name");
    existingUser.setEmail("old@email.com");
    existingUser.setRole(UserRole.OWNER); // Important: Role determines DTO creation

    // B. Create the DTO containing the "New" changes
    UserDataDTO changesDTO = new UserDataDTO();
    changesDTO.setId(userId);
    changesDTO.setName("New Updated Name");   // We want to change this
    changesDTO.setEmail("new@email.com");     // We want to change this
    // We leave phone/address null to verify they don't overwrite existing data (mapper handles that)

    // C. Mocking
    // When Service asks for ID 1, give it the existing user
    when(mockUserRepository.findById(userId)).thenReturn(Optional.of(existingUser));

    // When Service saves the user, return the user (which will be modified by then)
    when(mockUserRepository.save(any(User.class))).thenReturn(existingUser);

    // 2. Act
    UserDataDTO result = userService.updateUser(changesDTO);

    // 3. Assert
    assertNotNull(result);

    // Check that the returned DTO has the NEW values
    assertEquals("New Updated Name", result.getName());
    assertEquals("new@email.com", result.getEmail());

    // Verify the repository save was called
    verify(mockUserRepository).save(existingUser);
  }

  //Exception (error) situation test
  @Test
  void testUpdateUser_NotFound() {
    // 1. Arrange
    long nonExistentId = 999L;

    UserDataDTO changesDTO = new UserDataDTO();
    changesDTO.setId(nonExistentId);
    changesDTO.setName("Ghost User");

    // Mock the repository to return Empty
    when(mockUserRepository.findById(nonExistentId)).thenReturn(Optional.empty());

    // 2. Act & Assert
    // Expect the EntityNotFoundException because your service uses .orElseThrow(...)
    assertThrows(EntityNotFoundException.class, () -> {
      userService.updateUser(changesDTO);
    });

    // Verify we NEVER tried to save anything
    verify(mockUserRepository, never()).save(any());
  }

  @Test
  void testDeleteUser_Success() {
    //1. Arrange
    long id = 10L;
    when(mockUserRepository.existsById(id)).thenReturn(true);

    //2. Act
    userService.deleteUser(id);

    //3. Assert
    verify(mockUserRepository).deleteById(id);
  }
}