package sep3.RequestHandlers;

import sep3.Mapping.UserMapper;
import sep3.dto.UserCreationDTO;
import sep3.dto.UserDataDTO;
import sep3.dto.UserLoginDTO;
import sep3.entity.User;

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
}
