using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FastCarSales.Data.Models
{
	/*
	Sedan: A traditional car with a separate compartment for the engine, passengers, and cargo.

	Hatchback: A car with a rear door that swings upward to provide access to a cargo area.

	Coupe: A two-door car with a fixed roof, often sporty in nature.

	Convertible: A car with a roof structure that can be 'converted' to allow open-air or enclosed driving.

	SUV (Sport Utility Vehicle): A larger vehicle designed for more passenger and cargo capacity, often with off-road capabilities.

	Crossover: A vehicle built on a car platform, combining features of an SUV and a passenger car.

	Station Wagon: A car with a longer body and a large cargo area that extends to the back.

	Minivan: A vehicle designed for passenger transport, typically with sliding doors.

	Pickup Truck: A light motor vehicle with an enclosed cab and an open cargo area with low sides and tailgate.

	Roadster: A two-seat convertible car, typically with sporty performance and design.

	Van: A larger vehicle designed for transporting goods or groups of people.

	Sports Car: A car designed for high-speed performance and agility, often with a sleek design.
	*/

    public class BodyType
	{
		public int Id { get; init; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }=string.Empty;

		public ICollection<Car> Cars { get; set; } = new HashSet<Car>();
        public ICollection<CarModel> CarModels { get; set; } = new HashSet<CarModel>();
    }
}
