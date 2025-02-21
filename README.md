# library-management

Test project for managing book storage.

The API includes CRUD operations for 2 resources: authors and books. It also has a fuzzy search of books by title and a selection of all books by author. For the author entity, caching is implemented using Redis at the endpoints of selecting. Pagination is implemented on all endpoints with a potentially large response. Fuzzy search is implemented using the Levenshtein distance and has 2 implementations: on the database side and on the service side (the implementation can be switched using a toggle). All requests are validated, in case of an error, a correct and understandable description is displayed in the log. The DbUp library is used for migrations. For validation, the FluentValidation library is used.

Database [schema](./library-management-backend/Resources/Db). 

API [description](./library-management-backend/Resources/Http) and [postman collection](./library-management-backend/Resources/Postman)

Each service has its own dockerfile, so to run the entire project, just run the external docker-compose:

```bash
docker-compose up -d
```

if an error occurs during startup, you need to remove the frontend and backend services from docker-compose, run postgresql and redis, and then run the frontend and backend locally separately:

```bash
docker-compose up -d
dotnet run
npm start
```
