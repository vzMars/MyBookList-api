# MyBookList API

The back-end for a full-stack social media web application that uses the Google Books API and allows users to search for books and add them to their list and users can also share their list with others.

**Link to project:** https://mybooklist.vzmars.com

**Front-End:** https://github.com/vzMars/mybooklist

![alt text](https://i.imgur.com/XvYLDkI.png)
![alt text](https://i.imgur.com/IoTMXdM.png)

## How It's Made:

**Tech used:** C#, ASP.NET, Microsoft SQL Server

The back-end for this application was made using C# and ASP.NET. This application was organized using the MVC (Model-View-Controller). Microsoft SQL Server is used as the database and stores the information of every user, book, and author. The User model contains the email, username, and the hashed password. The Book model contains the Google Books ID, title, and cover. The BookUser model represents the many-to-many relationship between books and users and contains the book ID, user ID, and the reading status of that relationship. The BookAuthor model represents the many-to-many relationship between books and authors and contains the book ID and the author ID. Identity is used for authenticating users of this application and the BookController and UserController both use the Authorize attribute to prevent unauthorized users from accessing routes in these two controllers. The Google Books API is used when the client is searching for a book and when the client wants to see the details of a book. The back-end also prevents users from adding/updating/deleting books that don't belong to them by checking if the current user's ID matches the book's user ID.

## Optimizations:

The application currently sets the cookie's same-site to none which is only used to test the application locally which means I would have to change that in the future in order to protect my application. I would like to try using JWTs for authentication instead of using cookies since I've never tried that form of authentication before. I would also like to learn what is the best way to deploy both the ASP.NET Web API and the Microsoft SQL Server database for free on Microsoft Azure.

## Lessons Learned:

I learned how to recreate a web application using C# and ASP.NET that was originally made using Node.js and Express. I also switched from using a MongoDB database to using a relational database (Microsoft SQL Server) so there were a few changes I had to make to the models such as adding a table for the authors and adding two join tables for the many-to-many relationships that the books have with users and authors.

## More Projects:

Take a look at these other projects that I have in my portfolio:

**Employee CRM API:** https://github.com/vzMars/employee-crm-api

**GameBlog API:** https://github.com/vzMars/gameblog-api

**MangaNotifications:** https://github.com/vzMars/manga-notifications