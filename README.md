# TO DO List 

# Demo Video
[DEMO-TODO.webm](https://github.com/mclaramarinho/ToDoAspNet/assets/119897667/8817e8c9-1165-4612-860e-7fd19893a83a)


# Features
- Create tasks (title + description)
- Edit tasks (title + description)
- Delete tasks
- View details
- Table and Board Views
- Change task status (To Do / Doing / Finished)
- Filter tasks by status
- Create user (NEW)
- Login with email + password (NEW)
- Logout (NEW)

## DON'T FORGET!
### Change the server name in file ``` appsettings.json ```
```
"ConnectionStrings": {
  "DefaultConnection": "Server= <YOUR SERVER NAME HERE> ;Database=toDoListDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### After changing the server name, do the following in the Package Manager Console
```add-migration <NameTheMigration>```

```update-database```
