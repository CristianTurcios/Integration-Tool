-- -----------------------------------------------------
-- DATABASE IntegrationTool
-- -----------------------------------------------------
-- DROP DATABASE IntegrationTool;

-- -----------------------------------------------------
-- DATABASE IntegrationTool
-- -----------------------------------------------------
-- CREATE DATABASE IntegrationTool;
-- USE IntegrationTool;

-- -----------------------------------------------------
-- Table Engines
-- -----------------------------------------------------
CREATE TABLE Engines (
  EngineId INT NOT NULL IDENTITY(1,1),
  Name VARCHAR(80) NOT NULL,
  CONSTRAINT PK_Engines
    PRIMARY KEY (EngineId));


-- -----------------------------------------------------
-- Table Status
-- -----------------------------------------------------
CREATE TABLE Status (
  StatusId INT NOT NULL IDENTITY(1,1),
  Name VARCHAR(80) NOT NULL,
  CONSTRAINT PK_Status
    PRIMARY KEY (StatusId));


-- -----------------------------------------------------
-- Table DatabaseParameters
-- -----------------------------------------------------
CREATE TABLE DatabaseParameters (
  DatabaseParametersId INT NOT NULL IDENTITY(1,1),
  Ip VARCHAR(30) NOT NULL,
  Port VARCHAR(20),
  Instance VARCHAR(80),
  Name VARCHAR(80) NOT NULL,
  Username VARCHAR(80) NOT NULL,
  Password VARCHAR(80) NOT NULL,
  EngineId INT NOT NULL,
  CONSTRAINT PK_DatabaseParameter
    PRIMARY KEY (DatabaseParametersId),
  CONSTRAINT FK_DatabaseParametersEngines_EngineId
    FOREIGN KEY (EngineId)
    REFERENCES Engines (EngineId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table Resource
-- -----------------------------------------------------
CREATE TABLE Resources (
  ResourceId INT NOT NULL IDENTITY(1,1),
  Name VARCHAR(80) NOT NULL,
  CONSTRAINT PK_Resource
    PRIMARY KEY (ResourceId));


-- -----------------------------------------------------
-- Table UsersType
-- -----------------------------------------------------
CREATE TABLE UsersType (
  UserTypeId INT NOT NULL IDENTITY(1,1),
  Type VARCHAR(20) NOT NULL,
  CONSTRAINT PK_UserType
    PRIMARY KEY (UserTypeId));


-- -----------------------------------------------------
-- Table Users
-- -----------------------------------------------------
CREATE TABLE Users (
  UserId INT NOT NULL IDENTITY(1,1),
  Name VARCHAR(80) NOT NULL,
  Username VARCHAR(80) NOT NULL,
  Password VARCHAR(80) NOT NULL DEFAULT 'wsfHCXPULUlICR67Did9BA==',
  Email VARCHAR(80) NOT NULL,
  UserTypeId INT NOT NULL,
  StatusId INT NOT NULL,
  CONSTRAINT PK_User
    PRIMARY KEY (UserId),
  CONSTRAINT UQ_Username
    UNIQUE(Username),
  CONSTRAINT FK_UsersUsersType_UserTypeId
    FOREIGN KEY (UserTypeId)
    REFERENCES UsersType (UserTypeId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_UsersStatus_StatusId
    FOREIGN KEY (StatusId)
    REFERENCES Status (StatusId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table WebServices
-- -----------------------------------------------------
CREATE TABLE WebServices (
  WebServiceId INT NOT NULL IDENTITY(1,1),
  Endpoint VARCHAR(MAX) NOT NULL,
  Username VARCHAR(80) NOT NULL,
  Password VARCHAR(80) NOT NULL,
  CONSTRAINT PK_WebService
    PRIMARY KEY (WebServiceId));


-- -----------------------------------------------------
-- Table OperationsWebServices
-- -----------------------------------------------------
CREATE TABLE OperationsWebServices (
  OperationWebServiceId INT NOT NULL IDENTITY(1,1),
  Name VARCHAR(50) NOT NULL,
  Identifier VARCHAR(50) NOT NULL,
  CONSTRAINT OperationWebServiceId
    PRIMARY KEY (OperationWebServiceId));


-- -----------------------------------------------------
-- Table IntegrationsType
-- -----------------------------------------------------
CREATE TABLE IntegrationsType (
  IntegrationTypeId INT NOT NULL IDENTITY(1,1),
  Name VARCHAR(50) NOT NULL,
  Identifier VARCHAR(50) NOT NULL,
  CONSTRAINT PK_IntegrationType
    PRIMARY KEY (IntegrationTypeId));


-- -----------------------------------------------------
-- Table Queries
-- -----------------------------------------------------
CREATE TABLE Queries (
  QueryId INT NOT NULL IDENTITY(1,1),
  Query VARCHAR(MAX) NOT NULL,
  QueryName VARCHAR(200) NOT NULL,
  Description VARCHAR(MAX) NOT NULL,
  IntegrationTypeId INT NOT NULL,
  CONSTRAINT PK_Query
    PRIMARY KEY (QueryId),
  CONSTRAINT FK_QueriesIntegrationsType_IntegrationTypeId
    FOREIGN KEY (IntegrationTypeId)
    REFERENCES IntegrationsType (IntegrationTypeId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table ServerSMTPParameters
-- -----------------------------------------------------
CREATE TABLE ServerSMTPParameters (
  ServerSMTPParametersId INT NOT NULL IDENTITY(1,1),
  NameServerSMTP VARCHAR(80) NOT NULL,
  Port VARCHAR(80) NOT NULL,
  UsernameSMTP VARCHAR(80) NOT NULL,
  PasswordSMTP VARCHAR(80) NOT NULL,
  EmailFrom VARCHAR(200) NOT NULL,
  Subject VARCHAR(MAX) NOT NULL,
  Body VARCHAR(MAX) NOT NULL,
  CONSTRAINT PK_EmailParameter
    PRIMARY KEY (ServerSMTPParametersId));


-- -----------------------------------------------------
-- Table FlatFilesParameters
-- -----------------------------------------------------
CREATE TABLE FlatFilesParameters (
  FlatFileParameterId INT NOT NULL IDENTITY(1,1),
  Location VARCHAR(MAX) NOT NULL,
  CONSTRAINT PK_FlatFilesParameters
    PRIMARY KEY (FlatFileParameterId));


  -- -----------------------------------------------------
  -- Table PathReports
  -- -----------------------------------------------------
  CREATE TABLE FlatFiles (
    FlatFileId INT NOT NULL IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    CONSTRAINT PK_FlatFileId
      PRIMARY KEY (FlatFileId));


-- -----------------------------------------------------
-- Table PathReports
-- -----------------------------------------------------
CREATE TABLE PathReports (
  PathReportId INT NOT NULL IDENTITY(1,1),
  Location VARCHAR(100) NOT NULL,
  CONSTRAINT PK_PathReport
    PRIMARY KEY (PathReportId));

-- -----------------------------------------------------
-- Table IntegrationCategories
-- -----------------------------------------------------
CREATE TABLE IntegrationCategories (
  IntegrationCategoryId INT NOT NULL IDENTITY(1,1),
  Name VARCHAR(50) NOT NULL,
  CONSTRAINT PK_IntegrationCategory
    PRIMARY KEY (IntegrationCategoryId));


-- -----------------------------------------------------
-- Table Integrations
-- -----------------------------------------------------
CREATE TABLE Integrations (
  IntegrationId INT NOT NULL IDENTITY(1,1),
  IntegrationName VARCHAR(200) NOT NULL,
  CurlParameters VARCHAR(120),
  IntegrationDate DATETIME NOT NULL,
  UserId INT NOT NULL,
  WebServiceId INT NOT NULL,
  OperationWebServiceId INT NOT NULL,
  DatabaseParametersId INT NOT NULL,
  FlatFileId INT NOT NULL,
  FlatFileParameterId INT NOT NULL,
  IntegrationTypeId INT NOT NULL,
  IntegrationCategoryId INT NOT NULL,
  QueryId INT NOT NULL,
  StatusId INT NOT NULL,
  CONSTRAINT PK_Integration
    PRIMARY KEY (IntegrationId),
  CONSTRAINT FK_IntegrationsUsers_UserId
    FOREIGN KEY (UserId)
    REFERENCES Users (UserId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_IntegrationsWebServices_WebServiceId
    FOREIGN KEY (WebServiceId)
    REFERENCES WebServices (WebServiceId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_IntegrationsOperationsWebServices_OperationWebServiceId
    FOREIGN KEY (OperationWebServiceId)
    REFERENCES OperationsWebServices (OperationWebServiceId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_IntegrationsDatabaseParameters_DatabaseParametersId
    FOREIGN KEY (DatabaseParametersId)
    REFERENCES DatabaseParameters (DatabaseParametersId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_IntegrationsFlatFiles_FlatFileId
    FOREIGN KEY (FlatFileId)
    REFERENCES FlatFiles (FlatFileId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_IntegrationsFlatFilesParameters_FlatFileParameterId
    FOREIGN KEY (FlatFileParameterId)
    REFERENCES FlatFilesParameters (FlatFileParameterId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_IntegrationsIntegrationsType_IntegrationTypeId
    FOREIGN KEY (IntegrationTypeId)
    REFERENCES IntegrationsType (IntegrationTypeId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_IntegrationsIntegrationCategories_IntegrationCategoryId
    FOREIGN KEY (IntegrationCategoryId)
    REFERENCES IntegrationCategories (IntegrationCategoryId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_IntegrationsQueries_QueryId
    FOREIGN KEY (QueryId)
    REFERENCES Queries (QueryId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_IntegrationsStatus_StatusId
    FOREIGN KEY (StatusId)
    REFERENCES Status (StatusId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table QueryParameters
-- -----------------------------------------------------
CREATE TABLE QueryParameters (
  QueryParameterId INT NOT NULL IDENTITY(1,1),
  Name VARCHAR(100) NOT NULL,
  Value VARCHAR(max) NOT NULL,
  IntegrationId INT NOT NULL,
  CONSTRAINT PK_QueryParameters
    PRIMARY KEY (QueryParameterId),
  CONSTRAINT FK_QueryParametersIntegrations_IntegrationId
    FOREIGN KEY (IntegrationId)
    REFERENCES Integrations (IntegrationId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table ActiveDirectoryParameters
-- -----------------------------------------------------
CREATE TABLE ActiveDirectoryParameters (
  ActiveDirectoryId INT NOT NULL IDENTITY(1,1),
  ADPath VARCHAR(MAX) NOT NULL,
  ADDomain VARCHAR(80) NOT NULL,
  CONSTRAINT PK_ActiveDirectoryParameter
    PRIMARY KEY (ActiveDirectoryId));


-- -----------------------------------------------------
-- Table Keys
-- -----------------------------------------------------
CREATE TABLE Keys (
  KeyId INT NOT NULL IDENTITY(1,1),
  KeyValue VARCHAR(200) NOT NULL,
  Description VARCHAR(500) NOT NULL,
  CONSTRAINT PK_Key
    PRIMARY KEY (KeyId));


-- -----------------------------------------------------
-- Table Permissions
-- -----------------------------------------------------
CREATE TABLE Permissions (
  PermissionId INT NOT NULL IDENTITY(1,1),
  UserId INT NOT NULL,
  ResourceId INT NOT NULL,
  CONSTRAINT PK_Permission
    PRIMARY KEY (PermissionId),
  CONSTRAINT FK_PermissionsUsers_UserId
    FOREIGN KEY (UserId)
    REFERENCES Users (UserId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_PermissionsResources_ResourceId
    FOREIGN KEY (ResourceId)
    REFERENCES Resources (ResourceId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table IntegrationLogs
-- -----------------------------------------------------
CREATE TABLE IntegrationLogs (
  LogId INT NOT NULL IDENTITY(1,1),
  Description VARCHAR(MAX) NOT NULL,
  ErrorDate DATETIME NOT NULL,
  IntegrationId INT NOT NULL,
  CONSTRAINT PK_IntegrationLog
    PRIMARY KEY (LogId),
  CONSTRAINT FK_IntegrationLogsIntegrations_IntegrationId
    FOREIGN KEY (IntegrationId)
    REFERENCES Integrations (IntegrationId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table Recurrences
-- -----------------------------------------------------
CREATE TABLE Recurrences (
  RecurrenceId INT NOT NULL IDENTITY(1,1),
  NameRecurrence VARCHAR(20) NOT NULL,
  CONSTRAINT PK_Recurrence
    PRIMARY KEY (RecurrenceId));


-- -----------------------------------------------------
-- Table Calendars
-- -----------------------------------------------------
CREATE TABLE Calendars (
  CalendarId INT NOT NULL IDENTITY(1,1),
  ExecutionStartDate DATETIME NOT NULL,
  NextExecutionDate DATETIME NOT NULL,
  ExecutionEndDate DATETIME NOT NULL,
  Emails VARCHAR(MAX),
  IntegrationId INT NOT NULL,
  RecurrenceId INT NOT NULL,
  CONSTRAINT PK_Calendar
    PRIMARY KEY (CalendarId),
  CONSTRAINT FK_CalendarsIntegrations_IntegrationId
    FOREIGN KEY (IntegrationId)
    REFERENCES Integrations (IntegrationId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_CalendarsRecurrences_RecurrenceId
    FOREIGN KEY (RecurrenceId)
    REFERENCES Recurrences (RecurrenceId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table System_Logs
-- -----------------------------------------------------
CREATE TABLE SystemLogs (
  SystemLogId INT NOT NULL IDENTITY(1,1),
  Description VARCHAR(MAX) NOT NULL,
  ErrorDate DATETIME NOT NULL,
  IntegrationId INT NOT NULL,
  CONSTRAINT PK_SystemLog
    PRIMARY KEY (SystemLogId),
  CONSTRAINT FK_SystemLogsIntegrations_IntegrationId
    FOREIGN KEY (IntegrationId)
    REFERENCES Integrations (IntegrationId)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Inserts
-- -----------------------------------------------------
INSERT INTO Engines VALUES
  ('SQLSERVER'),
  ('MYSQL');

INSERT INTO FlatFiles VALUES
  ('Default');

INSERT INTO IntegrationCategories VALUES
  ('Manual'),
  ('Scheduled');

INSERT INTO IntegrationsType VALUES
  ('Course',	'course'),
  ('Course Association',	'courseassociation'),
  ('Course Category',	'coursecategory'),
  ('Course Category Membership',	'coursecategorymembership'),
  ('Course Membership',	'membership'),
  ('Course Standard Association',	'standardsassociation'),
  ('Hierarchy Node',	'node'),
  ('Observer Association',	'associateobserver'),
  ('Organization',	'organization'),
  ('Organization Association',	'organizationassociation'),
  ('Organization Category',	'organizationcategory'),
  ('Organization Category Membership',	'organizationcategorymembership'),
  ('Organization Membership',	'organizationmembership'),
  ('Person',	'person'),
  ('Term', 'term'),
  ('User Association',	'userassociation'),
  ('User Secondary Institution Role',	'secondaryinstrole');

INSERT INTO Keys VALUES
  ('Laureate123*LNO321', 'Key used to encrypt data.');

INSERT INTO OperationsWebServices VALUES
  ('Store',	'store'),
  ('Complete Refresh',	'refresh'),
  ('Complete Refresh by Data Source',	'refreshlegacy'),
  ('Delete',	'delete');

INSERT INTO Recurrences VALUES
  ('Hourly'),
  ('Daily'),
  ('Weekly'),
  ('Monthly');

INSERT INTO Resources VALUES
  ('Configuration'),
  ('Integrations'),
  ('Users'),
  ('Reports'),
  ('Logs');

INSERT INTO Status VALUES
  ('Enable'),
  ('Disable');

INSERT INTO UsersType VALUES
  ('Local'),
  ('Active Directory');

INSERT INTO Users VALUES
  ('Admin',	'admin',	'o5467lbMI4DGvgtntlbTcQ==',	'admin@email.com',	1,	1);

INSERT INTO Permissions VALUES
  (1,1),
  (1,2),
  (1,3),
  (1,4),
  (1,5);
