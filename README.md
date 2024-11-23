# FastCarSales

## Overview

FastCarSales is a modern web application for managing car sales. Built with Blazor, it offers a seamless and interactive user experience for car dealers and buyers. The application features a robust set of tools for creating, viewing, and managing car listings.

## Features

## Posts ##
  **Featured Posts**: Features 2 Posts in the featured section.
  
  **Recent Posts**: Lists recent post in the front page.
  
  **Search**: Browsers can use the simple, fast and flexible search feature to search for cars.
  
  **Admin Panel**: Special admin interface for managing listings. Admin can view, approve, delete, restore, set featured posts

## User Accounts ##
    **Registration**: Users who need to sell on the website can register to sell. Users can post an unlimited number of cars.
    **Posts Management**: Users can view, edit and delete their posts.
## User Authentication ##
    *Secure login and registration for users.

## Responsive Design ##
    *Fully responsive design for mobile and desktop.

## Technologies Used

**Blazor**: Frontend framework for building interactive web UIs.

**.NET Core**: Backend framework for the server-side logic.

**Entity Framework Core**: ORM for database operations.

**SQL Server**: Database for storing application data.

**Bootstrap**: CSS framework for responsive design.

## Getting Started

### Prerequisites

    - .NET Core SDK
    - SQL Server

## Installation

Clone the repository:

`git clone https://github.com/dpmangwiro/FastCarSales.git`
`cd FastCarSales`

Restore dependencies:

`dotnet restore`

Update the database connection string:

Modify the appsettings.json file to include your SQL Server connection details.

Apply database migrations:

`dotnet ef database update`

Run the application:

`dotnet run`

## Original Project

This project was originally based on the MyCarDealershipProject by Nikolay Mihov, which was implemented using MVC. Significant modifications and enhancements have been made to adapt the project to Blazor and introduce new features.

## Contributing

We welcome contributions! Please follow these steps to contribute:

Fork the repository.

Create a new branch (git checkout -b feature-branch).

Make your changes and commit (git commit -m 'Add some feature').

Push to the branch (git push origin feature-branch).

Create a new Pull Request.

## License

This project is licensed under the MIT License. See the LICENSE file for details.

## Contact

For any questions or feedback, please contact us at dpmangwiro@gmail.com.
