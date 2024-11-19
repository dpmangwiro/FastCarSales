using FastCarSales.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Data.Seeding
{
    public class MakeSeeder: ISeeder
    {
        public async Task SeedAsync(FastCarSalesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Makes.Any())
            {
                return;
            }
                      
            var makesToSeed = new List<Make>()
            {
                new Make() { Name = "Alpha Romeo"},
                new Make() { Name = "Audi" },
                new Make() { Name = "Bentley"},
                new Make() { Name = "BMW" },
                new Make() { Name = "Chery"},
                new Make() { Name = "Chevrolet"},
                new Make() { Name = "Citroen"},
                new Make() { Name = "Chrysler"},
                new Make() { Name = "DAF"},
                new Make() { Name = "Daihatsu"},
                new Make() { Name = "Dodge"},
                new Make() { Name = "Freightliner"},
                new Make() { Name = "ERF"},
                new Make() { Name = "Fiat"},
                new Make() { Name = "Ford"},
                new Make() { Name = "Haval"},
                new Make() { Name = "Higer"},
                new Make() { Name = "Hino"},
                new Make() { Name = "Honda"},
                new Make() { Name = "Howo"},
                new Make() { Name = "Hyundai"},
                new Make() { Name = "Infiniti"},
                new Make() { Name = "Isuzu"},
                new Make() { Name = "Iveco"},
                new Make() { Name = "Jaguar"},
                new Make() { Name = "Jeep"},
                new Make() { Name = "Kamaz"},
                new Make() { Name = "Kia"},
                new Make() { Name = "Lamborghini"},
                new Make() { Name = "Lexus"},
                new Make() { Name = "Leyland"},
                new Make() { Name = "Mahindra"},
                new Make() { Name = "MAN"},
                new Make() { Name = "Maserati"},
                new Make() { Name = "Mazda"},
                new Make() { Name = "Mercedes-Benz"},
                new Make() { Name = "Mitsubishi"},
                new Make() { Name = "Nissan"},
                new Make() { Name = "Opel"},
                new Make() { Name = "Peugeot"},
                new Make() { Name = "Porsche"},
                new Make() { Name = "Range Rover"},
                new Make() { Name = "Renault"},
                new Make() { Name = "Rolls-Royce"},
                new Make() { Name = "Scania"},
                new Make() { Name = "Shacman"},
                new Make() { Name = "SinoTruck"},
                new Make() { Name = "Subaru"},
                new Make() { Name = "Suzuki"},
                new Make() { Name = "Tata"},
                new Make() { Name = "Toyota"},
                new Make() { Name = "UD" },
                new Make() { Name = "Vauxhall"},
                new Make() { Name = "VW" },
                new Make() { Name = "Volvo"},
                new Make() { Name = "Yutong"},

            };

            await dbContext.Makes.AddRangeAsync(makesToSeed);
            await dbContext.SaveChangesAsync();          

        }
    }
}
