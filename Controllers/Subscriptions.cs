using Microsoft.AspNetCore.Mvc;

// RUTA FINAL: /api/v1/subscriptions
[ApiController]
[Route("api/v1/[controller]")] // [controller] = "subscriptions"
public class SubscriptionsController : ControllerBase
{
    // Persistencia en memoria (lista simple)
    private static readonly List<Subscription> _subscriptions = new()
    {
        new Subscription { Name = "Plan Básico", Duration = 6 },
        new Subscription { Name = "Plan Premium", Duration = 12 }
    };

    // GET /api/v1/subscriptions  -> 200
    [HttpGet]
    public ActionResult<IEnumerable<Subscription>> GetAll()
        => Ok(_subscriptions);

    // GET /api/v1/subscriptions/{id}  -> 200 / 404
    [HttpGet("{id:guid}")]
    public ActionResult<Subscription> GetOne(Guid id)
        => _subscriptions.FirstOrDefault(s => s.Id == id) is { } sub
           ? Ok(sub)
           : NotFound(new { error = "Subscription not found" });

    // POST /api/v1/subscriptions  -> 201 / 400
    [HttpPost]
    public ActionResult<Subscription> Create([FromBody] CreateSubscriptionDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState); // 400

        var sub = new Subscription
        {
            Id = Guid.NewGuid(),
            SubscriptionDate = dto.SubscriptionDate,
            Duration = dto.Duration,
            Name = dto.Name.Trim()
        };

        _subscriptions.Add(sub);
        return CreatedAtAction(nameof(GetOne), new { id = sub.Id }, sub); // 201
    }

    // PUT /api/v1/subscriptions/{id}  -> 204 / 400 / 404
    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] UpdateSubscriptionDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState); // 400

        var index = _subscriptions.FindIndex(s => s.Id == id);
        if (index == -1) return NotFound(new { error = "Subscription not found" }); // 404

        _subscriptions[index] = new Subscription
        {
            Id = id,
            SubscriptionDate = dto.SubscriptionDate,
            Duration = dto.Duration,
            Name = dto.Name.Trim()
        };

        return NoContent(); // 204
    }

    // DELETE /api/v1/subscriptions/{id}  -> 204 / 404
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var removed = _subscriptions.RemoveAll(s => s.Id == id);
        return removed == 0 ? NotFound(new { error = "Subscription not found" }) // 404
                            : NoContent(); // 204
    }
}
