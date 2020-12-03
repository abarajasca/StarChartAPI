using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }        

        [HttpGet("{id:int}",Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(p => p.Id == id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            celestialObject.Satellites = _context.CelestialObjects.Where(p => p.OrbitedObjectId == id).ToList();
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(p => p.Name == name);
            if (celestialObjects.Count() == 0 )
            {
                return NotFound();
            }
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(p => p.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects;
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(p => p.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create(CelestialObject FromBody)
        {
            _context.CelestialObjects.Add(FromBody);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = FromBody.Id },FromBody);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id,CelestialObject updatedCelestialObject)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(p => p.Id == id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            celestialObject.Name = updatedCelestialObject.Name;
            celestialObject.OrbitalPeriod = updatedCelestialObject.OrbitalPeriod;
            celestialObject.OrbitedObjectId = updatedCelestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id,string name)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(p => p.Id == id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            celestialObject.Name = name;
            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(p => p.Id == id).ToArray();
            if (celestialObjects.Count() == 0 )
            {
                return NotFound();
            }
            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
