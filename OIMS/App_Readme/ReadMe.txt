-------------------------------
Developer Notes for Deployment
-------------------------------

ON DATE : 30-09-2014
BY DEVELOPER : Vikram Singh Saini
EMAIL : vs00saini@gmail.com
CONTACT : 9829478687

---------------
DATABASE
---------------

1. Open HeidiSql --> Create database named as inventory with utf8 encoding.
2. Select database and open 'Query' tab.
3. In blank space, right click, select option 'Load from Sql file' --> select the Oims Database Script.sql file.
4. Execute queries by F9. Now database is ready with tables and some basic data.
5. By default user is admin@gmail.com and password is 123456 for 'Supervisor' role.

---------------
DEPLOYMENT
---------------

1. Publish the website to some specific directory.
2. While most of the dlls goes in bin directory.
3. The following files also need to be copied to 'bin' directory of published folder - 

(a) Elmah.Mvc.dll - Require for watching error log by http://localhost/oims/elmah
(b) Microsoft.Web.Infrastructure.dll
(c) System.Web.Razor.dll
(d) System.Web.WebPages.Deployment.dll
(e) Newtonsoft.Json.dll

4. The following are necessary for 'MySql' part - 

(a) MySql.Web.dll - v6.8.3.0
(b) MySql.Data.dll - v6.8.3.0

5. The following are necessary for 'Telerik ORM' part - 

(a) Telerik.OpenAccess.dll
(b) Telerik.OpenAccess.Runtime.dll

** Note - Rest of third party dlls get copied automatically.

------------------------------
CONFIGURE 'ELMAH' FOR MySql
------------------------------

1. From Package Manager Console : Install-Package elmah.mysql
2. Run scripts Elmah.MySql.sql.
3. Configure Elmah connection string accordingly.
