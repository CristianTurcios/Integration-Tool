Integration Tool

El siguiente "Readme" detalla la funcionalidad y especificaciones técnicas que se requieren para realizar un proceso de integración entre distintos servidores de bases de datos ya sea MySql o SQL Server hacia una plataforma BlackBoard.

Esta es una aplicación web creada en los lenguajes asp.net, c#, angular js, Html para el maquetado y bootstrap para el diseño de la aplicacion y una base de datos sql server

La aplicacion se divide en dos elementos:
1- Plataforma web
2- Un servicio de windows 


1.Servicio de Windows: es el encargado de revisar en la base de datos la configuración de una integracion calendarizada, este servicio realiza cada minuto una consulta a la base de datos y determina si la fecha en la que esta guardada la calendarización automática coincide con la fecha del Sistema, entonces realiza la integracion con los parámetros que estén almacenados en la base de datos.

2.Plataforma web: desde donde se realizan las configuraciones de los distintos parámetros del sistema, se ejecutan integraciones manuales, se programan integraciones automáticas, se gestionan los usuarios del sistema y finalmente se generan bitácoras y reportes. 
La plataforma web está desarrollada en un entorno ASP.NET y para esto se necesita un servidor IIS en Windows Server para poder servir la página de forma exitosa. 
