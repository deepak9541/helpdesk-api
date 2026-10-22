using HelpdeskApi.DTOs;
using HelpdeskApi.Entities;
using HelpdeskApi.Enums;
using HelpdeskApi.Repositories;

namespace HelpdeskApi.Services;

public class TicketService : ITicketService
{
    private readonly IUnitOfWork _uow;

    public TicketService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TicketDto> CreateAsync(CreateTicketDto dto, int currentUserId)
    {
        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            Status = TicketStatus.Open,   
            CreatedById = currentUserId,  
            CreatedAt = DateTime.UtcNow
        };

        await _uow.Tickets.AddAsync(ticket);
        await _uow.SaveChangesAsync();

        var saved = await _uow.Tickets.GetByIdWithDetailsAsync(ticket.Id);
        return MapToDto(saved!);
    }

    public async Task<TicketDto?> GetByIdAsync(int id)
    {
        var ticket = await _uow.Tickets.GetByIdWithDetailsAsync(id);
        return ticket == null ? null : MapToDto(ticket);
    }

    private static TicketDto MapToDto(Ticket t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        Status = t.Status,
        Priority = t.Priority,
        CreatedAt = t.CreatedAt,
        UpdatedAt = t.UpdatedAt,
        CreatedByName = t.CreatedBy?.FullName ?? "",
        AssignedToName = t.AssignedTo?.FullName
    };
    public async Task<(bool Success, string? Error, TicketDto? Ticket)> AssignAsync(int ticketId, int developerId)
    {
        var ticket = await _uow.Tickets.GetByIdWithDetailsAsync(ticketId);
        if (ticket == null)
            return (false, "Ticket not found", null);

        if (ticket.Status == TicketStatus.Closed)
            return (false, "Closed ticket cannot be assigned", null);

        var developer = await _uow.Users.GetByIdAsync(developerId);
        if (developer == null || developer.Role != Roles.Developer)
            return (false, "Invalid developer", null);

        ticket.AssignedToId = developerId;
        ticket.Status = TicketStatus.InProgress;
        ticket.UpdatedAt = DateTime.UtcNow;

        _uow.Tickets.Update(ticket);
        await _uow.SaveChangesAsync();

        var updated = await _uow.Tickets.GetByIdWithDetailsAsync(ticketId);
        return (true, null, MapToDto(updated!));
    }

    public async Task<(bool Success, string? Error, TicketDto? Ticket)> ChangeStatusAsync(
    int ticketId, TicketStatus newStatus, int currentUserId, string role)
    {
        var ticket = await _uow.Tickets.GetByIdWithDetailsAsync(ticketId);
        if (ticket == null)
            return (false, "Ticket not found", null);

        if (role == "Developer" && ticket.AssignedToId != currentUserId)
            return (false, "You can only update tickets assigned to you", null);

        var current = ticket.Status;
        bool allowed;

        if (role == "Developer")
        {
            allowed = current == TicketStatus.InProgress && newStatus == TicketStatus.Resolved;
        }
        else 
        {
            allowed =
                (current == TicketStatus.InProgress && newStatus == TicketStatus.Resolved) ||
                (current == TicketStatus.Resolved && newStatus == TicketStatus.Closed) ||
                (current == TicketStatus.Resolved && newStatus == TicketStatus.InProgress);
        }

        if (!allowed)
            return (false, $"Cannot change status from {current} to {newStatus}", null);

        ticket.Status = newStatus;
        ticket.UpdatedAt = DateTime.UtcNow;
        await _uow.SaveChangesAsync();

        return (true, null, MapToDto(ticket));
    }


    public async Task<(bool Success, string? Error, TicketDto? Ticket)> UpdateAsync(
    int ticketId, UpdateTicketDto dto, int currentUserId, string role)
    {
        var ticket = await _uow.Tickets.GetByIdWithDetailsAsync(ticketId);
        if (ticket == null)
            return (false, "Ticket not found", null);

        if (role == "Employee")
        {
            if (ticket.CreatedById != currentUserId)
                return (false, "You can only edit your own tickets", null);

            if (ticket.Status != TicketStatus.Open)
                return (false, "Ticket can be edited only while it is Open", null);
        }

        ticket.Title = dto.Title;
        ticket.Description = dto.Description;
        ticket.Priority = dto.Priority;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _uow.SaveChangesAsync();
        return (true, null, MapToDto(ticket));
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int ticketId)
    {
        var ticket = await _uow.Tickets.GetByIdAsync(ticketId);
        if (ticket == null)
            return (false, "Ticket not found");

        _uow.Tickets.Delete(ticket);
        await _uow.SaveChangesAsync();
        return (true, null);
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        return await _uow.Tickets.GetStatsAsync();
    }

    public async Task<IEnumerable<TicketDto>> GetAllAsync(int currentUserId, string role)
    {
        IEnumerable<Ticket> tickets;
        if (role == "Admin")
        {
            tickets = await _uow.Tickets.GetAllWithDetailsAsync();
        }
        else if (role == "Developer")
        {
            tickets = await _uow.Tickets.GetByAssignedToAsync(currentUserId);
        }
        else
        {
            tickets = await _uow.Tickets.GetByCreatedByAsync(currentUserId);
        }

        return tickets.Select(MapToDto);
    }

    public async Task<IEnumerable<object>> GetDevelopersAsync()
    {
        var users = await _uow.Users.GetAllAsync();
        return users
            .Where(u => u.Role == Roles.Developer)
            .Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email
            });
    }

}