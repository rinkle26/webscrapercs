# webscrapercs
Webscraper written in c#

## Prerequisites

### 1. PostgreSQL

    1. Install PostgreSQL 9.6.10 from this link : https://www.enterprisedb.com/downloads/postgres-postgresql-downloads

### 2. VS Code

    1. Install VSCode from this link : https://code.visualstudio.com/

### 3. Git

    1. Install git from this link : https://git-scm.com/downloads
    
### 4. PGAdmin III

    1. Download PGAdmin III from this link : https://www.pgadmin.org/download/
    
    2. Make sure you are able to connect to the database using PGAdmin III

## Steps to install:

    1. Clone this repo into a local folder using the following command : > git clone https://github.com/dc297/webscrapercs

    2. A new folder named webscrapercs would be created.

    3. Open PGAdmin III. Create a new database named webscrapercs

    4. Right click on the newly created database and choose restore. PGAdmin III will ask you to select a file. Select webscrapercs.backup from the newly created folder. Click restore.

    5. You should be able to see 11 new tables added to the database

    6. Open the newly created folder in VS Code. There would be an option under file menu.

    7. VS Code would automatically tell you that there are unresolved dependencies for the project and it'll ask you to add/restore them. Add/Restore those.

    8. Copy the sample.config to App.config and update the details of your PostgreSQL installation in App.config.

    9. Open a new terminal and execute > dotnet run

    10. That's it. You should be able to see the parser logs in the terminal. And new entries would be added in the url, property and propertytype talbles in the database. You can check the new data using PGAdmin III.
