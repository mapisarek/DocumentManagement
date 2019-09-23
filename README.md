# Document Management System

Final project created during classes in programming ASP.NET web technology.
The task received from the teacher was to create a system that would allow the company to store documents.
I have done the research of the necessary requirements and in 3 days I have completed the following features presented below.

## Features

* Logging into the system,

### Admin options:
* Possibility to add and remove system users,
* The ability to add and delete documents in the system,
* Possibility to add and remove categories according to which documents are added,
* The ability to inform system users about a newly uploaded file to the system and choosing a specific document to send to users

### User & Admin options:
* The ability to search and filter the document database,
* Displaying the details of a selected document from the system,
* The ability to download the selected document from the system,
* The ability to send the viewed file to your own company email account

## How it's done
The system has two roles (user, administrator) which are stored in the database and read when logging in. The files are added with the following information: title, category, date the document was added and a brief description that are stored in the database along with the path to the file (the file itself is stored on the server in the wwwroot / Documents folder).
In the Services folder there is the EmailService class which is responsible for the connection with AWS SES (Simple Email System) and the message itself as an HTML template is also sent from the above service.


## Gallery

### Pictures shown below are available in a reduced resolution, full pictures are avalible in Preview folder inside repository.

<img src="https://github.com/mapisarek/DocumentManagement/blob/master/Preview/LoginForm.png" width=700 height=400/>
<img src="https://github.com/mapisarek/DocumentManagement/blob/master/Preview/AddDocument.png" width=700 height=400/>
<img src="https://github.com/mapisarek/DocumentManagement/blob/master/Preview/MailTemplate.PNG">
