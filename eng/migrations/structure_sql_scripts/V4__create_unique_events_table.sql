USE [MyCompany]
GO

CREATE TABLE UniqueEvents
(
    EventId        UNIQUEIDENTIFIER       NOT NULL,
    DateConsumed   Datetime2              NOT NULL,
    EventType      VARCHAR(255)           NOT NULL,
    ConsumerAction VARCHAR(255)           NOT NULL,
    InternalId     BIGINT IDENTITY (1, 1) NOT NULL,
    DataVersion    ROWVERSION,
    CONSTRAINT EventId_ConsumerAction_PK PRIMARY KEY (EventId, ConsumerAction)
);

GO
CREATE INDEX idx_internal_event_type_id ON UniqueEvents (InternalId DESC, EventType);
