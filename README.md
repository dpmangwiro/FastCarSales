# FastCarSales
Overview
FastCarSales is a modern web application for managing car sales. Built with Blazor, it offers a seamless and interactive user experience for car dealers and buyers. The application features a robust set of tools for creating, viewing, and managing car listings.
Features
•	User Authentication: Secure login and registration for users.
•	Role-Based Access Control: Admin and User roles to manage permissions.
•	CRUD Operations: Full Create, Read, Update, Delete functionality for car listings.
•	Search and Filtering: Advanced search and filtering capabilities for car listings.
•	Responsive Design: Fully responsive design for mobile and desktop.
•	Admin Panel: Special admin interface for managing users and listings.
Technologies Used
•	Blazor: Frontend framework for building interactive web UIs.
•	.NET Core: Backend framework for the server-side logic.
•	Entity Framework Core: ORM for database operations.
•	SQL Server: Database for storing application data.
•	Bootstrap: CSS framework for responsive design.
Getting Started
Prerequisites
•	.NET Core SDK
•	SQL Server
Installation
1.	Clone the repository:
bash
git clone https://github.com/dpmangwiro/FastCarSales.git
cd FastCarSales
2.	Restore dependencies:
bash
dotnet restore
3.	Update the database connection string:
o	Modify the appsettings.json file to include your SQL Server connection details.
4.	Apply database migrations:
bash
dotnet ef database update
5.	Run the application:
bash
dotnet run
Usage
•	Create Listings: Users can create new car listings by navigating to the "Sell A Car" page.
•	View Listings: Browse available cars on the "Home" page.
•	Admin Panel: Admins can manage all listings and users through the admin panel accessible via the "Administration" link.
Original Project
This project was originally based on the MyCarDealershipProject by Nikolay Mihov, which was implemented using MVC. Significant modifications and enhancements have been made to adapt the project to Blazor and introduce new features.

Contributing
We welcome contributions! Please follow these steps to contribute:
1.	Fork the repository.
2.	Create a new branch (git checkout -b feature-branch).
3.	Make your changes and commit (git commit -m 'Add some feature').
4.	Push to the branch (git push origin feature-branch).
5.	Create a new Pull Request.
License
This project is licensed under the MIT License. See the LICENSE file for details.
Contact
For any questions or feedback, please contact us at dpmangwiro@gmail.com.


