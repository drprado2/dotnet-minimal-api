CREATE TABLE Company
(
    CompanyID          uuid       NOT NULL PRIMARY KEY,
    Name               VARCHAR(255)           NOT NULL,
    Document           VARCHAR(255)           NOT NULL,
    Logo               VARCHAR(2000)          NOT NULL,
    PrimaryColor       VARCHAR(20)            NOT NULL,
    PrimaryFontColor   VARCHAR(20)            NOT NULL,
    SecondaryColor     VARCHAR(20)            NOT NULL,
    SecondaryFontColor VARCHAR(20)            NOT NULL,
    TotalCollaborators integer                 NOT NULL DEFAULT 0,
    Active             BOOLEAN                    NOT NULL,
    CreatedAt          TIMESTAMPTZ              NOT NULL,
    UpdatedAt          TIMESTAMPTZ              NULL,
    InternalId         SERIAL NOT NULL
);

CREATE INDEX idx_internal_id ON Company (InternalId DESC);

CREATE UNIQUE  INDEX idx_document_unique ON Company (Document);
