using HRP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjectX.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OffersController : ControllerBase
{
    private readonly ProjectContext _context;

    public OffersController(ProjectContext context)
    {
        _context = context;
    }

    // Case 1: Create a new offer
    [HttpPost]
    public async Task<IActionResult> CreateOffer(CreateOfferRequest request)
    {
        var applicationExists = await _context.Applications
            .AnyAsync(a => a.ApplicationID == request.ApplicationID);

        if (!applicationExists)
        {
            return BadRequest("The application does not exist.");
        }

        var offerAlreadyExists = await _context.Offers
            .AnyAsync(o => o.ApplicationID == request.ApplicationID);

        if (offerAlreadyExists)
        {
            return BadRequest("An offer already exists for this application.");
        }

        if (request.ProposalSalary <= 0)
        {
            return BadRequest("The proposed salary must be greater than zero.");
        }

        var offer = new Offer
        {
            ProposalSalary = request.ProposalSalary,
            OfferState = request.OfferState,
            JobTitle = request.JobTitle,
            ApplicationID = request.ApplicationID
        };

        _context.Offers.Add(offer);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetOfferById),
            new { id = offer.OfferID },
            offer);
    }
    // Case 2: Update salary and job title
    [HttpPut("{id}/details")]
    public async Task<IActionResult> UpdateOfferDetails(
        int id,
        UpdateOfferDetailsRequest request)
    {
        var offer = await _context.Offers.FindAsync(id);

        if (offer == null)
        {
            return NotFound("Offer not found.");
        }

        if (request.ProposalSalary <= 0)
        {
            return BadRequest("The proposed salary must be greater than zero.");
        }

        offer.ProposalSalary = request.ProposalSalary;
        offer.JobTitle = request.JobTitle;

        await _context.SaveChangesAsync();

        return Ok(offer);
    }

    // Case 3: Update offer state
    [HttpPatch("{id}/state")]
    public async Task<IActionResult> UpdateOfferState(
        int id,
        UpdateOfferStateRequest request)
    {
        var offer = await _context.Offers.FindAsync(id);

        if (offer == null)
        {
            return NotFound("Offer not found.");
        }

        offer.OfferState = request.OfferState;

        await _context.SaveChangesAsync();

        return Ok(offer);
    }

    // Case 4: Delete an offer
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOffer(int id)
    {
        var offer = await _context.Offers.FindAsync(id);

        if (offer == null)
        {
            return NotFound("Offer not found.");
        }

        _context.Offers.Remove(offer);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Case 5: Get all offers with application details
    [HttpGet]
    public async Task<IActionResult> GetAllOffers()
    {
        var offers = await _context.Offers
            .AsNoTracking()
            .Include(o => o.Application)
            .Select(o => new
            {
                o.OfferID,
                o.ProposalSalary,
                o.OfferState,
                o.JobTitle,
                o.ApplicationID,

                Application = new
                {
                    o.Application.ApplicationID,
                    o.Application.AppliedAt,
                    o.Application.ApplicationStatus,
                    o.Application.JobPostingID
                }
            })
            .ToListAsync();

        return Ok(offers);
    }

    // case 6: Find an offer by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOfferById(int id)
    {
        var offer = await _context.Offers
            .AsNoTracking()
            .Include(o => o.Application)
            .Where(o => o.OfferID == id)
            .Select(o => new
            {
                o.OfferID,
                o.ProposalSalary,
                o.OfferState,
                o.JobTitle,
                o.ApplicationID,

                Application = new
                {
                    o.Application.ApplicationID,
                    o.Application.AppliedAt,
                    o.Application.ApplicationStatus,
                    o.Application.JobPostingID
                }
            })
            .FirstOrDefaultAsync();

        if (offer == null)
        {
            return NotFound("Offer not found.");
        }

        return Ok(offer);
    }

    // Case 7: Filter offers by state
    [HttpGet("filter-by-state")]
    public async Task<IActionResult> FilterOffersByState(string state)
    {
        if (string.IsNullOrWhiteSpace(state))
        {
            return BadRequest("Offer state is required.");
        }

        var offers = await _context.Offers
            .AsNoTracking()
            .Where(o => o.OfferState.ToLower() == state.ToLower())
            .OrderBy(o => o.OfferID)
            .ToListAsync();

        return Ok(offers);
    }

    // Case 8: Sort offers by proposed salary
    [HttpGet("sort-by-salary")]
    public async Task<IActionResult> SortOffersBySalary(
        bool descending = false)
    {
        var query = _context.Offers.AsNoTracking();

        var offers = descending
            ? await query
                .OrderByDescending(o => o.ProposalSalary)
                .ToListAsync()

            : await query
                .OrderBy(o => o.ProposalSalary)
                .ToListAsync();

        return Ok(offers);
    }
}

// Data required when creating an offer
public class CreateOfferRequest
{
    public decimal ProposalSalary { get; set; }

    public string OfferState { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public int ApplicationID { get; set; }
}


// Data required when updating offer details
public class UpdateOfferDetailsRequest
{
    public decimal ProposalSalary { get; set; }

    public string JobTitle { get; set; } = string.Empty;
}


// Data required when updating offer state
public class UpdateOfferStateRequest
{
    public string OfferState { get; set; } = string.Empty;
}




