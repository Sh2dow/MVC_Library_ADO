# ADO.Net-MVC-Library

To run this example follow this steps:

1 - Download this repository

2 - Open and copy the content of database.sql and run in your sql server

3 - Change the connection string into the MVC_Library.Persistence.Database.cs for your database connection

4 - Run the project.


The application should provide the following functionality:

- managing books available in the library: adding, removing, changing quantity. Each book can have a few authors,
 an author can write a few books.

- a book can be taken by different people and at different time;

- registration is required to take books. User should be able to register himself. Password is not necessary,
 but email is (i.e. strong security protection is not necessary for this task);

- Implement filter that shows all books / books available / books taken by the user.

- library tracks its readers (users). Show a history of a book (when and by whom was taken)

- implement sending notifications by mail to people who took a book (“You took the
following books in our library”)


Implementation requirements:

1. ASP.NET MVC Application

2. Library UI is a web application, which has a front-end page with grid. Grid should allow paging,
sorting by book titles and author names. No complex design required.

3. Minimize number of post backs.

4. No usage of an existing ORM like Entity Framework, NHibernate etc. for working with the database is allowed.
