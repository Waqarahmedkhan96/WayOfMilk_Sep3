package sep3.service.impl;

import jakarta.persistence.EntityNotFoundException;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import sep3.mapping.UserMapper;
import sep3.repository.UserRepository;
import sep3.dto.userDTO.UserCreationDTO;
import sep3.dto.userDTO.UserDataDTO;
import sep3.dto.userDTO.UserLoginDTO;
import sep3.entity.user.*;
import sep3.service.interfaces.IUserService;

import java.util.List;

@Service
public class UserServiceImpl implements IUserService
{
    private final String DEFAULT_PASSWORD = "0000";
    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;

    public UserServiceImpl(UserRepository userRepository, PasswordEncoder passwordEncoder)
    {
        this.userRepository = userRepository;
        this.passwordEncoder = passwordEncoder;
    }

    // CREATE
    @Override
    public UserDataDTO registerUser(UserCreationDTO creationDTO)
    {
        if (userRepository.existsByEmail(creationDTO.getEmail()))
            throw new IllegalArgumentException("Email already registered: " + creationDTO.getEmail());

        if (creationDTO.getRole() == null || creationDTO.getRole().isBlank())
            creationDTO.setRole(UserRole.WORKER.name());

        if (UserRole.OWNER.name().equalsIgnoreCase(creationDTO.getRole()))
            throw new IllegalArgumentException("Owner cannot be created via API.");

        creationDTO.setPassword(passwordEncoder.encode(creationDTO.getPassword()));

        User newUser = UserMapper.toEntity(creationDTO);
        User saved = userRepository.save(newUser);

        return UserMapper.toDTO(saved);
    }

    // READ
    @Override
    public UserDataDTO getUserById(long id)
    {
        User user = userRepository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("User not found: " + id));
        return UserMapper.toDTO(user);
    }

    @Override
    public List<UserDataDTO> getUsersByName(String name)
    {
        return userRepository.findByNameContainingIgnoreCase(name)
                .stream().map(UserMapper::toDTO).toList();
    }

    @Override
    public List<UserDataDTO> getAllUsers()
    {
        return userRepository.findAll().stream().map(UserMapper::toDTO).toList();
    }

    // ======================================================
    //                  FIXED UPDATE METHOD
    // ======================================================
    @Override
    public UserDataDTO updateUser(UserDataDTO dto)
    {
        User existing = userRepository.findById(dto.getId())
                .orElseThrow(() -> new EntityNotFoundException("User not found"));

        String newRole = dto.getRole() != null
                ? dto.getRole().toUpperCase()
                : existing.getRole().name();

        String oldRole = existing.getRole().name();

        User updated;

        // ---------- CASE A: WORKER → VET ----------
        if (!oldRole.equals("VET") && newRole.equals("VET"))
        {
            Vet vet = new Vet(
                    existing.getName(),
                    existing.getEmail(),
                    existing.getPhone(),
                    existing.getAddress(),
                    existing.getPassword(),
                    dto.getLicenseNumber()
            );
            vet.setId(existing.getId());
            applyBasicUpdates(vet, dto);
            updated = vet;
        }

        // ---------- CASE B: VET → WORKER ----------
        else if (!oldRole.equals("WORKER") && newRole.equals("WORKER"))
        {
            Worker worker = new Worker(
                    existing.getName(),
                    existing.getEmail(),
                    existing.getPhone(),
                    existing.getAddress(),
                    existing.getPassword()
            );
            worker.setId(existing.getId());
            applyBasicUpdates(worker, dto);
            updated = worker;
        }

        // ---------- CASE C: NO ROLE CHANGE ----------
        else
        {
            applyBasicUpdates(existing, dto);

            if (existing instanceof Vet vet && dto.getLicenseNumber() != null)
                vet.setLicenseNumber(dto.getLicenseNumber());

            updated = existing;
        }

        userRepository.save(updated);
        return UserMapper.toDTO(updated);
    }

    // helper
    private void applyBasicUpdates(User user, UserDataDTO dto)
    {
        if (dto.getName() != null) user.setName(dto.getName());
        if (dto.getEmail() != null) user.setEmail(dto.getEmail());
        if (dto.getPhone() != null) user.setPhone(dto.getPhone());
        if (dto.getAddress() != null) user.setAddress(dto.getAddress());
    }

    // DELETE
    @Override
    public void deleteUser(long id)
    {
        if (!userRepository.existsById(id))
            throw new EntityNotFoundException("User not found: " + id);

        userRepository.deleteById(id);
    }

    // LOGIN
    @Override
    public UserDataDTO validateUser(UserLoginDTO loginDTO)
    {
        User user = userRepository.findByEmail(loginDTO.getEmail())
                .orElseThrow(() -> new EntityNotFoundException("User not found"));

        if (!passwordEncoder.matches(loginDTO.getPassword(), user.getPassword()))
            throw new IllegalArgumentException("Incorrect password");

        return UserMapper.toDTO(user);
    }

    // CHANGE PASSWORD
    public boolean changePassword(String oldpassword, String newpassword, long userid)
    {
        User user = userRepository.findById(userid)
                .orElseThrow(() -> new EntityNotFoundException("User not found"));

        if (!passwordEncoder.matches(oldpassword, user.getPassword()))
            throw new IllegalArgumentException("Old password incorrect");

        user.setPassword(passwordEncoder.encode(newpassword));
        userRepository.save(user);
        return true;
    }

    // RESET PASSWORD
    public void resetPassword(long userid)
    {
        User user = userRepository.findById(userid)
                .orElseThrow(() -> new EntityNotFoundException("User not found"));

        user.setPassword(passwordEncoder.encode(DEFAULT_PASSWORD));
        userRepository.save(user);
    }
}
