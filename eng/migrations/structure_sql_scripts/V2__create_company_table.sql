USE [MyCompany]
GO

CREATE TABLE Company
(
    CompanyID          UNIQUEIDENTIFIER       NOT NULL PRIMARY KEY,
    Name               VARCHAR(255)           NOT NULL,
    Document           VARCHAR(255)           NOT NULL,
    Logo               VARCHAR(2000)          NOT NULL,
    PrimaryColor       VARCHAR(20)            NOT NULL,
    PrimaryFontColor   VARCHAR(20)            NOT NULL,
    SecondaryColor     VARCHAR(20)            NOT NULL,
    SecondaryFontColor VARCHAR(20)            NOT NULL,
    TotalCollaborators Bigint                 NOT NULL DEFAULT 0,
    Active             BIT                    NOT NULL,
    CreatedAt          Datetime2              NOT NULL,
    UpdatedAt          Datetime2              NULL,
    InternalId         BIGINT IDENTITY (1, 1) NOT NULL,
    DataVersion        ROWVERSION
);

GO

CREATE INDEX idx_internal_id ON Company (InternalId DESC);

GO

CREATE UNIQUE  INDEX idx_document_unique ON Company (Document);
