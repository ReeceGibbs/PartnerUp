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

        //we write a handler for an HTTP Put request to update a workout record given an id
        [HttpPut]
        public IHttpActionResult UpdateWorkout(Workout workout)
        {

            //we check to see if the data is valid
            if (!ModelState.IsValid)
            {

                //if the workout object passed here is invalid, then we return a BadRequest
                return BadRequest("Invalid Workout Object");
            }

            //we begin a using block to reference our DbContext
            using (PartnerUpEntities entities = new PartnerUpEntities())
            {

                //we define a new Workout object and assign it the record in the database that has the id passed to this handler
                Workout dbWorkout = entities.Workouts.FirstOrDefault(w => w.id == workout.id);

                //we check to see if the workout object that the user was looking for existed
                if (dbWorkout == null)
                {

                    //if so, we return a BadRequest
                    return BadRequest("Invalid Record Id");
                }

                //if the record id was valid, then we want to update the record with the information the user provided
                dbWorkout.date = workout.date;
                dbWorkout.exercises = workout.exercises;
                dbWorkout.reps = workout.reps;
                dbWorkout.sets = workout.sets;

                //we save the changes to the database
                entities.SaveChanges();
            }

            return Ok("Workout Record Successfully Updated");
        }

        //we write a handler for an HTTP Delete request to delete a workout record given an id
        [HttpDelete]
        public IHttpActionResult DeleteWorkout(int id)
        {

            //we begin a using block to reference our dbcontext
            using (PartnerUpEntities entities = new PartnerUpEntities())
            {

                //we define a workout object that is representative of the workout record with the given id
                Workout workout = entities.Workouts.FirstOrDefault(w => w.id == id);

                //we check to see if the record id is valid
                if (workout == null)
                {

                    //if the id is invalid, then we return a BadRequest
                    return BadRequest("Invalid Record Id");
                }

                //if the record does exist, then we want to remove it from the database
                entities.Workouts.Remove(workout);

                //we save the changes to the database
                entities.SaveChanges();
            }
            
            //if all actions were completed successfully, then we want to return a message that indicates that
            return Ok("Workout Record Successfully Deleted");
        }
    }
}
