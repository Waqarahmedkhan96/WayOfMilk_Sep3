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
import sep3.entity.user.Owner; // Assuming Owner is a concrete implementation of User
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
    // 1. ARRANGE
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

    // 2. ACT
    UserDataDTO result = userService.registerUser(creationDTO);

    // 3. ASSERT
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

    // Important: Mock matches() to return TRUE
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

 // BASIC CRUD TESTS

  @Test
  void testGetUserById_NotFound() {
    when(mockUserRepository.findById(99L)).thenReturn(Optional.empty());

    assertThrows(EntityNotFoundException.class, () -> {
      userService.getUserById(99L);
    });
  }

  @Test
  void testDeleteUser_Success() {
    long id = 10L;
    when(mockUserRepository.existsById(id)).thenReturn(true);

    userService.deleteUser(id);

    verify(mockUserRepository).deleteById(id);
  }
}