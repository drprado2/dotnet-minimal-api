CREATE TABLE Employee
(
    EmployeeID         uuid       NOT NULL PRIMARY KEY,
    IdentityUserId     uuid       NOT NULL,
    CompanyId          uuid       NOT NULL,
    Name               VARCHAR(255)           NOT NULL,
    Email              VARCHAR(255)           NOT NULL,
    Phone              VARCHAR(2000)          NULL,
    BirthDate          DATE                   NULL,
    RecordCreatedCount integer                 NOT NULL DEFAULT 0,
    RecordEditedCount  integer                 NOT NULL DEFAULT 0,
    RecordDeletedCount integer                 NOT NULL DEFAULT 0,
    Active             BOOLEAN                    NOT NULL,
    CreatedAt          TIMESTAMPTZ              NOT NULL,
    UpdatedAt          TIMESTAMPTZ              NULL,
    InternalId         SERIAL NOT NULL,
    FOREIGN KEY (CompanyId) REFERENCES Company (CompanyID)
);

CREATE INDEX employee_idx_internal_id ON Employee (InternalId DESC);