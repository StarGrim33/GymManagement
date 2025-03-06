﻿using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Application.MembershipTypes;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Memberships.Errors;

namespace GymManagement.Application.Memberships.GetMembership;

internal sealed class GetMembershipQueryHandler(IMembershipRepository membershipRepository)
    : IQueryHandler<GetMembershipQuery, MembershipResponse>
{
    public async Task<Result<MembershipResponse>> Handle(GetMembershipQuery request,
        CancellationToken cancellationToken)
    {
        var membershipDto = await membershipRepository.GetByIdAsync(request.MembershipId, cancellationToken);

        if (membershipDto == null)
        {
            return Result.Failure<MembershipResponse>(MembershipErrors.NotFound);
        }

        return Result.Success(new MembershipResponse
        {
            Id = membershipDto.Id,
            UserId = membershipDto.UserId,
            GymId = membershipDto.GymId,
            Name = membershipDto.MembershipTypeName,
            PriceAmount = membershipDto.Price,
            Status = (int)membershipDto.MembershipStatus,
            IsActive = membershipDto.IsActive,
            StartDate = membershipDto.StartDate,
            EndDate = membershipDto.EndDate,
            MembershipType = new MembershipTypesResponse
            {
                Id = membershipDto.MembershipTypeId,
                Name = membershipDto.MembershipType
            },
            Invoices = membershipDto.Invoices
        });
    }
}