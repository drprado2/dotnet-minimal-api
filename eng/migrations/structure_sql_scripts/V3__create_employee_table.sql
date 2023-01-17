USE [MyCompany]
GO

CREATE TABLE Employee
(
    EmployeeID         UNIQUEIDENTIFIER       NOT NULL PRIMARY KEY,
    IdentityUserId     UNIQUEIDENTIFIER       NOT NULL,
    CompanyId          UNIQUEIDENTIFIER       NOT NULL FOREIGN KEY REFERENCES Company (CompanyID),
    Name               VARCHAR(255)           NOT NULL,
    Email              VARCHAR(255)           NOT NULL,
    Phone              VARCHAR(2000)          NULL,
    BirthDate          DATE                   NULL,
    RecordCreatedCount Bigint                 NOT NULL DEFAULT 0,
    RecordEditedCount  Bigint                 NOT NULL DEFAULT 0,
    RecordDeletedCount Bigint                 NOT NULL DEFAULT 0,
    Active             BIT                    NOT NULL,
    CreatedAt          Datetime2              NOT NULL,
    UpdatedAt          Datetime2              NULL,
    InternalId         BIGINT IDENTITY (1, 1) NOT NULL,
    DataVersion        ROWVERSION
);

GO

CREATE INDEX idx_internal_id ON Employee (InternalId DESC);