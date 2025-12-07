package sep3.RequestHandlers;

import sep3.dto.userDTO.UserCreationDTO;
import sep3.dto.userDTO.UserDataDTO;
import sep3.dto.userDTO.UserLoginDTO;

import java.util.List;

public interface IUserDataService
{
  //CREATE/REGISTER
  UserDataDTO registerUser(UserCreationDTO creationDTO);

  // READ/GET
  UserDataDTO getUserById(long id);
  List<UserDataDTO> getAllUsers();
  List<UserDataDTO> getUsersByName(String name);

  //UPDATE
  UserDataDTO updateUser(UserDataDTO user);

  //DELETE
  void deleteUser(long id);

  // EXTRA - CREDENTIAL VERIFICATION (Login Support)
   UserDataDTO validateUser(UserLoginDTO loginDTO);
  boolean changePassword(String oldPassword, String newPassword, long userId);
  void resetPassword(long userId);
}
