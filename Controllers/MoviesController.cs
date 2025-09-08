using Microsoft.AspNetCore.Mvc;
using newCRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private static readonly List<Movie> _movies = new()
        {
            new Movie { Id = Guid.NewGuid(), Title = "Inception", Genre = "Sci-Fi", Year = 2010 },
            new Movie { Id = Guid.NewGuid(), Title = "The Godfather", Genre = "Crime", Year = 1972 },
            new Movie { Id = Guid.NewGuid(), Title = "Spirited Away", Genre = "Animation", Year = 2001 }
        };

        [HttpGet]
        public ActionResult<IEnumerable<MovieDto>> GetAll()
        {
            return Ok(_movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Genre = m.Genre,
                Year = m.Year
            }));
        }

        [HttpGet("{id:guid}")]
        public ActionResult<MovieDto> GetById(Guid id)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);
            if (movie is null) return NotFound();

            return Ok(new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Year = movie.Year
            });
        }
        [HttpPost]
        public ActionResult<MovieDto> Create([FromBody] CreateMovieDto dto)
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Genre = dto.Genre,
                Year = dto.Year
            };

            _movies.Add(movie);

            var result = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Year = movie.Year
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdateMovieDto dto)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);
            if (movie is null) return NotFound();

            movie.Title = dto.Title;
            movie.Genre = dto.Genre;
            movie.Year = dto.Year;

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);
            if (movie is null) return NotFound();

            _movies.Remove(movie);
            return NoContent();
        }
    }
}
