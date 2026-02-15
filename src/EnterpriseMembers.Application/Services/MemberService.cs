using AutoMapper;
using EnterpriseMembers.Application.DTOs;
using EnterpriseMembers.Application.Interfaces;
using EnterpriseMembers.Domain.Entities;

namespace EnterpriseMembers.Application.Services;

public class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<(List<MemberDto> Items, int TotalCount)> GetMembersAsync(
        int page,
        int pageSize,
        string? search,
        bool expiredOnly,
        string sortBy,
        string sortDir)
    {
        var (items, totalCount) = await _unitOfWork.Members.GetPagedAsync(
            page, pageSize, search, expiredOnly, sortBy, sortDir);

        var memberDtos = _mapper.Map<List<MemberDto>>(items);

        return (memberDtos, totalCount);
    }

    public async Task<MemberDto?> GetMemberByIdAsync(int id)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id);
        return member != null ? _mapper.Map<MemberDto>(member) : null;
    }

    public async Task<MemberDto?> GetMemberByEmailAsync(string email)
    {
        var member = await _unitOfWork.Members.GetByEmailAsync(email);
        return member != null ? _mapper.Map<MemberDto>(member) : null;
    }

    public async Task<MemberDto> CreateMemberAsync(CreateMemberDto dto)
    {
        // Business validation
        if (await _unitOfWork.Members.EmailExistsAsync(dto.Email))
        {
            throw new InvalidOperationException($"A member with email '{dto.Email}' already exists.");
        }

        // Map and create
        var member = _mapper.Map<Member>(dto);

        // Transaction
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.Members.AddAsync(member);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<MemberDto>(member);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<MemberDto> UpdateMemberAsync(int id, UpdateMemberDto dto)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id);

        if (member == null)
        {
            throw new KeyNotFoundException($"Member with ID {id} not found.");
        }

        // Business validation - check if email is taken by another member
        if (await _unitOfWork.Members.EmailExistsAsync(dto.Email, id))
        {
            throw new InvalidOperationException($"A member with email '{dto.Email}' already exists.");
        }

        // Update properties
        _mapper.Map(dto, member);

        // Transaction
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.Members.UpdateAsync(member);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<MemberDto>(member);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task DeleteMemberAsync(int id)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id);

        if (member == null)
        {
            throw new KeyNotFoundException($"Member with ID {id} not found.");
        }

        // Transaction
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.Members.DeleteAsync(member);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> IsMembershipExpiredAsync(int memberId)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(memberId);

        if (member == null)
        {
            throw new KeyNotFoundException($"Member with ID {memberId} not found.");
        }

        return member.ExpiryDate < DateTime.UtcNow;
    }

    public async Task<int> GetActiveMembersCountAsync()
    {
        var allMembers = await _unitOfWork.Members.GetAllAsync();
        return allMembers.Count(m => m.ExpiryDate >= DateTime.UtcNow);
    }
}
