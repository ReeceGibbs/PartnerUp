using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PartnerUpDataAccess;

namespace PartnerUp.Controllers
{
    public class WorkoutsController : ApiController
    {

        //we write a handler for an HTTP Get request that returns a list of all workouts in the Workouts table
        //the handler returns a list of Workouts DTOs
        [HttpGet]
        public IEnumerable<Workout> GetAllWorkouts()
        {
            
            //we use a using block to reference our DbContext through the PartnerUpEntities class
            using (PartnerUpEntities entities = new PartnerUpEntities())
            {

                //we return a list of all Workouts DTOs
                return entities.Workouts.ToList();
            }
        }

        //we write a handler for an HTTP Get request that returns a single Workout DTO given an id
        [HttpGet]
        public Workout GetWorkout(int id)
        {

            //we begin a using block in our reference to our DbContext through the PartnerUpEntities class
            using (PartnerUpEntities entities = new PartnerUpEntities())
            {

                //we return the relevant Workouts DTO given the id passed to this handler
                return entities.Workouts.FirstOrDefault(workout => workout.id == id);
            }
        }

        //we write a handler for an HTTP post request that will allow workout records to be inserted into the database
        [HttpPost]
        public IHttpActionResult InsertWorkout(Workout workout)
        {

            //we check to see if the data is valid
            if (!ModelState.IsValid)
            {

                //if invalid then we return a BadRequest
                return BadRequest("Invalid Workout Object");
            }

            //we begin a using block to reference our DbContext through our PartnerUpEntities class
            using (PartnerUpEntities entities = new PartnerUpEntities())
            {

                //we add a now workout to our dbcontext
                entities.Workouts.Add(new Workout()
                {
                    date = workout.date,
                    exercises = workout.exercises,
                    reps = workout.reps,
                    sets = workout.sets
                });

                //we save the changes made to our dbcontext so that they are committed to the database
                entities.SaveChanges();
            }

            //we return an HttpActionResult that reflects that everything went well
            return Ok("Workout Successfully Inserted");
        }
    }
}
