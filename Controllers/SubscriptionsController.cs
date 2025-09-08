using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using newCRUD.Models; // <-- importante

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // -> /api/subscriptions
    public class SubscriptionsController : ControllerBase
    {
        private static readonly List<Subscription> _subscriptions = new()
        {
            new Subscription {
                Id = Guid.NewGuid(),
                Name = "Basic",
                SubscriptionDate = DateTime.UtcNow.Date,
                DurationMonths = 1
            },
            new Subscription {
                Id = Guid.NewGuid(),
                Name = "Premium",
                SubscriptionDate = DateTime.UtcNow.Date.AddDays(-7),
                DurationMonths = 12
            }
        };

        // READ: GET api/subscriptions (con paginación, filtros y ordenamiento)
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] string? name,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] int? minDuration,
            [FromQuery] int? maxDuration,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "subscriptionDate",
            [FromQuery] string sortDir = "asc")
        {
            const int MAX_PAGE_SIZE = 100;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            if (pageSize > MAX_PAGE_SIZE) pageSize = MAX_PAGE_SIZE;

            IEnumerable<Subscription> query = _subscriptions;

            // filtros
            if (!string.IsNullOrWhiteSpace(name))
            {
                var term = name.Trim().ToLowerInvariant();
                query = query.Where(s => s.Name.ToLowerInvariant().Contains(term));
            }
            if (dateFrom.HasValue)
                query = query.Where(s => s.SubscriptionDate.Date >= dateFrom.Value.Date);
            if (dateTo.HasValue)
                query = query.Where(s => s.SubscriptionDate.Date <= dateTo.Value.Date);
            if (minDuration.HasValue)
                query = query.Where(s => s.DurationMonths >= minDuration.Value);
            if (maxDuration.HasValue)
                query = query.Where(s => s.DurationMonths <= maxDuration.Value);

            // ordenamiento
            bool desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
            query = (sortBy.ToLowerInvariant()) switch
            {
                "name" => desc ? query.OrderByDescending(s => s.Name)
                                          : query.OrderBy(s => s.Name),
                "durationmonths" => desc ? query.OrderByDescending(s => s.DurationMonths)
                                          : query.OrderBy(s => s.DurationMonths),
                _ => desc ? query.OrderByDescending(s => s.SubscriptionDate)
                                          : query.OrderBy(s => s.SubscriptionDate),
            };

            // paginación
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // respuesta anónima con metadatos + items
            return Ok(new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                SortBy = sortBy,
                SortDir = sortDir,
                Name = name,
                DateFrom = dateFrom?.ToString("yyyy-MM-dd"),
                DateTo = dateTo?.ToString("yyyy-MM-dd"),
                MinDuration = minDuration,
                MaxDuration = maxDuration,
                Items = items
            });
        }

        // READ: GET api/subscriptions/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Subscription> GetOne(Guid id)
        {
            var sub = _subscriptions.FirstOrDefault(s => s.Id == id);
            return sub is null
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : Ok(sub);
        }

        // CREATE: POST api/subscriptions
        [HttpPost]
        public ActionResult<Subscription> Create([FromBody] CreateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var sub = new Subscription
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                SubscriptionDate = dto.SubscriptionDate,
                DurationMonths = dto.DurationMonths
            };

            _subscriptions.Add(sub);
            return CreatedAtAction(nameof(GetOne), new { id = sub.Id }, sub);
        }

        // UPDATE: PUT api/subscriptions/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Subscription> Update(Guid id, [FromBody] UpdateSubscriptionDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _subscriptions.FindIndex(s => s.Id == id);
            if (index == -1)
                return NotFound(new { error = "Subscription not found", status = 404 });

            var updated = new Subscription
            {
                Id = id,
                Name = dto.Name.Trim(),
                SubscriptionDate = dto.SubscriptionDate,
                DurationMonths = dto.DurationMonths
            };

            _subscriptions[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/subscriptions/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _subscriptions.RemoveAll(s => s.Id == id);
            return removed == 0
                ? NotFound(new { error = "Subscription not found", status = 404 })
                : NoContent();
        }
    }
}
