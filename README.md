# File cabinet application
[Source](https://github.com/epam-dotnet-lab/file-cabinet-task).
## Command line parameters for FileCabinetApp:

| Full Parameter Name | Short Parameter Name | Description                                   |
|---------------------|----------------------|-----------------------------------------------|
| validation-rules    | v                    | Rules for input parameters (default, custom). |
| storage             | s                    | Type of service (memory, file).               |
| use-logger          |                      | Create log for every command.                 |
| use-stopwat         |                      | Prints the execution time of each command.    |

#### FileCabinetMemoryService.
Provides memory service for working with records.
#### FileCabinetFilesystemService.
Provides filesystem service for working with records in binary format.
## Available command:
### create
Creating new record. 
### export
Exporting data of record into specific doc type. 
- csv 
  - > export csv filename.csv
  - > export csv e:\filename.csv
- xml 
  - > export xml filename.xml

### import 
Example:
- csv
```sh
> import csv d:\data\records.csv
10000 records were imported from d:\data\records.csv.
> import csv d:\data\records2.csv
Import error: file d:\data\records.csv is not exist.
```
- xml
```sh
> import xml c:\users\myuser\records.xml
5000 records were imported from c:\users\myuser\records.xml.

### purge
Example:
```sh
> purge
Data file processing is completed: 10 of 100 records were purged.
```

### insert 
Example:
```
> insert(id, firstname, lastname, dateofbirth, salary, accounttype, bonuses) values('1', 'John', 'Doe', '05/08/1986', '3500', 'd', '12')
```
### delete
Example:
```
> delete where id = '1'
Record #1 is deleted.
> delete where LastName='Smith'
Records #2, #3, #4 are deleted. 
```
### update
Example:
```
> update set firstname = 'John', lastname = 'Doe' , dateofbirth = '5/18/1986' where id = '1'
> update set DateOfBirth = '5/18/1986' where FirstName='Stan' and LastName='Smith'
```

### select 
Example:
```
> select id, firstname, lastname where firstname = 'John' and lastname = 'Doe'
+----+-----------+----------+
| Id | FirstName | LastName |
+----+-----------+----------+
|  1 | John      | Doe      |
+----+-----------+----------+
```

### help
Example:
```

> help
Available commands:
        help    - prints the help screen
        exit    - exits the application
        stat    - show stats
        create  - create new record
        insert  - insert new record
        update  - update record
        export  - export records in file(csv/xml))
        import  - import records from file(csv/xml))
        delete  - delete record
        purge   - purge records
        select  - select records
```

## Command line parameters for FileCabinetGenerator.
| Full Parameter Name | Short Parameter Name | Description                    |
|---------------------|----------------------|--------------------------------|
| output-type         | t                    | Output format type (csv, xml). |
| output              | o                    | Output file name.              |
| records-amount      | a                    | Amount of generated records.   |
| start-id            | i                    | ID value to start.             |

Example:
```sh
$ FileCabinetGenerator.exe --output-type=csv --output=d:\data\records.csv --records-amount=10000 --start-id=30
10000 records were written to records.csv.
$ FileCabinetGenerator.exe -t:xml -o:c:\users\myuser\records.xml -a:5000 -i:45
5000 records were written to c:\users\myuser\records.xml
```
